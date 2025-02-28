using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EmployeeCrud.Models.Interfaces;
using EmployeeCrud.Repositories.Interfaces;

namespace EmployeeCrud.Services.Interfaces
{
    // Abstract service class implementing the generic service interface
    public abstract class Service<T> : IService<T> where T : class, IEntity
    {
        protected readonly IRepository<T> _repository;

        // Constructor to initialize the repository
        public Service(IRepository<T> repository)
        {
            _repository = repository;
        }

        // Retrieve an entity by its ID
        public virtual async Task<T> GetById(string id) => await _repository.GetById(id);

        // Retrieve all entities
        public virtual async Task<IEnumerable<T>> GetAll() => await _repository.GetAll();

        // Retrieve entities based on a condition
        public virtual async Task<IEnumerable<T>> GetByCondition(Expression<Func<T, bool>> expression) => await _repository.GetByCondition(expression);

        // Create a new entity
        public virtual async Task<T> Create(T entity) => await _repository.Create(entity);

        // Update an existing entity
        public virtual async Task<T> Update(T entity) => await _repository.Update(entity);

        // Delete an entity by its ID
        public virtual async Task Delete(string id) => await _repository.Delete(id);
    }
}
