namespace Social_MeidaApp.Server.Models.Domain
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // Foreign key properties
        public int AuthorUserId { get; set; }
        public int PostId { get; set; }

        // Navigation properties
        public User Author { get; set; }
        public Post Post { get; set; }
        public int? ParentCommentId { get; set; }
        public Comment ParentComment { get; set; }
        public List<Comment> Replies { get; set; }
        public List<Like> Likes { get; set; } // Add this navigation property

        public int LikeCount { get; set; } // Number of likes for the post
    }


}