
using AuthService.Domain.Contracts;
using AuthService.Domain.Entities;
using Grpc.Core;
using Microsoft.AspNetCore.Identity;

namespace AuthService.GrpcServer.Services;

public class AuthServiceImpl : AuthService.AuthServiceBase
{
    private readonly IAuthRepository _authRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthServiceImpl(IAuthRepository authRepository, UserManager<ApplicationUser> userManager)
    {
        _authRepository = authRepository;
        _userManager = userManager;
    }

    public override async Task<SaveTelegramIdResponse> SaveTelegramId(SaveTelegramIdRequest request, ServerCallContext context)
    {
        await _authRepository.SaveTelegramIdAsync(request.UserId, request.TelegramId);
        return new SaveTelegramIdResponse { Success = true };
    }
    
    public override async Task<GetTelegramIdResponse> GetTelegramIdByUserId(GetTelegramIdRequest request, ServerCallContext context)
    {
        var telegramId = await _authRepository.GetTelegramIdByUserIdAsync(request.UserId);
        return new GetTelegramIdResponse { TelegramId = telegramId ?? 0 };
    }

    public override async Task<DeleteUserResponse> DeleteUserById(DeleteUserRequest request, ServerCallContext context)
    {
        var user = await _userManager.FindByIdAsync(request.UserId)
            ?? throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
        var isDeleted = await _userManager.DeleteAsync(user);
        return new DeleteUserResponse { Success = isDeleted.Succeeded };
    }
}