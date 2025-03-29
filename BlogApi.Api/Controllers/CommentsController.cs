using BlogApi.Core.Dtos;
using BlogApi.Core.Models;
using BlogApi.Core.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var comment = await _unitOfWork.Comments.GetByIdAsync(id);
                if (comment == null)
                    return NotFound(new { message = $"No comment found with id {id}." });

                return Ok(new
                {
                    commentId = comment.Id,
                    content = comment.Content,
                    blogId = comment.BlogId
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var comments = await _unitOfWork.Comments.GetAllAsync();
                var result = comments.Select(comment => new
                {
                    commentId = comment.Id,
                    content = comment.Content,
                    blogId = comment.BlogId
                });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CommentDto dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Comment data is required." });

            var blogExists = await _unitOfWork.Blogs.GetByIdAsync(dto.BlogId);
            if (blogExists == null)
                return NotFound(new { message = "No blog found with the given BlogId." });

            try
            {
                var commentEntity = new Comment
                {
                    Content = dto.Content,
                    BlogId = dto.BlogId
                };

                var createdComment = await _unitOfWork.Comments.CreateAsync(commentEntity);
                return CreatedAtAction(nameof(GetById), new { id = createdComment.Id }, new
                {
                    message = "Comment created successfully.",
                    commentId = createdComment.Id,
                    content = createdComment.Content,
                    blogId = createdComment.BlogId
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] CommentDto dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Comment data is required." });

            if (id <= 0)
                return BadRequest(new { message = "Invalid comment id." });

            try
            {
                var existingComment = await _unitOfWork.Comments.GetByIdAsync(id);
                if (existingComment == null)
                    return NotFound(new { message = $"No comment found with id {id}." });

                if (existingComment.BlogId != dto.BlogId)
                {
                    var blogExists = await _unitOfWork.Blogs.GetByIdAsync(dto.BlogId);
                    if (blogExists == null)
                        return NotFound(new { message = "No blog found with the given BlogId." });
                }

                existingComment.Content = dto.Content;
                existingComment.BlogId = dto.BlogId;

                var updatedComment = await _unitOfWork.Comments.UpdateAsync(existingComment);
                return Ok(new
                {
                    message = "Comment updated successfully.",
                    commentId = updatedComment.Id,
                    content = updatedComment.Content,
                    blogId = updatedComment.BlogId
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid comment id." });

            try
            {
                var deletedComment = await _unitOfWork.Comments.DeleteAsync(id);
                if (deletedComment == null)
                    return NotFound(new { message = $"No comment found with id {id}." });

                return Ok(new
                {
                    message = "Comment deleted successfully.",
                    commentId = deletedComment.Id
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
