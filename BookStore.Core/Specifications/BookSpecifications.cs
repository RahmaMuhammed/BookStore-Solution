using BookStore.Core.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Core.Specifications
{
    public class BookSpecifications : BaseSpecifications<Book>
    {
        public BookSpecifications(BookSpecParams Params) :base
            (B =>
            (!Params.GenreId.HasValue|| B.genres.Any(g => g.Id == Params.GenreId))
            &&
            (!Params.FormatId.HasValue|| B.Formats.Any(g => g.Id == Params.FormatId))
            ) 
        {
            if (!string.IsNullOrEmpty(Params.sort))
            {
                switch (Params.sort)
                {
                    case "PriceAsc":
                        AddOrderBy(P => P.Price);
                        break ;
                    case "PriceDesc":
                        AddOrderByDescending(P => P.Price);
                        break ;
                    case "NameDesc":
                        AddOrderByDescending(T => T.Title);
                        break ;
                     default:
                        AddOrderBy(T => T.Title);
                        break ;
                }
            }

            ApplyPagination(Params.PageSize * (Params.PageIndex - 1), Params.PageSize);
        
        
        }
        public BookSpecifications(int id) : base(B => B.Id == id)
        {
            
        }
    }
}
