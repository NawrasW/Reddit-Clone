namespace Social_MeidaApp.Server.Models.Dto
{
    public class UserForLoginDto
    {

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }

    public class UserLoginDto
    {
     


        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;



        public string Name { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;


    }
   
}
