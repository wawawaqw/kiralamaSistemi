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
    public class LogConfig : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.HasKey(i => i.Id);
            builder.ToTable("Logs");

            builder.Property(i => i.DataId).HasColumnType("int");
            builder.Property(i => i.Date).HasColumnType("datetime2(7)").HasDefaultValueSql("GETDATE()");
            builder.Property(i => i.UserId).HasColumnType("int");
            builder.Property(i => i.UserName).HasColumnType("NVARCHAR(MAX)");
            builder.Property(i => i.Title).HasColumnType("NVARCHAR(MAX)");
            builder.Property(i => i.NewValue).HasColumnType("NVARCHAR(MAX)");
            builder.Property(i => i.OldValue).HasColumnType("NVARCHAR(MAX)");
            builder.Property(i => i.Module).HasColumnType("int");
            builder.Property(i => i.Event).HasColumnType("int");
            builder.Property(i => i.LoginId).HasColumnType("int");

            builder.HasOne(i => i.Login)
                    .WithMany()
                    .HasForeignKey(i => i.LoginId);
        }
    }
}
