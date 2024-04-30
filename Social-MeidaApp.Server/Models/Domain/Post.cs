namespace Social_MeidaApp.Server.Models.Domain
{
    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public User Author { get; set; }
        public List<Comment> Comments { get; set; }

        public List<Like> Likes { get; set; } // Add this navigation property

        public int LikeCount { get; set; } // Number of likes for the post


    }
}
