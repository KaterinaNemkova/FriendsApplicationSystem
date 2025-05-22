using EventsService.Infrastructure.BackgroundJobs;
using Microsoft.AspNetCore.Builder;

namespace EventsService.Infrastructure.Extensions;

using EventsService.Application.Contracts;
using EventsService.Domain.Entities;
using EventsService.Infrastructure.Options;
using EventsService.Infrastructure.Services;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

public static class Extensions
{
    private static void RegisterMappings()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(Entity)))
        {
            BsonClassMap.RegisterClassMap<Entity>(
                cm =>
            {
                cm.MapIdMember(c => c.Id)
                    .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(Event)))
        {
            BsonClassMap.RegisterClassMap<Event>(
                cm =>
                {
                    cm.MapMember(c => c.ParticipantIds)
                        .SetSerializer(
                            new EnumerableInterfaceImplementerSerializer<List<Guid>>(
                                new GuidSerializer(GuidRepresentation.Standard)));
                    cm.MapMember(c => c.Title);
                    cm.MapMember(c => c.Description);
                });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(Meeting)))
        {
            BsonClassMap.RegisterClassMap<Meeting>(
                cm =>
                {
                    cm.AutoMap();

                    cm.MapMember(c => c.Author)
                        .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
                });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(Goal)))
        {
            BsonClassMap.RegisterClassMap<Goal>(
                cm =>
                {
                    cm.AutoMap();

                    cm.MapMember(c => c.Author)
                        .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
                });
        }
    }

    public static IServiceCollection AddDb(this IServiceCollection services, IConfiguration configuration)
    {
        configuration["ConnectionStrings:MongoDb"] = Environment.GetEnvironmentVariable("MONGO_DB2_CONNECTION_STRING") ?? string.Empty;
        var hangfireConnectionString = Environment.GetEnvironmentVariable("HANGFIRE2_CONNECTION");
        services.AddSingleton<IMongoClient>(
            new MongoClient(configuration.GetConnectionString("MongoDb")));
        services.AddHangfire(config =>
        {
            config.UseMongoStorage(
                hangfireConnectionString,
                "HangfireDatabase",
                new MongoStorageOptions
                {
                    CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection,
                    MigrationOptions = new MongoMigrationOptions
                    {
                        MigrationStrategy = new MigrateMongoMigrationStrategy(),
                        BackupStrategy = new CollectionMongoBackupStrategy(),
                    },
                    Prefix = "hangfire",
                    CheckConnection = true,
                });
        });

        services.AddHangfireServer();

        services.AddSingleton<IMongoDatabase>(
            sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase("EventsDatabase");
        });

        services.AddScoped(
            sp =>
            sp.GetRequiredService<IMongoDatabase>().GetCollection<Date>("Dates"));

        services.AddScoped(
            sp =>
            sp.GetRequiredService<IMongoDatabase>().GetCollection<Meeting>("Meetings"));

        services.AddScoped(
            sp =>
            sp.GetRequiredService<IMongoDatabase>().GetCollection<Goal>("Goals"));
        RegisterMappings();
        return services;
    }

    public static IServiceCollection AddRepresentation(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddHangfireServer();

        services.AddAuthentication();

        services.AddAuthorization();

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(
            c =>
        {
            c.EnableAnnotations();
        });

        return services;
    }

    public static void ConfigureRabbitMQ(this IServiceCollection services, IConfiguration configuration)
    {
        var messageBrokerSection = configuration.GetSection("MessageBroker");

        messageBrokerSection["HostName"] =
            Environment.GetEnvironmentVariable("RABBITMQ_HOSTNAME") ?? messageBrokerSection["HostName"];
        messageBrokerSection["Username"] =
            Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_USER") ?? messageBrokerSection["Username"];
        messageBrokerSection["Password"] =
            Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_PASS") ?? messageBrokerSection["Password"];
        messageBrokerSection["Port"] =
            Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? messageBrokerSection["Port"];

        services.AddOptions<RabbitMQOptions>()
            .Bind(messageBrokerSection)
            .ValidateDataAnnotations()
            .Validate(
                config =>
                    config.Queues != null &&
                    !string.IsNullOrEmpty(config.Queues.MeetingRequest),
                "Queue names must be configured in RabbitMQ options.")
            .ValidateOnStart();

        services.AddSingleton<IMessageService, RabbitMQService>();
    }

    public static void RunJobs(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

        recurringJobManager.AddOrUpdate<IDateNotificationJobService>(
            "check-important-dates",
            x => x.CheckImportantDates(),
            Cron.Daily);
        recurringJobManager.AddOrUpdate<IMeetingNotificationJobService>(
            "check-future-meetings",
            x => x.CheckFutureMeetings(),
            Cron.Daily);
        recurringJobManager.AddOrUpdate<IGoalNotificationJobService>(
            "check-future-goals",
            x => x.CheckFutureGoals(),
            Cron.Monthly);
        recurringJobManager.AddOrUpdate<IDeleteAchievedGoalJobService>(
            "delete-achieved-goals",
            s => s.DeleteAchievedGoalsAsync(),
            Cron.Daily);
    }
}