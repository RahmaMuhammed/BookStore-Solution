using BookStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Core.Specifications
{
    public class BookWithFiltrationForCountAsync : BaseSpecifications<Book>
    {
        public BookWithFiltrationForCountAsync(BookSpecParams Params) : base
            (B =>
            (!Params.GenreId.HasValue || B.genres.Any(g => g.Id == Params.GenreId))
            &&
            (!Params.FormatId.HasValue || B.Formats.Any(g => g.Id == Params.FormatId))
            )
        { }
    }
}
