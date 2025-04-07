// <copyright file="DependencyInjection.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EventsService.Infrastructure;

using EventsService.Application.Mappers;
using EventsService.Application.UseCases.Dates.Commands.CreateDate;
using EventsService.Domain.Contracts;
using EventsService.Domain.Entities;
using EventsService.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        services.AddScoped<IRepository<Date>, DateRepository>();
        services.AddScoped<IRepository<Meeting>, MeetingRepository>();
        services.AddScoped<IRepository<Goal>, GoalRepository>();
        services.AddScoped<IMeetingRepository, MeetingRepository>();
        services.AddScoped<IDateRepository, DateRepository>();
        services.AddScoped<IGoalRepository, GoalRepository>();
        services.AddAutoMapper(typeof(DateToDateDto));
        services.AddAutoMapper(typeof(UpdateDateDtoToDate));
        services.AddAutoMapper(typeof(GoalToGoalDto));
        services.AddAutoMapper(typeof(MeetingToMeetingDto));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateDateHandler).Assembly));
        return services;
    }
}