using AgencyMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgencyMVC.Configurations
{
    public class AppUseConfigurationr : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasColumnType("varchar(30)");
            builder.Property(x => x.Surname).IsRequired().HasColumnType("varchar(50)");
        }
    }
}
