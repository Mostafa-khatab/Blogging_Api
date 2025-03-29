using BlogApi.Core.Models;
using BlogApi.Core.Repos;
using BlogApi.EF.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.EF.Repositories
{
    public class FollowRepository : IFollowRepository
    {
        private readonly AppDbContext _context;

        public FollowRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> FollowUserAsync(string followerId, string followingId)
        {
            if (followerId == followingId) return false;

            var existingFollow = await _context.UsersFollows
                .AnyAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);
            if (existingFollow) return false;

            var follow = new UserFollow
            {
                FollowerId = followerId,
                FollowingId = followingId
            };

            await _context.UsersFollows.AddAsync(follow);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UnfollowUserAsync(string followerId, string followingId)
        {
            var follow = await _context.UsersFollows
                .FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);

            if (follow == null) return false;

            _context.UsersFollows.Remove(follow);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<object>> GetFollowersAsync(string userId)
        {
            return await _context.UsersFollows
                .Where(f => f.FollowingId == userId)
                .Include(f => f.Follower)
                .Select(f => new
                {
                    UserId = f.Follower.Id,
                    UserName = f.Follower.UserName,
                    Email = f.Follower.Email
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<object>> GetFollowingAsync(string userId)
        {
            return await _context.UsersFollows
                .Where(f => f.FollowerId == userId)
                .Include(f => f.Following)
                .Select(f => new
                {
                    UserId = f.Following.Id,
                    UserName = f.Following.UserName,
                    Email = f.Following.Email
                })
                .ToListAsync();
        }
    }
}
