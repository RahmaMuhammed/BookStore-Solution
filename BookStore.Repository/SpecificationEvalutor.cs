using BookStore.Core.Entities;
using BookStore.Core.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Repository
{
    public static class SpecificationEvalutor<T> where T : BaseEntity
    {

        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecifications<T> Spec)
        {
            var query = inputQuery; 
            if (Spec.Criteria is not null)
            {
                query = query.Where(Spec.Criteria);
            }
            if(Spec.OrderBy is not null)
            {
                query = query.OrderBy(Spec.OrderBy);
            }
            if (Spec.OrderByDescending is not null)
            {
                query = query.OrderByDescending(Spec.OrderByDescending);
            }
            if(Spec.IsPaginationEnable)
            {
                query = query.Skip(Spec.Skip).Take(Spec.Take);
            }

            query = Spec.Includes.Aggregate(query, 
                (CurrentQuery, IncludeExpressions) => CurrentQuery.Include(IncludeExpressions));

            return query;
        }
    }
}
