namespace kiralamaSistemi.DataAccess.Sevices
{
    public class Result
    {
        public bool Status { get; set; }
        public List<Error>? Errors { get; set; }
        public Result()
        {
            Status = false;
        }
        public Result(params Error[] errors)
        {
            Status = false;
            Errors = errors.ToList();
        }
        public Result(bool status)
        {
            Status = status;
        }
    }
}
