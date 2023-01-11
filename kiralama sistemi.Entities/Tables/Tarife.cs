using kiralama_sistemi.Entities.Enums;

namespace kiralamaSistemi.Entities.Tables
{
    public class Tarife
    {

        public int Id { get; set; }
        public EnumKiralamaSuresi EnumKiralamaSuresi { get; set; }
        public decimal TarifTutar { get; set; }
        public int ArabaId { get; set; }
        public Araba Araba { get; set; }
    }
}
