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
    internal class KiralamaConfig : IEntityTypeConfiguration<Kiralama>
    {
        public void Configure(EntityTypeBuilder<Kiralama> builder)
        {
            builder.HasKey(i => i.Id);
            //builder.HasKey(i => new {i.ArabaId,i.MusteriId});
            builder.ToTable("Kiralamalar");

           // builder.Property(i => i.Id).HasColumnType("int");
            builder.Property(i => i.BaslangicTarihi).HasColumnType("datetime2(7)").HasDefaultValueSql("getdate()");
            builder.Property(i => i.SonTarihi).HasColumnType("datetime2(7)").HasDefaultValueSql("getdate()");
            builder.Property(i => i.EnumKiralamaSuresi).HasColumnType("smallint");

            builder.HasOne(i => i.Araba)
               .WithMany(i => i.Kiralamalar)
               .HasForeignKey(i => i.ArabaId)
               .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(i => i.Musteri)
               .WithMany(i => i.Kiralamalar)
               .HasForeignKey(i => i.MusteriId)
               .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
