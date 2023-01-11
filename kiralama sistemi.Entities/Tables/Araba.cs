using kiralamaSistemi.Entities.Enums;

namespace kiralamaSistemi.Entities.Tables
{
    public class Araba
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Notlar { get; set; }
        public bool Durum { get; set; }
        public string ArabaNumrasi { get; set; }
        public EnumArabaRenk EnumArabaRenk { get; set; }
        public EnumArabaTur EnumArabaTur { get; set; }
        public int MusteriId { get; set; }
        public Musteri Musteri { get; set; }
        public List<Tarife> Tarifalar { get; set; }
        public List<Kiralama> Kiralamalar { get; set; }

    }
}
