using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Social_MeidaApp.Server.Models.Domain;
using Social_MeidaApp.Server.Models.Dto;
using Social_MeidaApp.Server.Repsitories.Abstrsct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Social_MediaApp.Server.Repositories.Implementation
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DatabaseContext _db;
        private readonly IMapper _mapper;

        public CommentRepository(DatabaseContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<bool> AddUpdate(CommentDtoAddUpdate commentDto)
        {
            try
            {
                // Create a new comment object
                Comment comment = new Comment
                {
                    CommentId = commentDto.CommentId,
                    Content = commentDto.Content,
                    CreatedAt = commentDto.CreatedAt,
                    UpdatedDate = commentDto.UpdatedDate,
                    PostId = commentDto.PostId,
                    AuthorUserId = commentDto.AuthorUserId,
                };

                // Check if the provided Author exists in the database
                var existingUser = await _db.Users.FindAsync(commentDto.AuthorUserId);
                if (existingUser == null)
                {
                    // If the Author doesn't exist, return false or handle the error as needed
                    return false;
                }

                // Set the Author property of the comment to the existing user
                comment.Author = existingUser;

                // If the comment is a reply to a parent comment
                if (commentDto.ParentCommentId != null)
                {
                    // Find the parent comment in the database
                    var parentComment = await _db.Comments.FindAsync(commentDto.ParentCommentId);
                    if (parentComment == null)
                    {
                        // If the parent comment doesn't exist, return false or handle the error as needed
                        return false;
                    }

                    // Set the ParentCommentId and ChildComment properties to establish the parent-child relationship
                    comment.ParentCommentId = commentDto.ParentCommentId;
                    parentComment.Replies ??= new List<Comment>();
                    parentComment.Replies.Add(comment);
                }
                else
                {
                    // If it's not a reply, just add or update the comment as before
                    if (comment.CommentId == 0)
                    {
                        comment.CreatedAt = DateTime.Now;
                        await _db.Comments.AddAsync(comment);
                    }
                    else
                    {
                        comment.UpdatedDate = DateTime.Now;
                        _db.Comments.Update(comment);
                    }
                }


                // Save changes to the database
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Handle any exceptions and return false
                return false;
            }
        }



        public async Task<bool> Delete(int commentId)
        {
            try
            {
                var comment = await _db.Comments.FindAsync(commentId);
                if (comment == null)
                {
                    return false;
                }

                _db.Comments.Remove(comment);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<CommentDto>> GetAllByPostId(int postId)
        {
            var comments = await _db.Comments
                .Include(c => c.Author) // Include the Author entity
                .Include(c => c.Post) // Include the Post entity
                .Where(c => c.PostId == postId)
                .ToListAsync();

            // Map comments to CommentDto objects
            var commentDtos = _mapper.Map<List<CommentDto>>(comments);

            return commentDtos;
        }



        public async Task<bool> LikeComment(LikeCommentDTO likeCommentDto)
        {
            try
            {
                var comment = await _db.Comments.FindAsync(likeCommentDto.CommentId);
                if (comment == null)
                {
                    return false; // Comment not found
                }

                // Check if the like already exists
                var existingLike = await _db.Likes.FirstOrDefaultAsync(l => l.UserId == likeCommentDto.UserId && l.CommentId == likeCommentDto.CommentId);
                if (existingLike == null)
                {
                    // Create a new like
                    var like = new Like
                    {
                        UserId = likeCommentDto.UserId,
                        CommentId = likeCommentDto.CommentId,
                        PostId = null // Assuming this is for comment liking
                    };

                    _db.Likes.Add(like);
                }

                // Increment like count for the comment
                comment.LikeCount++;
                await _db.SaveChangesAsync();

                return true; // Like added successfully
            }
            catch (Exception)
            {
                return false; // Error occurred while liking the comment
            }
        }

        public async Task<bool> UnlikeComment(LikeCommentDTO unlikeCommentDto)
        {
            try
            {
                var comment = await _db.Comments.FindAsync(unlikeCommentDto.CommentId);
                if (comment == null)
                {
                    return false; // Comment not found
                }

                // Find the existing like
                var existingLike = await _db.Likes.FirstOrDefaultAsync(l => l.UserId == unlikeCommentDto.UserId && l.CommentId == unlikeCommentDto.CommentId);
                if (existingLike != null)
                {
                    // Remove the existing like
                    _db.Likes.Remove(existingLike);

                    // Decrement like count for the comment
                    comment.LikeCount--;
                    await _db.SaveChangesAsync();
                }

                return true; // Unlike operation successful
            }
            catch (Exception)
            {
                return false; // Error occurred while unliking the comment
            }
        }





    }
}

