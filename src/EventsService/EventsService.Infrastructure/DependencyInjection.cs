// <copyright file="DependencyInjection.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using EventsService.Application.UseCases.Dates.Commands.CreateDate;

namespace EventsService.Infrastructure;

using EventsService.Application.Mappers;
using EventsService.Domain.Contracts;
using EventsService.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        services.AddScoped<IMeetingRepository, MeetingRepository>();
        services.AddScoped<IDateRepository, DateRepository>();
        services.AddScoped<IGoalRepository, GoalRepository>();
        services.AddAutoMapper(typeof(DateToDateDto));
        services.AddAutoMapper(typeof(UpdateDateDtoToDate));
        services.AddAutoMapper(typeof(GoalToGoalDto));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateDateHandler).Assembly));
        return services;
    }
}