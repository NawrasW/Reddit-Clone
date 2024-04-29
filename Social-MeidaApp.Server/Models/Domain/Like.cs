namespace Social_MeidaApp.Server.Models.Domain
{
    public class Like
    {
        public int LikeId { get; set; } // Single primary key
        public int UserId { get; set; }
        public int? PostId { get; set; } // Nullable, as like could be for a post or a comment
        public int? CommentId { get; set; } // Nullable
        public User User { get; set; }
        public Post Post { get; set; }
        public Comment Comment { get; set; }
    }

}
