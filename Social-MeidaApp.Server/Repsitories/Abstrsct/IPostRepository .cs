using Social_MeidaApp.Server.Models.Domain;
using Social_MeidaApp.Server.Models.Dto;

namespace Social_MeidaApp.Server.Repsitories.Abstrsct
{
    public interface IPostRepository
    {
        Task<PostDto> GetById(int id);

        Task<List<PostDto>> GetAll(int currentUserId);

        Task<bool> AddUpdate(PostDto post);

        Task<bool> Delete(int Id);

        Task<int> GetLikeCountForPost( int postId);

        Task<bool> LikePost(LikePostDTO likePostDto);
        Task<bool> UnlikePost(LikePostDTO unlikePostDto);

    }
}
