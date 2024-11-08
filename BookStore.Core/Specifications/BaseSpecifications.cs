using BookStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Core.Specifications
{
    public class BaseSpecifications<T> : ISpecifications<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDescending { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPaginationEnable { get; set; }

        // Get All
        public BaseSpecifications() { }

        // Get By Id
        public BaseSpecifications(Expression<Func<T, bool>> CriteriaExpression)
        {
            Criteria = CriteriaExpression;
        }
        // Order By
        public void AddOrderBy(Expression<Func<T, object>> Orderexpression)
        {
            OrderBy = Orderexpression;
        }

        public void AddOrderByDescending(Expression<Func<T, object>> Orderexpression)
        {
            OrderByDescending = Orderexpression;
        }
        // Take And Skip
        public void ApplyPagination(int skip, int take)
        {
            IsPaginationEnable = true;
            Take = take;
            Skip = skip;
        }

      
    }
}
