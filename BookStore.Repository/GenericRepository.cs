using BookStore.Core.Entities;
using BookStore.Core.Repositories;
using BookStore.Core.Specifications;
using BookStore.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public async Task<T> Add(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            _dbContext.SaveChanges();
            return entity;
        }
         
        public async Task<T> Update(T entity)
        {
             _dbContext.Set<T>().Update(entity);
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                throw new InvalidOperationException("Error while saving entity changes.", ex);
            }
            return entity;
        }

        public async Task<T> Delete(T entity)
        {
            // Mark the entity for removal
            _dbContext.Set<T>().Remove(entity);

            try
            {
                // Save the changes to the database
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                throw new InvalidOperationException("Error while deleting entity.", ex);
            }

            return entity; // Optionally, return the deleted entity if needed
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(ISpecifications<T> spec)
            => await SpecificationEvalutor<T>.GetQuery(_dbContext.Set<T>(), spec).ToListAsync();
        

        public async Task<T> GetByIdAsync(ISpecifications<T> spec)
            => await ApplySpecification(spec).FirstOrDefaultAsync();

        public async Task<int> GetCountWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecifications<T> spec)
        {
            return SpecificationEvalutor<T>.GetQuery(_dbContext.Set<T>(), spec);
        }

        public async Task<List<T>> GetRelatedEntitiesByIds(IEnumerable<int> ids)
        {

            try
            {
                var entity = await _dbContext.Set<T>()
                                   .Where(entity => ids.Contains(entity.Id))
                                   .ToListAsync();

                return entity;
            }
            catch (Exception ex)
            {
                return null;
            }


        }

        public async Task<T> GetRelatedEntitiesById(int id)
        {
            try
            {
                var entity = await _dbContext.Set<T>()
                                              .Where(entity => entity.Id == id)
                                              .FirstOrDefaultAsync();

                return entity; 
            }
            catch (Exception ex)
            {
                return null; 
            }
        }
    }
}
