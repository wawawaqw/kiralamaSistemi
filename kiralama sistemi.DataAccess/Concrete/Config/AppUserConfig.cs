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
    internal class AppUserConfig : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            //builder.HasKey(i => i.Id);
            builder.ToTable("Users");



            builder.Property(i => i.Ad).HasColumnType("nvarchar(50)");

            builder.Property(i => i.CreatedById).HasColumnType("int").IsRequired(false);
            builder.Property(i => i.CreatedByName).HasColumnType("NVARCHAR(50)").IsRequired(false);
            builder.Property(i => i.CreatedDate).IsRequired(true).HasColumnType("datetime2(7)").HasDefaultValueSql("getdate()");

            builder.Property(i => i.ModifiedById).HasColumnType("int").IsRequired(false);
            builder.Property(i => i.ModifiedByName).HasColumnType("NVARCHAR(50)").IsRequired(false);
            builder.Property(i => i.ModifiedDate).HasColumnType("datetime2(7)").IsRequired(false);


        }
    }
}
