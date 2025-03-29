using BlogApi.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Core.Repos
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user , string password);
        Task<User> GetByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<User> GetByUserNameAsync(string username);
        Task<User> GetByIdAsync(string userId);
        Task<bool> UpdateAsync(User user);

    }
}
