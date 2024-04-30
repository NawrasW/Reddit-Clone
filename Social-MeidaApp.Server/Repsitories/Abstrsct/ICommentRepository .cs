using Social_MeidaApp.Server.Models.Domain;
using Social_MeidaApp.Server.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Social_MeidaApp.Server.Repsitories.Abstrsct
{
    public interface ICommentRepository
    {
        Task<bool> AddUpdate(CommentDtoAddUpdate commentDto);
        Task<bool> Delete(int commentId);
        Task<List<CommentDto>> GetAllByPostId(int postId);

        Task<bool> LikeComment(LikeCommentDTO likeCommentDto);
        Task<bool> UnlikeComment(LikeCommentDTO unlikeCommentDto);
    }
}
