using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Social_MeidaApp.Server.Models.Dto;
using Social_MeidaApp.Server.Repsitories.Abstrsct;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _repository;
        private readonly IConfiguration _configuration;

        public CommentController(ICommentRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        [HttpPost("addComment")]
        public async Task<IActionResult> AddComment(CommentDtoAddUpdate commentDto)
        {
            var result = await _repository.AddUpdate(commentDto);
            var status = new { StatusCode = result ? 1 : 0, StatusMessage = result ? "Comment Added Successfully" : "Error Adding Comment" };
            return Ok(status);
        }



        [HttpPut("UpdateComment")]
        public async Task<IActionResult> UpdateComment(CommentDtoAddUpdate commentDto)
        {
            var result = await _repository.AddUpdate(commentDto);
            var status = new { StatusCode = result ? 1 : 0, StatusMessage = result ? "Comment Updated Successfully" : "Error Updating Comment" };
            return Ok(status);
        }

        [HttpDelete("deleteComment/{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var result = await _repository.Delete(id);
            var status = new { StatusCode = result ? 1 : 0, StatusMessage = result ? "Comment Deleted Successfully" : "Error Deleting Comment" };
            return Ok(status);
        }

        [HttpGet("getAllCommentsByPostId/{postId}")]
        public async Task<IActionResult> GetAllCommentsByPostId(int postId)
        {
            var comments = await _repository.GetAllByPostId(postId);
            return Ok(comments);
        }










        [HttpPost("like/{commentId}")]
        public async Task<IActionResult> LikeComment(int commentId, [FromBody] UserLoginDto user)
        {
            try
            {
                var likeCommentDto = new LikeCommentDTO
                {
                    UserId = GetUserIdFromToken(user),
                    CommentId = commentId
                };

                var success = await _repository.LikeComment(likeCommentDto);
                if (success)
                {
                    return Ok(likeCommentDto);
                  
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

        [HttpPost("unlike/{commentId}")]
        public async Task<IActionResult> UnlikeComment(int commentId, [FromBody] UserLoginDto user)
        {
            try
            {
                var unlikeCommentDto = new LikeCommentDTO
                {
                    UserId = GetUserIdFromToken(user),
                    CommentId = commentId
                };

                var success = await _repository.UnlikeComment(unlikeCommentDto);
                if (success)
                {
                    return Ok(unlikeCommentDto);
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
