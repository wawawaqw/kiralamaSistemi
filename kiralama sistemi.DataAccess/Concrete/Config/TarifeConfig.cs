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
    internal class TarifeConfig : IEntityTypeConfiguration<Tarife>
    {
        public void Configure(EntityTypeBuilder<Tarife> builder)
        {
            builder.HasKey(i => i.Id);
            builder.ToTable("Tarifeler");


            builder.Property(i => i.EnumKiralamaSuresi).HasColumnType("smallint");
            builder.Property(i => i.TarifTutar).HasColumnType("decimal(6,2)");


            builder.HasOne(i => i.Araba)
               .WithMany(i => i.Tarifalar)
               .HasForeignKey(i => i.ArabaId)
               .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
