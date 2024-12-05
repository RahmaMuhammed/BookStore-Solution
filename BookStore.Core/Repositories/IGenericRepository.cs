using BookStore.Core.Entities;
using BookStore.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Core.Repositories
{
    public interface IGenericRepository <T> where T : BaseEntity
    {
        public Task<IReadOnlyList<T>> GetAllAsync(ISpecifications<T> spec);
        public Task<T> GetByIdAsync(ISpecifications<T> spec);
        public Task<int> GetCountWithSpecAsync(ISpecifications<T> spec);
        public Task<T> Add(T entity); 
        public Task<T> Update(T entity);
        public Task<T> Delete(T entity);

        public Task<List<T>> GetRelatedEntitiesByIds(IEnumerable<int> ids);
        public Task<T> GetRelatedEntitiesById(int id);
    }
}

