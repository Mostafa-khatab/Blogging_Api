using BlogApi.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Core.Repos
{
    public interface IFollowRepository
    {
        Task<bool> FollowUserAsync(string followerId, string followingId);
        Task<bool> UnfollowUserAsync(string followerId, string followingId);
        Task<IEnumerable<object>> GetFollowersAsync(string userId);
        Task<IEnumerable<object>> GetFollowingAsync(string userId);
    }
}
