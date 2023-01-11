using kiralama_sistemi.Entities.Enums;
using kiralamaSistemi.Entities.Enums;
using kiralamaSistemi.Entities.Tables;

namespace kiralamaSistemi.API.Model.Araba
{
    public class ResAraba
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Notlar { get; set; }
        public bool Durum { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime SonTarihi { get; set; }
        public string ArabaNumrasi { get; set; }
        public EnumArabaRenk EnumArabaRenk { get; set; }
        public EnumArabaTur EnumArabaTur { get; set; }
    }

   

}
