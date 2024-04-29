using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Social_MeidaApp.Server.Models.Domain;
using Social_MeidaApp.Server.Models.Dto;
using Social_MeidaApp.Server.Repsitories.Abstrsct;
using System.Security.Cryptography;

namespace Social_MediaApp.Server.Repositories.Implementation
{
    public class AuthRepsitory: IAuthRepsitory
    {

        private readonly DatabaseContext _db;
        private readonly IMapper _mapper;


        public AuthRepsitory(DatabaseContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<UserLoginDto> Login(UserForLoginDto userDto)
        {
            // var record = await _db.Users.FirstOrDefaultAsync(us => us.Username.ToLower() == userDto.UserName && us.Password == userDto.Password);

            UserLoginDto record;
            try
            {

                var user = await _db.Users.Where(user => user.Email.ToLower() == userDto.Email.ToLower()).FirstOrDefaultAsync();

                /*&& user.Password == userDto.Password)*/
                //.Select(user => new UserLoginDto
                //{
                //    Id = user.Id,
                //    UserName = user.Username,
                //    FirstName = user.FirstName,
                //    LastName = user.LastName,
                //    Role = user.Role

                //}).FirstOrDefaultAsync()?? new UserLoginDto();

                if (user == null || user.PasswordKey == null)
                    return null;
                if (!MatchPasswordHash(userDto.Password, user.Password, user.PasswordKey))
                    return null;
                UserLoginDto result = new UserLoginDto
                {
                    Id = user.UserId,
                    Email = user.Email,

                    Name = user.Name,
                   

                };
                return result;
            }

            catch (Exception ex)
            {

                throw null;
            }

            //if (record == null)
            //    return null;
            //{ UserLoginDto result = new UserLoginDto(record.Username, record.FirstName, record.LastName); }

            //UserLoginDto result = new UserLoginDto(record.Id, record.UserName, record.FirstName, record.LastName);
            //if (record.Id == 0)

            //    return null;

        }

        public async Task<bool> Register(UserDto userDto)
        {
            byte[] passwordkey, passwordHash;
            using (var hmac = new HMACSHA512())
            {

                passwordkey = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(userDto.Password));


            }
            User user = new User
            {

                UserId = userDto.UserId,
                Name = userDto.Name,
                
                Email = userDto.Email,
             

                //Password = userDto.Password,
            };

            user.Password = passwordHash;
            user.PasswordKey = passwordkey;
            await _db.Users.AddAsync(user);

            _db.SaveChanges();
            return true;
        }

        public async Task<bool> UserAlreadyExists(string userName)
        {
            return await _db.Users.AnyAsync(user => user.Email == userName);
        }


        private bool MatchPasswordHash(string passwordText, byte[] storedPasswordHash, byte[] passwordkey)
        {
            using (var hmac = new HMACSHA512(passwordkey))
            {
                var computedPasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passwordText));

                // Compare each byte of the computed hash with the stored hash
                for (int i = 0; i < storedPasswordHash.Length; i++)
                {
                    if (computedPasswordHash[i] != storedPasswordHash[i])
                        return false;
                }

                return true;
            }
        }

    }


}

