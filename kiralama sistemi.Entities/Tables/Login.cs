namespace kiralamaSistemi.Entities.Tables
{
    public class Login
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public AppUser? User { get; set; }
        public string UserName { get; set; }
        public string? Ip { get; set; }
        public string? Browser { get; set; }
        public string? OsType { get; set; }
        public DateTime Date { get; set; }
    }
}
