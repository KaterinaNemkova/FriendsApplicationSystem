namespace UserService.Infrastructure;

using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Contracts;
using UserService.Application.Mappers;
using UserService.Application.UseCases.Profiles.Commands.UploadImage;
using UserService.Infrastructure.Repositories;
using UserService.Infrastructure.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        services.AddScoped<IPhotoService, PhotoService>();
        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddScoped<IFriendshipRepository, FriendshipRepository>();
        services.AddAutoMapper(typeof(ProfileToProfileDto));
        services.AddAutoMapper(typeof(FriendshipToFriendshipDto));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UploadImageHandler).Assembly));
        return services;
    }
}