using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EmployeeCrud.Models.Interfaces;
using EmployeeCrud.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeCrud.Repositories
{
    // Abstract repository class implementing the generic repository interface
    public abstract class Repository<T> : IRepository<T> where T : class, IEntity
    {
        protected readonly DbContext _dbContext;

        protected Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Retrieve an entity by its ID
        public virtual async Task<T> GetById(string id)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(entity => entity.Id == id);
        }

        // Retrieve all entities
        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        // Retrieve entities based on a condition
        public virtual async Task<IEnumerable<T>> GetByCondition(Expression<Func<T, bool>> expression)
        {
            return await _dbContext.Set<T>().Where(expression).ToListAsync();
        }

        // Create a new entity
        public virtual async Task<T> Create(T entity)
        {
            if (entity.Id is null)
            {
                entity.Id = Guid.NewGuid().ToString();
            }

            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return await GetById(entity.Id);
        }

        // Update an existing entity
        public virtual async Task<T> Update(T entity)
        {
            var entry = _dbContext.Add(entity);
            entry.State = EntityState.Unchanged;

            _dbContext.Set<T>().Update(entity);
            await _dbContext.SaveChangesAsync();

            return await GetById(entity.Id);
        }

        // Delete an entity by its ID
        public virtual async Task Delete(string id)
        {
            var entity = await GetById(id);

            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
