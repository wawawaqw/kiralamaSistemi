using kiralamaSistemi.Entities.Tables;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiralamaSistemi.DataAccess.Concrete.Config
{
    class AppRoleUserConfig : IEntityTypeConfiguration<AppUserRole>
    {
        public void Configure(EntityTypeBuilder<AppUserRole> builder)
        {
            //builder.ToTable("AppUserRoles");
            //builder.HasKey(i => new { i.UserId,i.RoleId });

            builder.HasOne(i => i.Role)
                .WithMany(i => i.AppRoleUsers)
                .HasForeignKey(i => i.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(i => i.User)
                .WithMany(i => i.AppUserRoles)
                .HasForeignKey(i => i.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
