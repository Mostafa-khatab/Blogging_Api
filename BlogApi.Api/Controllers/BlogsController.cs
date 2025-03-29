using BlogApi.Core.Dtos;
using BlogApi.Core.Models;
using BlogApi.Core.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlogApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BlogsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public BlogsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var blog = await _unitOfWork.Blogs.GetByIdAsync(id, userId);
                if (blog == null)
                {
                    return NotFound($"No blog found with id {id} or you are not authorized to access it.");
                }
                return Ok(blog);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var blogs = await _unitOfWork.Blogs.GetAllAsync(userId);
                return Ok(blogs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlog([FromBody] BlogDto dto)
        {
            if (dto == null)
                return BadRequest("Blog data is required.");

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User not authenticated.");

                var newBlog = new Blog
                {
                    Title = dto.Title,
                    Content = dto.Content,
                    UserId = userId
                };

                var createdBlog = await _unitOfWork.Blogs.CreateAsync(newBlog);

                return CreatedAtAction(nameof(GetById), new { id = createdBlog.Id }, createdBlog);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBlog(int id, BlogDto dto)
        {
            if (dto == null)
                return BadRequest("Blog data is required.");

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var existingBlog = await _unitOfWork.Blogs.GetByIdAsync(id, userId);
                if (existingBlog == null)
                    return NotFound($"No blog found with id {id} or you are not authorized to update it.");

                existingBlog.Title = dto.Title;
                existingBlog.Content = dto.Content;

                var updatedBlog = await _unitOfWork.Blogs.UpdateAsync(existingBlog, userId);
                return Ok(updatedBlog);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid blog id.");

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var deletedBlog = await _unitOfWork.Blogs.DeleteAsync(id, userId);
                if (deletedBlog == null)
                    return NotFound($"No blog found with id {id} or you are not authorized to delete it.");

                return Ok(deletedBlog);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
