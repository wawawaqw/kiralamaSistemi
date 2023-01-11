using kiralamaSistemi.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace LokantaSisteme_API.Models.Account
{
    public class SignUpModel
    {
        [Required]
        public string Ad{ get; set; }
        [Required]
        public string Soyad { get; set; }
        [Required]
        [Compare("RePassword")]
        public string Password { get; set; }
        [Required]
        public string RePassword { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string Tc { get; set; }
        public EnumCinsiyet Cinsiyet { get; set; }
        public bool  State { get; set; }
    }
}
