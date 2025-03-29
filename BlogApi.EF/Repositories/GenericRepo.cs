using BlogApi.Core.Models;
using BlogApi.Core.Repos;
using BlogApi.EF.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApi.EF.Repositories
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        private readonly AppDbContext _db;

        public GenericRepo(AppDbContext db)
        {
            _db = db;
        }

        public async Task<T> CreateAsync(T item)
        {
            await _db.Set<T>().AddAsync(item);
            await _db.SaveChangesAsync();
            return item;
        }

        public async Task<T> DeleteAsync(int id, string userId = null)
        {
            var entity = await GetByIdAsync(id, userId);

            if (entity == null)
                throw new Exception("Entity not found or unauthorized.");

            _db.Set<T>().Remove(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<T>> GetAllAsync(string userId = null)
        {
            IQueryable<T> query = _db.Set<T>();

            if (typeof(T) == typeof(Blog) && !string.IsNullOrEmpty(userId))
            {
                query = query.Cast<Blog>().Where(b => b.UserId == userId).Cast<T>();
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id, string userId = null)
        {
            IQueryable<T> query = _db.Set<T>();

            if (typeof(T) == typeof(Blog) && !string.IsNullOrEmpty(userId))
            {
                var blog = await _db.Blogs.FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);
                return blog as T;  
            }

            var entity = await _db.Set<T>().FindAsync(id);

            if (entity == null)
                throw new Exception("Entity not found.");
            return entity;
        }

        public async Task<T> UpdateAsync(T item, string userId = null)
        {
            if (typeof(T) == typeof(Blog))
            {
                var updatedBlog = item as Blog;

                var existingBlog = await _db.Blogs.FirstOrDefaultAsync(b => b.Id == updatedBlog.Id && b.UserId == userId);
                if (existingBlog == null)
                    throw new Exception("Blog not found or unauthorized.");

                existingBlog.Title = updatedBlog.Title;
                existingBlog.Content = updatedBlog.Content;

                _db.Blogs.Update(existingBlog);
            }
            else
            {
                _db.Set<T>().Update(item);
            }

            await _db.SaveChangesAsync();
            return item;
        }
    }
}
