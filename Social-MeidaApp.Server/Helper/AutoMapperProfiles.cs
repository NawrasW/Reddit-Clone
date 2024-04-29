using AutoMapper;
using Social_MeidaApp.Server.Models.Domain;
using Social_MeidaApp.Server.Models.Dto;

namespace Social_MeidaApp.Server.Helper
{
    public class AutoMapperProfiles : Profile
    {

        public AutoMapperProfiles()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap <User,UserDtoRes>().ReverseMap();
            CreateMap<Post, PostDto>().ReverseMap();
            CreateMap<Comment, CommentDto>().ReverseMap();
            CreateMap<Post, PostDtoRes>();
            //CreateMap<UserDto, User>();

        }

    }
}
