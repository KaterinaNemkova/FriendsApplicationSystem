namespace EventsService.Infrastructure;

using System.Reflection;
using EventsService.Application.Mappers;
using EventsService.Application.Mappers.Dates;
using EventsService.Application.Mappers.Goals;
using EventsService.Application.Mappers.Meetings;
using EventsService.Application.UseCases.Dates.Commands.CreateDate;
using EventsService.Application.Validators;
using EventsService.Domain.Contracts;
using EventsService.Domain.Entities;
using EventsService.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
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

        services.AddAutoMapper(typeof(DateMapper));
        services.AddAutoMapper(typeof(GoalMapper));
        services.AddAutoMapper(typeof(MeetingMapper));

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateDateHandler).Assembly));

        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssembly(typeof(DateValidator).Assembly);

        return services;
    }
}