using BookStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Core.Specifications
{
    public class GenreSpecifications : BaseSpecifications<BookGenre>
    {
        public GenreSpecifications(string name) : base(N => N.Name == name) 
        {
            
        }
    }
}
