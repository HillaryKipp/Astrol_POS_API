using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Persistence.Contexts;
using AstrolPOSAPI.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseAuditableEntity
    {
        private readonly AppDbContext _dbContext;

        public GenericRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<T> Entities => _dbContext.Set<T>();

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbContext
                .Set<T>()
                .ToListAsync();
        }

        public async Task<T?> GetByIdAsync(string id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(T entity)
        {
            T? exist = _dbContext.Set<T>().Find(entity.Id);
            if (exist != null)
            {
                _dbContext.Entry(exist).CurrentValues.SetValues(entity);
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(T entity)
        {
            // Soft delete: Set DeletedDate instead of removing
            entity.DeletedDate = DateTime.UtcNow;
            _dbContext.Set<T>().Update(entity);
            return Task.CompletedTask;
        }
    }
}
