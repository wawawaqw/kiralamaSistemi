namespace kiralamaSistemi.DataAccess.Sevices
{
    public class JwtSettings
    {

        public string SecretTime { get; set; }
        public string Secret { get; set; }
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
    }
}
