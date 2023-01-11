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
    public class LoginConfig : IEntityTypeConfiguration<Login>
    {
        public void Configure(EntityTypeBuilder<Login> builder)
        {
            builder.ToTable("Logins");
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Browser).HasColumnType("NVARCHAR(50)");
            builder.Property(i => i.UserName).HasColumnType("NVARCHAR(MAX)");
            builder.Property(i => i.Date).HasColumnType("datetime2(7)").HasDefaultValueSql("getdate()");
            builder.Property(i => i.OsType).HasColumnType("NVARCHAR(25)");
            builder.Property(i => i.Ip).HasColumnType("NVARCHAR(32)");

            builder
               .HasOne(i => i.User)
               .WithMany(i => i.Logins)
               .HasForeignKey(i => i.UserId)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
