using kiralamaSistemi.Entities.Tables;

namespace kiralama_sistemi.DataAccess.Sevices
{
    public class AuthenticateResponse
    {
        public string Token { get; set; }
        public int Id { get; set; }
        public string AdSoyad { get; set; }
        public string Username { get; set; }


        public AuthenticateResponse(AppUser user, string token)
        {
            Token = token;
            Id = user.Id;
            AdSoyad = user.Ad;
            Username = user.UserName;
        }
    }
}
