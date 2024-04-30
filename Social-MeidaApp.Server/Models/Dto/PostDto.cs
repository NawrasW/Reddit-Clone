using Social_MeidaApp.Server.Models.Domain;

namespace Social_MeidaApp.Server.Models.Dto
{
    public class PostDto
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserDtoRes Author { get; set; }
        public int LikeCount { get; set; } // Number of likes for the post
        public bool IsLikedByCurrentUser { get; set; } // Indicates whether the current user has liked the post




    }

    public class PostDtoRes
    {
        public int PostId { get; set; }
        public string Title { get; set; }

    }

    public class LikeDto
    {
        public int UserId { get; set; }
        public int PostId { get; set; }
    }
}
