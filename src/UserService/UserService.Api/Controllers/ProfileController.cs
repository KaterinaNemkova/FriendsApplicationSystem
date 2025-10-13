using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UserService.Application.Common.Exceptions;
using UserService.Application.UseCases.Profiles.Commands.DeleteProfile;

namespace UserService.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.DTOs;
using UserService.Application.UseCases.Profiles.Commands.DeleteImage;
using UserService.Application.UseCases.Profiles.Commands.EstablishStatus;
using UserService.Application.UseCases.Profiles.Commands.UploadImage;
using UserService.Application.UseCases.Profiles.Queries.GetAllByFilter;
using UserService.Application.UseCases.Profiles.Queries.GetPhoto;
using UserService.Application.UseCases.Profiles.Queries.GetProfileById;
using UserService.Domain.Enums;

[ApiController]
[Route("api/profiles")]
public class ProfileController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProfileController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
    {
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
    }
    
    [HttpGet("profile")]

    public async Task<IActionResult> GetProfileById(CancellationToken token)
    {
        var profileId = GetProfileId();
        var profile = await _mediator.Send(new GetProfileByIdQuery(profileId), token);

        return Ok(profile);
    }

    [HttpGet]
    public async Task<IActionResult> GetProfilesByFilter([FromQuery] GetAllByFilterQuery query, CancellationToken token)
    {
        var profiles = await _mediator.Send(query, token);

        return Ok(profiles);
    }

    [HttpGet("photo/{profileId:guid}")]
    public async Task<IActionResult> GetProfilePhoto([FromRoute] Guid profileId, CancellationToken token)
    {
        var url = await _mediator.Send(new GetPhotoByIdQuery(profileId), token);

        return Ok(url);
    }

    [HttpPost("photo/{profileId:guid}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadProfilePhoto([FromRoute] Guid profileId, [FromForm] UploadImageRequest request,CancellationToken token)
    {
        var result = await _mediator.Send(new UploadImageCommand(profileId, request.File), token);

        return Ok(result);
    }

    [HttpDelete("photo/{profileId:guid}")]
    public async Task<IActionResult> DeletePhoto([FromRoute] Guid profileId, CancellationToken token)
    {
        var result = await _mediator.Send(new DeleteImageCommand(profileId), token);
        return Ok(result);
    }

    [HttpPost("status/{profileId:guid}")]
    public async Task<IActionResult> EstablishStatus([FromRoute] Guid profileId, [FromQuery] ActivityStatus activityStatus,CancellationToken token)
    {
        await _mediator.Send(new EstablishStatusCommand(profileId, activityStatus), token);
        return Ok();
    }

    [HttpDelete("profile/{profileId:guid}")]

    public async Task<IActionResult> DeleteProfile([FromRoute] Guid profileId, CancellationToken token)
    {
        await _mediator.Send(new DeleteProfileCommand(profileId), token);

        return Ok();
    }

    private Guid GetProfileId()
    {
        var token = _httpContextAccessor.HttpContext?.Request.Cookies["accessToken"];
        if (string.IsNullOrEmpty(token))
            throw new UnauthorizedException("Unauthorized.", "Token not found in cookies.");

        token = token.Replace("Bearer ", "");

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var profileIdValue = jwtToken.Claims.First(c => c.Type == "profile_id").Value;

        if (!Guid.TryParse(profileIdValue, out var profileId))
            throw new UnauthorizedException("Unauthorized.", "Invalid Profile ID format.");

        return profileId;
    }
}