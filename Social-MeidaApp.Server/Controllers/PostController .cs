using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Social_MeidaApp.Server.Repsitories.Abstrsct;
using Social_MeidaApp.Server.Models.Dto;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPostRepository _repository;
        private readonly IConfiguration _configuration;
        public PostController(IPostRepository repository, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _configuration = configuration;
        }

        [HttpGet("getPostById/{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            var post = await _repository.GetById(id);
            if (post == null)
            {
                var status = new { StatusCode = 0, StatusMessage = "Post Not Found" };
                return Ok(status);
            }
            return Ok(post);
        }

        [HttpGet("getAllPosts")]
        public async Task<IActionResult> GetAllPosts(int currentUserId)
        {
            var posts = await _repository.GetAll( currentUserId);
            return Ok(posts);
        }

        [HttpPost("addPost")]
        public async Task<IActionResult> AddPost(PostDto postDto)
        {
            var result = await _repository.AddUpdate(postDto);
            var status = new { StatusCode = result ? 1 : 0, StatusMessage = result ? "Post Added Successfully" : "Error Adding Post" };
            return Ok(status);
        }

        [HttpPut("updatePost")]
        public async Task<IActionResult> UpdatePost(PostDto postDto)
        {
            var result = await _repository.AddUpdate(postDto);
            var status = new { StatusCode = result ? 1 : 0, StatusMessage = result ? "Post Updated Successfully" : "Error Updating Post" };
            return Ok(status);
        }

        [HttpDelete("deletePost/{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var result = await _repository.Delete(id);
            var status = new { StatusCode = result ? 1 : 0, StatusMessage = result ? "Post Deleted Successfully" : "Error Deleting Post" };
            return Ok(status);
        }


        [HttpGet("post/likeCount/{postId}")]
        public async Task<IActionResult> GetLikeCountForPost(int postId)
        {
            try
            {
                var likeCount = await _repository.GetLikeCountForPost(postId);
                return Ok(likeCount);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error fetching like count");
            }
        }

        [HttpPost("like/{postId}")]
        public async Task<IActionResult> LikePost(int postId, [FromBody] UserLoginDto user)
        {
            try
            {
                var likePostDto = new LikePostDTO
                {
                    UserId = GetUserIdFromToken(user),
                    PostId = postId
                };

                var success = await _repository.LikePost(likePostDto);
                if (success)
                {
                    return Ok(likePostDto);
                }
                else
                {
                    return BadRequest("User has already liked this post.");
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(401, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("unlike/{postId}")]
        public async Task<IActionResult> UnlikePost(int postId, [FromBody] UserLoginDto user)
        {
            try
            {
                var unlikePostDto = new LikePostDTO
                {
                    UserId = GetUserIdFromToken( user),
                    PostId = postId
                };

                var success = await _repository.UnlikePost(unlikePostDto);
                if (success)
                {
                    return Ok(unlikePostDto);
                }
                else
                {
                    return BadRequest("User hasn't liked this post.");
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Error unliking post.");
            }
        }


        private int GetUserIdFromToken(UserLoginDto user)
        {
            var jwtToken = user.Token; // Extract token from UserLoginDto
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:key").Value)),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            try
            {
                ClaimsPrincipal principal = tokenHandler.ValidateToken(jwtToken, validationParameters, out SecurityToken validatedToken);
                var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    return userId;
                }
                else
                {
                    // Handle the case where user ID cannot be retrieved
                    throw new UnauthorizedAccessException("User ID not found in token.");
                }
            }
            catch (Exception ex)
            {
                // Handle token validation errors
                throw new UnauthorizedAccessException("Invalid token.", ex);
            }
        }







    }
}

