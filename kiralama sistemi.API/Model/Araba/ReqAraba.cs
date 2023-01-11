using kiralamaSistemi.Entities.Enums;

namespace kiralamaSistemi.API.Model.Araba
{
    public class ReqAraba: ReqDataTable
    {
        public string? Model { get; set; }
        public string? ArabaNumrasi { get; set; }
    }
}
