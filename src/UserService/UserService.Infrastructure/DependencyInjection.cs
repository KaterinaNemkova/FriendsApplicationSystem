using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.UseCases.Profiles.Commands.UploadImage;
using UserService.Domain.Contracts;
using UserService.Infrastructure.Repositories;
using UserService.Infrastructure.Services;

namespace UserService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        services.AddScoped<IPhotoService, PhotoService>();
        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UploadImageHandler).Assembly)); 
        return services;
    }
}