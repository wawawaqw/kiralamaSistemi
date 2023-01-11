namespace kiralama_sistemi.Entities.Abstract
{
    public class InvalidLogin
    {
        public string Ip { get; set; }
        public DateTime Date { get; set; }
        public int AttemptCount { get; set; }
        public bool IsBanned { get; set; }
    }
}
