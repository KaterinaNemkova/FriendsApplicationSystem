using Microsoft.EntityFrameworkCore;
using AuthService.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AuthService.Infrastructure;

public class FriendsAppDbContext : IdentityDbContext<ApplicationUser>
{
    public FriendsAppDbContext(DbContextOptions<FriendsAppDbContext> options):base(options)
    {
        
    }
    
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
}