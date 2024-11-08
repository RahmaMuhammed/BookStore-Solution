using BookStore.Core.Entities;
using BookStore.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Core.Repositories
{
    public interface IGenericRepository <T> where T : BaseEntity
    {
        public Task<IReadOnlyList<T>> GetAllAsync(ISpecifications<T> spec);
        public Task<T> GetByIdAsync(ISpecifications<T> spec);
        public Task<T> GetByNameAsync(ISpecifications<T> spec);
        public Task<int> GetCountWithSpecAsync(ISpecifications<T> spec);
    }
}

