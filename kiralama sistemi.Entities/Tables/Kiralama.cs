using kiralama_sistemi.Entities.Enums;

namespace kiralamaSistemi.Entities.Tables
{
    public class Kiralama
    {
        public int Id { get; set; }
        public int ArabaId { get; set; }
        public Araba Araba { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime SonTarihi { get; set; }
        public EnumKiralamaSuresi EnumKiralamaSuresi { get; set; }
        public int MusteriId { get; set; }
        public Musteri Musteri { get; set; }


    }
}
