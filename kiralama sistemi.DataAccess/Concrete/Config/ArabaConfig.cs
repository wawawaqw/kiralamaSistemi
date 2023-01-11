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
    internal class ArabaConfig : IEntityTypeConfiguration<Araba>
    {
        public void Configure(EntityTypeBuilder<Araba> builder)
        {
            builder.HasKey(i => i.Id);
            builder.ToTable("Arabalar");


            builder.Property(i => i.Model).HasColumnType("nvarchar(50)");
            builder.Property(i => i.ArabaNumrasi).HasColumnType("nvarchar(50)");
            builder.Property(i => i.Notlar).HasColumnType("nvarchar(250)"); 
            builder.Property(i => i.Durum).HasColumnType("tinyint").IsRequired(true);
            builder.Property(i => i.EnumArabaRenk).HasColumnType("smallint");
            builder.Property(i => i.EnumArabaTur).HasColumnType("smallint");

        }
    }
}
