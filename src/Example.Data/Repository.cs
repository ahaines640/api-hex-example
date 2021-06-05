using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Example.Data
{
    public class Repository<T> where T : Entity
    {
        private readonly ExampleDbContext _dbContext;
        private readonly DbSet<T> _entities;

        public Repository(
            ExampleDbContext dbContext)
        {
            _dbContext = dbContext;
            _entities = dbContext.Set<T>();
        }

        protected virtual Task<T> GetEntityAsync(int id)
        {
            return _entities.SingleOrDefaultAsync(entity => entity.Id == id);
        }

        protected virtual async Task<T> CreateEntityAsync(T entity, string modifiedBy)
        {
            entity.Modified = DateTimeOffset.UtcNow;
            entity.ModifiedBy = modifiedBy;

            await _entities.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        protected virtual async Task<T> UpdateEntityAsync(T entity, string modifiedBy)
        {
            entity.Modified = DateTimeOffset.UtcNow;
            entity.ModifiedBy = modifiedBy;

            await _dbContext.SaveChangesAsync();

            return entity;
        }

        protected virtual async Task DeleteEntityAsync(int id, string modifiedBy)
        {
            T entity = await _entities
                .SingleOrDefaultAsync(e => e.Id == id);

            if (entity == null)
                return;

            entity.IsDeleted = true;
            entity.Modified = DateTimeOffset.UtcNow;
            entity.ModifiedBy = modifiedBy;

            await _dbContext.SaveChangesAsync();
        }
    }
}
