namespace AuthService.Infrastructure;

using AuthService.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class FriendsAppDbContext : IdentityDbContext<ApplicationUser>
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