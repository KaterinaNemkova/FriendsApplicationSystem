using System.Security.Claims;

namespace AuthService.Api.Endpoints;

using System.Text;
using AuthService.Application.Common;
using AuthService.Application.DTOs;
using AuthService.Domain.Contracts;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Services;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

public static class AccountEndpoint
{
    public static RouteGroupBuilder MapAccountEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("/api/account").WithTags("account");

        group.MapPost("/register", async (
            HttpContext context,
            [FromServices] IBackgroundJobClient backgroundJobClient,
            [FromServices] IDeleteUncorfimedUserService deleteUncorfimedUserService,
            [FromServices] UserManager<AppUser> userManager,
            [FromServices] IEmailSender emailSender,
            [FromBody] RegisterDto dto) =>
        {
            var userFromDb = await userManager.FindByEmailAsync(dto.Email);
            if (userFromDb is not null)
            {
                return Results.BadRequest(Response<string>.Failure("User is already exist"));
            }

            var user = new AppUser
            {
                UserName = dto.UserName,
                Email = dto.Email,
                FullName = dto.FullName,
            };

            var result = await userManager.CreateAsync(user, dto.Password!);

            if (!result.Succeeded)
            {
                return Results.BadRequest(
                    Response<string>.Failure(result.Errors.Select(x => x.Description).FirstOrDefault()!));
            }

            await userManager.AddToRoleAsync(user, "User");
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var baseUrl = Environment.GetEnvironmentVariable("BASE_URL") ?? "http://localhost:5100";
            var confirmationLink = $"{baseUrl}/confirm-email?userId={user.Id}&token={encodedToken}";
            var subject = "Register confirmation";
            var htmlMessage = $@"
        <h2>Welcome to our FriendsApp!</h2>
        <p>Please, confirm your registration, follow this link:</p>
        <p><a href='{confirmationLink}'>Confirm registration</a></p>
        <p>Link valid for 5 minutes.</p>
        <p>If you did not register, ignore this email.</p>";

            await emailSender.SendEmailAsync(user.Email, subject, htmlMessage);

            var userId = await userManager.GetUserIdAsync(user);

            backgroundJobClient.Schedule(() => deleteUncorfimedUserService.DeleteUnconfirmedUserAsync(userId), TimeSpan.FromMinutes(5));


            return Results.Ok(Response<ResponseRegisterDto>.Success(
                new ResponseRegisterDto(
                    Id: user.Id,
                    UserName: user.UserName,
                    FullName: user.FullName,
                    Email: user.Email,
                    TelegramId: user.TelegramId),
                "User created successfully. Please check your email for confirmation."));
        });

        group.MapGet("/confirm-email", async (
            [FromServices] UserManager<AppUser> userManager,
            [FromQuery] string userId,
            [FromQuery] string token,
            [FromServices] IUserStore<AppUser> userStore,
            [FromServices] UserService.GrpcServer.UserProfileService.UserProfileServiceClient userProfileClient) =>
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return Results.BadRequest("Invalid parameters");
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Results.NotFound("User not found");
            }

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await userManager.ConfirmEmailAsync(user, decodedToken);

            var botUsername = "FriendsNotificationBot";
            var telegramLink = $"https://t.me/{botUsername}?start={userId}";
            var userName = await userStore.GetUserNameAsync(user, CancellationToken.None);
            var request = new UserService.GrpcServer.CreateProfileRequest
            {
                UserId = userId,
                UserName = userName,
            };

            if (result.Succeeded)
            {
                var response = await userProfileClient.CreateProfileAsync(request);
                return Results.Ok("Email confirmed successfully! Link for your FriendsBot: " + telegramLink);
            }

            return Results.BadRequest("Email confirmation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        });

        group.MapPost("/login", async (
            HttpContext context,
            [FromServices] UserManager<AppUser> userManager,
            [FromServices] TokenService tokenservice,
            [FromBody] LoginDto dto,
            [FromServices] UserService.GrpcServer.UserProfileService.UserProfileServiceClient userProfileClient) =>
        {
            var user = await userManager.FindByEmailAsync(dto.Email);
            if (user is null)
            {
                return Results.BadRequest(Response<string>.Failure("User doesn't exist"));
            }

            if (!await userManager.IsEmailConfirmedAsync(user))
            {
                return Results.BadRequest(Response<string>.Failure("Please confirm your email first"));
            }

            var result = await userManager.CheckPasswordAsync(user!, dto.Password);

            if (!result)
            {
                return Results.BadRequest(Response<string>.Failure("Invalid password"));
            }

            var request = new UserService.GrpcServer.GetProfileIdRequest()
            {
                UserId = user.Id,
            };
            var roles = await userManager.GetRolesAsync(user);
            var response = await userProfileClient.GetProfileIdByUserIdAsync(request);
            var token = await tokenservice.GenerateAccessToken(user.Id, user.UserName!, response.ProfileId);

            context.Response.Cookies.Append(
                "accessToken",
                token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTime.Now.AddMinutes(5),
                    Path = "/",
                });

            return Results.Ok(Response<ResponseLoginDto>.Success(
                new ResponseLoginDto(
                    Token: token,
                    Expires: DateTime.UtcNow.AddMinutes(5),
                    UserId: user.Id,
                    UserName: user.UserName!,
                    Roles: roles),
                "Login successfully"));
        });

        group.MapPost("/validate-token", async (
            [FromBody] ValidateTokenRequest request,
            [FromServices] TokenService tokenService) =>
        {
            var principal = tokenService.ValidateToken(request.Token);

            if (principal == null)
            {
                return Results.Unauthorized();
            }

            var claims = principal.Claims.ToDictionary(c => c.Type, c => c.Value);

            return Results.Ok(new
            {
                IsValid = true,
                UserId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Username = principal.FindFirst(ClaimTypes.Name)?.Value,
                Claims = claims,
            });
        });

        return group;
    }

}