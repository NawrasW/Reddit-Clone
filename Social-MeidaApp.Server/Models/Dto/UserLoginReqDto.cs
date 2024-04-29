namespace Social_MeidaApp.Server.Models.Dto
{
    public class UserLoginReqDto
    {
        public string? Email { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
    }


    public class UserLoginResponseDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }


    }
}
