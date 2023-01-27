namespace AWebAPI.Models
{
    public class AppUser
    {
        public long ID { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
    }
}
