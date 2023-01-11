using kiralama_sistemi.Entities.Enums;

namespace kiralamaSistemi.API.Model.Tarife
{
    public class ResTarife
    {

        public int Id { get; set; }
        public EnumKiralamaSuresi EnumKiralamaSuresi { get; set; }
        public decimal TarifTutar { get; set; }
        
    }
}
