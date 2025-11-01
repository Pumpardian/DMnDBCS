namespace DMnDBCS.Domain.Entities
{
    public class UserProfile
    {
        public int UserId { get; set; }
        public string Phone {  get; set; }
        public string Address { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? ProfilePicture { get; set; }
    }
}
