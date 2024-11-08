using BookStore.Core.Entities;
using BookStore.Core.Repositories;
using BookStore.Core.Specifications;
using BookStore.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext dbContext)
        {
           _dbContext = dbContext;
        }
        public async Task<IReadOnlyList<T>> GetAllAsync(ISpecifications<T> spec)
            => await SpecificationEvalutor<T>.GetQuery(_dbContext.Set<T>(), spec).ToListAsync();
        

        public async Task<T> GetByIdAsync(ISpecifications<T> spec)
            => await ApplySpecification(spec).FirstOrDefaultAsync();

        public async Task<T> GetByNameAsync(ISpecifications<T> spec)
             => await ApplySpecification(spec).FirstOrDefaultAsync();

        public async Task<int> GetCountWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecifications<T> spec)
        {
            return SpecificationEvalutor<T>.GetQuery(_dbContext.Set<T>(), spec);
        }

    }
}
