namespace DMnDBCS.Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime Time { get; set; }
        public int? ProjectId { get; set; }
        public int? UserId { get; set; }
    }
}
