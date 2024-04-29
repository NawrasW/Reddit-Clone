namespace Social_MeidaApp.Server.Models.Domain
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public byte[] Password { get; set; }
        public byte[] PasswordKey { get; set; }

        public List<Post> Posts { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Like> Likes { get; set; } // Add this navigation property


        public string? Role { get; set; }


        public string? Token { get; set; }
    }
}
