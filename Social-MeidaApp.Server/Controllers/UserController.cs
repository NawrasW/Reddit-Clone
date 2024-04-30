using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Social_MediaApp.Server.Repositories.Implementation;
using Social_MeidaApp.Server.Models.Domain;
using Social_MeidaApp.Server.Models.Dto;
using Social_MeidaApp.Server.Repsitories.Abstrsct;

namespace Social_MeidaApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserRepository _userRepsitory;

        public UserController(IUserRepository userRepsitory)
        {
            _userRepsitory = userRepsitory;


        }

        [HttpPost("AddUpdateUser")]
        public async Task<IActionResult> AddUpdateUser(UserDto userDto)
        {
            var result = await _userRepsitory.AddUpdateUser(userDto);
            var status = new Status
            {
                StatusCode = result ? 1 : 0,
                StatusMessage = result ? "Saved Successfully" : "Error Adding......."
            };
            return Ok(status);
        }

        [HttpGet("getAllUser")]
        public async Task<IActionResult> GetAllUser()
        {
            try
            {
                var result = await _userRepsitory.GetAllUser();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 0, StatusMessage = ex.Message });
            }
        }

        [HttpDelete("deleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await _userRepsitory.DeleteUser(id);
                return Ok(new { StatusCode = result ? 1 : 0, StatusMessage = result ? "Deleted Successfully" : "Error Deleting..." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 0, StatusMessage = ex.Message });
            }
        }

        [HttpGet("getUserById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userRepsitory.GetUserById(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 0, StatusMessage = ex.Message });
            }
        }

    }
}
