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
    public class LogModelMapConfig : IEntityTypeConfiguration<LogModuleMap>
    {
        public void Configure(EntityTypeBuilder<LogModuleMap> builder)
        {
            builder.ToTable("LogModuleSemalar");
            builder.HasKey(t => new { t.LogId, t.Module, t.DataId });

            builder.Property(i => i.Title).HasColumnType("NVARCHAR(max)");

            builder
                 .HasOne(i => i.Log)
                 .WithMany(i => i.LogModuleMaps)
                 .HasForeignKey(i => i.LogId);
        }
    }
}
