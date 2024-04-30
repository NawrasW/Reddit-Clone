using Social_MeidaApp.Server.Models.Dto;

namespace Social_MeidaApp.Server.Repsitories.Abstrsct
{
    public interface IAuthRepsitory
    {

        Task<UserLoginDto> Login(UserForLoginDto userDto);

        Task<bool> Register(UserDto userDto);


        Task<bool> UserAlreadyExists(string userName);
    }
}
