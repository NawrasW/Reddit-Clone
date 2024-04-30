
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Social_MeidaApp.Server.Models.Domain;
using Social_MeidaApp.Server.Models.Dto;
using Social_MeidaApp.Server.Repsitories.Abstrsct;
using System.Security.Cryptography;
using System.Text;

namespace Social_MediaApp.Server.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _db;
        private readonly IMapper _mapper;
        public UserRepository(DatabaseContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }


        public async Task<bool> AddUpdateUser(UserDto userDto)
        {
            try
            {
                Console.WriteLine("Entering AddUpdateUser method...");

                User user;

                if (userDto.UserId == 0)
                {
                    Console.WriteLine("Adding a new user...");

                    // Create a new user entity and manually set properties
                    user = new User
                    {
                        Name = userDto.Name,
                        Email = userDto.Email
                    };

                    // Hash the password before saving it
                    if (!string.IsNullOrEmpty(userDto.Password))
                    {
                        HashPassword(userDto.Password, out byte[] passwordKey, out byte[] passwordHash);
                        user.PasswordKey = passwordKey;
                        user.Password = passwordHash;
                    }

                    await _db.Users.AddAsync(user);
                }
                else
                {
                    Console.WriteLine($"Updating user with ID: {userDto.UserId}...");

                    // Retrieve the existing user
                    user = await _db.Users.FindAsync(userDto.UserId);

                    if (user != null)
                    {
                        // Manually update properties
                        user.Name = userDto.Name;
                        user.Email = userDto.Email;

                        // Check if a new password is provided
                        if (!string.IsNullOrEmpty(userDto.Password))
                        {
                            Console.WriteLine($"Updating user password for user with ID: {user.UserId}...");

                            // Hash the new password before updating it
                            HashPassword(userDto.Password, out byte[] newPasswordKey, out byte[] newPasswordHash);
                            user.PasswordKey = newPasswordKey;
                            user.Password = newPasswordHash;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"User with ID {userDto.UserId} not found.");
                        return false;
                    }
                }

                Console.WriteLine("Saving changes to the database...");
                await _db.SaveChangesAsync();

                Console.WriteLine("Changes saved successfully.");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddUpdateUser: {ex.Message}");
                return false;
            }
        }




        private void HashPassword(string password, out byte[] passwordKey, out byte[] passwordHash)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordKey = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> DeleteUser(int userId)
        {
            try
            {
                var user = await _db.Users.FindAsync(userId);

                if (user == null)
                {
                    return false;
                }

                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteUser: {ex.Message}");
                return false;
            }
        }

        public async Task<List<UserDto>> GetAllUser()
        {
            var result = await _db.Users.ToListAsync();
            var lstUserDto = _mapper.Map<List<UserDto>>(result);
            return lstUserDto;
        }

        public async Task<UserDto> GetUserById(int Id)
        {
            var result = await _db.Users.FirstOrDefaultAsync(user => user.UserId == Id);
            //return _db.Users.Find(Id);
            UserDto user = _mapper.Map<UserDto>(result);
            return user;
        }

        public async Task<UserLoginResponseDto> GetLoginUser(UserLoginReqDto user)
        {
            Console.WriteLine($"Attempting to log in user with email: {user.Email}");

            var result = await _db.Users.FirstOrDefaultAsync(us => us.Email.ToLower() == user.Email);

            if (result != null && MatchPasswordHash(user.Password, result.Password, result.PasswordKey))
            {
                Console.WriteLine($"User {user.Email} found, password matches.");

                var userResult = new UserLoginResponseDto
                {
                    Id = result.UserId,
                    Name = result.Name
                };

                Console.WriteLine($"Login successful for user {user.Email}");
                return userResult;
            }

            //Handle the case where the user is not found or the password doesn't match
            Console.WriteLine($"Login failed for user {user.Email}");
                return null;
            }



            private bool MatchPasswordHash(string passwordText, byte[] storedPasswordHash, byte[] passwordkey)
            {
                using (var hmac = new HMACSHA512(passwordkey))
                {
                    var computedPasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passwordText));

                    //Compare each byte of the computed hash with the stored hash
                        return computedPasswordHash.SequenceEqual(storedPasswordHash);
                }
            }
        }
}
