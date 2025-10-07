using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Design;

namespace AuthService.Infrastructure;

using AuthService.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class FriendsAppDbContext : IdentityDbContext<AppUser, IdentityRole, string>
{
    public FriendsAppDbContext(DbContextOptions<FriendsAppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {

        base.OnModelCreating(builder);

        builder.HasDefaultSchema("Identity");
    }
    
}

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<FriendsAppDbContext>
{
    public FriendsAppDbContext CreateDbContext(string[] args)
    {
        var possiblePaths = new[]
        {
            Path.Combine(Directory.GetCurrentDirectory(), "../../../.env"),
        };

        foreach (var path in possiblePaths)
        {
            Console.WriteLine($"Checking: {path}");
            if (File.Exists(path))
            {
                Console.WriteLine($"Found .env at: {path}");
                Env.Load(path);
                break;
            }
        }
        var connectionString = Environment.GetEnvironmentVariable("POSTGRES_DB_CONNECTION_STRING");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("POSTGRES_");
        }

        var optionsBuilder = new DbContextOptionsBuilder<FriendsAppDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new FriendsAppDbContext(optionsBuilder.Options);
    }
}