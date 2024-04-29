using Social_MeidaApp.Server.Models.Domain;
using Social_MeidaApp.Server.Models.Dto;

namespace Social_MeidaApp.Server.Repsitories.Abstrsct
{
    public interface IUserRepository
    {
        Task<bool> AddUpdateUser(UserDto user);

        Task<List<UserDto>> GetAllUser();

        Task<bool> DeleteUser(int id);

        Task<UserDto> GetUserById(int id);

        Task<UserLoginResponseDto> GetLoginUser(UserLoginReqDto user);
    }
}
