using kiralama_sistemi.Entities.Enums;
using kiralamaSistemi.Entities.Tables;

namespace kiralamaSistemi.API.Model.Kiralama
{
    public class CreateUpdateKiralamaModel
    {
        public int Id { get; set; }
        public DateTime KiralamaTarihi { get; set; }
        public int ArabaId { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime SonTarihi { get; set; }
        public EnumKiralamaSuresi EnumKiralamaSuresi { get; set; }
        public int MusteriId { get; set; }
    }
}
