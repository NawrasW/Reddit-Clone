using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Social_MeidaApp.Server.Models.Domain;
using Social_MeidaApp.Server.Models.Dto;
using Social_MeidaApp.Server.Repsitories.Abstrsct;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Social_MediaApp.Server.Repositories.Implementation
{
    public class PostRepository : IPostRepository
    {
        private readonly DatabaseContext _db;
        private readonly IMapper _mapper;

        public PostRepository(DatabaseContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<bool> AddUpdate(PostDto postDto)
        {
            try
            {
                var post = _mapper.Map<Post>(postDto);

                // Check if the provided Author exists in the database
                var existingUser = await _db.Users.FindAsync(postDto.Author.UserId);
                if (existingUser == null)
                {
                    // If the Author doesn't exist, return false or handle the error as needed
                    return false;
                }

                // Set the Author property of the post to the existing user
                post.Author = existingUser;

                // If the post doesn't have a PostId, it's a new post, so set the CreatedAt date
                if (post.PostId == 0)
                {
                    post.CreatedAt = DateTime.Now;
                   // Initialize Likes list

                    // Add the post to the database
                    await _db.Posts.AddAsync(post);
                }
                else
                {
                    // If post already exists, update it
                    _db.Posts.Update(post);
                }

                // Update the like count for the post
                postDto.LikeCount = await _db.Likes.CountAsync(like => like.PostId == post.PostId);

                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }



        public async Task<int> GetLikeCountForPost(int postId)
        {
            return await _db.Likes.CountAsync(like => like.PostId == postId);
        }



        public async Task<bool> LikePost(LikePostDTO likePostDto)
        {
            var post = await _db.Posts.FindAsync(likePostDto.PostId);
            if (post == null)
            {
                throw new ArgumentException("Post not found.");
            }

            var like = await _db.Likes.FirstOrDefaultAsync(l => l.UserId == likePostDto.UserId && l.PostId == likePostDto.PostId);
            if (like == null)
            {
                like = new Like
                {
                    UserId = likePostDto.UserId,
                    PostId = likePostDto.PostId
                };
                _db.Likes.Add(like);
                await _db.SaveChangesAsync();

                // Increment like count for the post
                post.LikeCount++;
                await _db.SaveChangesAsync();

                return true;
            }

            return false; // User has already liked the post
        }


        public async Task<bool> UnlikePost(LikePostDTO unlikePostDto)
        {
            var post = await _db.Posts.FindAsync(unlikePostDto.PostId);
            if (post == null)
            {
                throw new ArgumentException("Post not found.");
            }

            var like = await _db.Likes.FirstOrDefaultAsync(l => l.UserId == unlikePostDto.UserId && l.PostId == unlikePostDto.PostId);
            if (like != null)
            {
                _db.Likes.Remove(like);
                await _db.SaveChangesAsync();

                // Decrement like count for the post
                post.LikeCount--;
                await _db.SaveChangesAsync();

                return true;
            }

            return false; // User hasn't liked the post
        }








        public async Task<bool> Delete(int id)
        {
            try
            {
                var post = await _db.Posts.FindAsync(id);

                if (post == null)
                {
                    return false;
                }

                _db.Posts.Remove(post);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Delete: {ex.Message}");
                return false;
            }
        }

        public async Task<List<PostDto>> GetAll(int currentUserId)
        {
            try
            {
                var result = await _db.Posts
                    .Include(post => post.Likes) // Include likes associated with each post
                    .Select(post => new PostDto
                    {
                        PostId = post.PostId,
                        Title = post.Title,
                        Content = post.Content,
                        CreatedAt = post.CreatedAt,
                        Author = new UserDtoRes
                        {
                            UserId = post.Author.UserId,
                            Name = post.Author.Name
                        },
                        LikeCount = post.Likes.Count, // Calculate like count
                        IsLikedByCurrentUser = post.Likes.Any(like => like.UserId == currentUserId) // Check if the current user has liked the post
                    })
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }





        public async Task<PostDto> GetById(int id)
        {
            var result = await _db.Posts.FindAsync(id);
            return _mapper.Map<PostDto>(result);
        }

     
    }
}
