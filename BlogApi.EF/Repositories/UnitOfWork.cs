using BlogApi.Core.Models;
using BlogApi.Core.Repos;
using BlogApi.EF.Data;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BlogApi.EF.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;
        private readonly UserManager<User> _userManager;

        private IGenericRepo<Blog> _blogs;
        private IGenericRepo<Comment> _comments;
        private IUserRepository _users;
        private IFollowRepository _follows;

        public UnitOfWork(AppDbContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IGenericRepo<Blog> Blogs => _blogs ??= new GenericRepo<Blog>(_db);
        public IGenericRepo<Comment> Comments => _comments ??= new GenericRepo<Comment>(_db);
        public IUserRepository Users => _users ??= new UserRepository(_userManager);
        public IFollowRepository Follows => _follows ??= new FollowRepository(_db);

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
