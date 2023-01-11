using kiralamaSistemi.Entities.Tables;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace kiralamaSistemi.DataAccess.Concrete.Config
{
    internal class MusteriConfig : IEntityTypeConfiguration<Musteri>
    {
        public void Configure(EntityTypeBuilder<Musteri> builder)
        {
            builder.HasKey(i => i.Id);
            builder.ToTable("Musteriler");


            builder.Property(i => i.Adres).HasColumnType("nvarchar(50)");
            builder.Property(i => i.Telefon).HasColumnType("nvarchar(50)");
            builder.Property(i => i.Tc).HasColumnType("nvarchar(50)");
            builder.Property(i => i.Cinsiyet).HasColumnType("smallint");


            builder.HasOne(i => i.User)
                .WithOne(i => i.Musteri)
                .HasForeignKey<Musteri>(i => i.Id);

        }
    }
}
