namespace DMnDBCS.Domain.Entities
{
    public class Log
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public DateTime Date { get; set; }
        public string UserName { get; set; }
    }
}
