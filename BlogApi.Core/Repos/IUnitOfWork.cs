using System.Threading.Tasks;
using BlogApi.Core.Models;

namespace BlogApi.Core.Repos
{
    public interface IUnitOfWork
    {
        IGenericRepo<Blog> Blogs { get; }
        IGenericRepo<Comment> Comments { get; }
        IUserRepository Users { get; }
        IFollowRepository Follows { get; }
        Task SaveAsync();
    }
}
