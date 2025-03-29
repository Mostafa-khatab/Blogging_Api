using BlogApi.Core.Dtos;
using BlogApi.Core.Models;
using BlogApi.Core.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlogApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenServices _tokenServices;

        public UsersController(IUnitOfWork unitOfWork, ITokenServices tokenServices)
        {
            _unitOfWork = unitOfWork;
            _tokenServices = tokenServices;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid data.");
            try
            {
                var existingUserByEmail = await _unitOfWork.Users.GetByEmailAsync(dto.Email);
                if (existingUserByEmail != null)
                    return BadRequest("A user with the same email already exists.");

                var existingUserByUserName = await _unitOfWork.Users.GetByUserNameAsync(dto.UserName);
                if (existingUserByUserName != null)
                    return BadRequest("A user with the same username already exists.");

                var newUser = new User
                {
                    UserName = dto.UserName,
                    Email = dto.Email
                };

                var createdUser = await _unitOfWork.Users.CreateAsync(newUser, dto.Password);
                return Ok(createdUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid data.");
            try
            {
                var user = await _unitOfWork.Users.GetByEmailAsync(dto.Email);
                if (user == null)
                    return Unauthorized("Invalid credentials.");

                bool isPasswordValid = await _unitOfWork.Users.CheckPasswordAsync(user, dto.Password);
                if (!isPasswordValid)
                    return Unauthorized("Invalid credentials.");

                var token = _tokenServices.CreatToken(user);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("profile")]
        [Authorize]  
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token.");

            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                return NotFound("User not found.");

            var userProfile = new UserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                Bio = user.Bio,
            };

            return Ok(userProfile);
        }

        [HttpPut("profile")]
        [Authorize]  
        public async Task<IActionResult> UpdateProfile([FromBody] UserDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token.");

            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                return NotFound("User not found.");

            user.UserName = dto.UserName ?? user.UserName;
            user.Email = dto.Email ?? user.Email;
            user.Bio = dto.Bio ?? user.Bio;

            var updated = await _unitOfWork.Users.UpdateAsync(user);
            if (!updated)
                return BadRequest("Failed to update profile.");

            return NoContent();  
        }
    }
}
