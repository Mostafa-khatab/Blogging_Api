using BlogApi.Core.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FollowController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public FollowController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("follow/{userId}")]
        public async Task<IActionResult> FollowUser(string userId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);  
            if (string.IsNullOrEmpty(currentUserId))
                return Unauthorized(new { message = "User not authenticated." });

            var result = await _unitOfWork.Follows.FollowUserAsync(currentUserId, userId);
            if (!result)
                return BadRequest(new { message = "Failed to follow user or already following." });

            return Ok(new { message = "User followed successfully." });
        }

        [HttpPost("unfollow/{userId}")]
        public async Task<IActionResult> UnfollowUser(string userId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId))
                return Unauthorized(new { message = "User not authenticated." });

            var result = await _unitOfWork.Follows.UnfollowUserAsync(currentUserId, userId);
            if (!result)
                return BadRequest(new { message = "Failed to unfollow user." });

            return Ok(new { message = "User unfollowed successfully." });
        }

        [HttpGet()]
        [Route("Followers")]
        public async Task<IActionResult> GetFollowers()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var followers = await _unitOfWork.Follows.GetFollowersAsync(userId);
            return Ok(followers);
        }

        [HttpGet()]
        [Route("Following")]
        public async Task<IActionResult> GetFollowing()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var following = await _unitOfWork.Follows.GetFollowingAsync(userId);
            return Ok(following);
        }
    }
}