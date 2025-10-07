using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Infrastructure;

public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
      public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.Property(e => e.Id).HasDefaultValueSql("NEWID()");
        }
}