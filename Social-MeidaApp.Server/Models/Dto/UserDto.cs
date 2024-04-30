namespace Social_MeidaApp.Server.Models.Dto
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }
    }


    public class UserDtoRes {

        public int UserId { get; set; }
        public string Name { get; set; }


    }


}
