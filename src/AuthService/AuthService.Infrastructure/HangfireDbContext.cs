using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure;

public class HangfireDbContext : DbContext
{
    public HangfireDbContext(DbContextOptions<HangfireDbContext> options) : base(options) { }
}