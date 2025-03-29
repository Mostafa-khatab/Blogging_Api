using System;
using System.Linq;
using System.Threading.Tasks;
using BlogApi.Core.Models;
using BlogApi.Core.Repos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.EF.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;

        public UserRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<User> CreateAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            return user;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var normalizedEmail = _userManager.NormalizeEmail(email);
            return await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);
        }

        public async Task<User> GetByUserNameAsync(string username)
        {
            var normalizedUserName = _userManager.NormalizeName(username);
            return await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName);
        }

        public async Task<User> GetByIdAsync(string userId)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }
        public async Task<bool> UpdateAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "User cannot be null.");

            var existingUser = await _userManager.FindByIdAsync(user.Id);
            if (existingUser == null)
                throw new Exception($"User with ID {user.Id} does not exist.");

            if (!string.IsNullOrEmpty(user.UserName) && user.UserName != existingUser.UserName)
            {
                var userWithSameUsername = await _userManager.FindByNameAsync(user.UserName);
                if (userWithSameUsername != null && userWithSameUsername.Id != user.Id)
                    throw new Exception($"Username '{user.UserName}' is already taken by another user.");
            }

            if (!string.IsNullOrEmpty(user.Email) && user.Email != existingUser.Email)
            {
                var userWithSameEmail = await _userManager.FindByEmailAsync(user.Email);
                if (userWithSameEmail != null && userWithSameEmail.Id != user.Id)
                    throw new Exception($"Email '{user.Email}' is already registered by another user.");
            }

            existingUser.UserName = user.UserName ?? existingUser.UserName;
            existingUser.Email = user.Email ?? existingUser.Email;
            existingUser.Bio = user.Bio ?? existingUser.Bio;

            var result = await _userManager.UpdateAsync(existingUser);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            return true;
        }
    }
}
