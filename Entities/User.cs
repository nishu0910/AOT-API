namespace WebApi.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string emailId { get; set; }
        public string Token { get; set; }
    }
}