using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Core.Specifications
{
    public class BookSpecParams
    {
        public string? sort { get; set; }
        public int? GenreId { get; set; }
        public int? FormatId { get; set; }
        public int? AuthorId { get; set; }
        private int pageSize = 5;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > 10 ? 10 : value; }
        }
        public int PageIndex { get; set; } = 1;
    }
}
