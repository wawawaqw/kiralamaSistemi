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
    public class AppRoleConfig : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {


            //builder.ToTable("AppRoles");
            builder.HasKey(x => x.Id);

            builder.Property(i => i.CreatedById).HasColumnType("int").IsRequired(false);
            builder.Property(i => i.CreatedByName).HasColumnType("NVARCHAR(50)").IsRequired(false);
            builder.Property(i => i.CreatedDate).IsRequired(true).HasColumnType("datetime2(7)").HasDefaultValueSql("getdate()");

            builder.Property(i => i.ModifiedById).HasColumnType("int").IsRequired(false);
            builder.Property(i => i.ModifiedByName).HasColumnType("NVARCHAR(50)").IsRequired(false);
            builder.Property(i => i.ModifiedDate).HasColumnType("datetime2(7)").IsRequired(false);


        }
    }
}
