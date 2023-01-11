using kiralama_sistemi.Entities.Enums;
using kiralamaSistemi.Entities.Enums;

namespace kiralamaSistemi.API.Model.Araba
{
    public class CreateUpdateArabaModel
    {
       // public int MusteriId { get; set; }
        public int Id { get; set; }
        public int MusteriId { get; set; }
        public string Model { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime SonTarihi { get; set; }
        public bool Durum { get; set; }
        public string Notlar { get; set; }
        public string ArabaNumrasi { get; set; }
        public EnumArabaRenk EnumArabaRenk { get; set; }
        public EnumArabaTur EnumArabaTur { get; set; }
        public List<TarifeModel> Tarifeler { get; set; }
        //public List<KiralamaModel> Kiralama { get; set; }
    }

    public class TarifeModel
    {
        public decimal TarifTutar { get; set; }
        public EnumKiralamaSuresi EnumKiralamaSuresi { get; set; }
        //public int ArabaId { get; set; }
    }

}
