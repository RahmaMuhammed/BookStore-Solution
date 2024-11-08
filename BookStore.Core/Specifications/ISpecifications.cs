using BookStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Core.Specifications
{
    public interface ISpecifications<T> where T : BaseEntity
    {
        // Sign For Property For Where condition
        public Expression<Func<T, bool>> Criteria { get; set; }

        // Include
        public List<Expression<Func<T, object>>> Includes { get; set; }

        // Order By
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDescending { get; set; }

        // Take
        public int Take { get; set; }
        // Skip
        public int Skip { get; set; }
        // Make Pagination Or Not
        public bool IsPaginationEnable { get; set; }
    }
}
