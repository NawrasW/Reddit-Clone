

using Social_MeidaApp.Server.Models.Domain;

namespace Social_MeidaApp.Server.Models.Dto
{
    public class CommentDto
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedDate { get; set; }
        //public int AuthorUserId { get; set; }
        public int PostId { get; set; }

        //public PostDtoRes Post { get; set; }
        public UserDto Author { get; set; }

        public int? ParentCommentId { get; set; }
        // Optional: If you want to include author information in the DTO

        // List of replies (child comments)
        // List of replies (child comments)
        public List<CommentDto>? Replies { get; set; }
   public int LikeCount { get; set; } // Number of likes for the comment
        public bool IsLikedByCurrentUser { get; set; } // Indicates whether the current user has liked the comment
     


    }



    public class CommentDtoAddUpdate
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public int AuthorUserId { get; set; }

        public int? ParentCommentId { get; set; }

        public int PostId { get; set; }

        public int LikeCount { get; set; }
        public bool IsLikedByCurrentUser { get; set; }

    }
}