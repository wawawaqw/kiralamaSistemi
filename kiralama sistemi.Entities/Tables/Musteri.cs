using kiralamaSistemi.Entities.Enums;

namespace kiralamaSistemi.Entities.Tables
{
    public  class Musteri
    {
        public int Id { get; set; }
        public AppUser User { get; set; }
        public EnumCinsiyet Cinsiyet { get; set; }
        public string Tc { get; set; }
        public string Telefon { get; set; }
        public string? Adres { get; set; }
        public List<Araba> Arabalar { get; set; }
        public List<Kiralama> Kiralamalar { get; set; }
    }
}
