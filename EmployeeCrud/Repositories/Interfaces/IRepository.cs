using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EmployeeCrud.Models.Interfaces;

namespace EmployeeCrud.Repositories.Interfaces
{
    // Generic repository interface for CRUD operations
    public interface IRepository<T> where T : class, IEntity
    {
        // Retrieve an entity by its ID
        public Task<T> GetById(string id);

        // Retrieve all entities
        public Task<IEnumerable<T>> GetAll();

        // Retrieve entities based on a condition
        public Task<IEnumerable<T>> GetByCondition(Expression<Func<T, bool>> expression);

        // Create a new entity
        public Task<T> Create(T entity);

        // Update an existing entity
        public Task<T> Update(T entity);

        // Delete an entity by its ID
        public Task Delete(string id);
    }
}
