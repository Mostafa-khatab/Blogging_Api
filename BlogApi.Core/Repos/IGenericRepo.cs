using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Core.Repos
{
    public interface IGenericRepo<T> where T : class
    {
        Task<T> GetByIdAsync(int id, string userId = null);
        Task<IEnumerable<T>> GetAllAsync(string userId = null);
        Task<T> CreateAsync(T item);
        Task<T> UpdateAsync(T item, string userId = null);
        Task<T> DeleteAsync(int id, string userId = null);

    }
}
