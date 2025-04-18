
using AuthService.Domain.Contracts;
using Grpc.Core;

namespace AuthService.GrpcServer.Services;

public class AuthServiceImpl : AuthService.AuthServiceBase
{
    private readonly IAuthRepository _authRepository;

    public AuthServiceImpl(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
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
}