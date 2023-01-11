
using kiralama_sistemi.Entities.Enums;

namespace kiralamaSistemi.API.Model.Tarife
{
    public class CreateUpdateTarifeModel
    {
        public int Id { get; set; }
        public EnumKiralamaSuresi EnumKiralamaSuresi { get; set; }
        public decimal TarifTutar { get; set; }
        public int ArabaId { get; set; }
        public string? Model { get; set; }
    }
}
