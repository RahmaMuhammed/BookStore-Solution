using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Core.Entities
{
    public class Book : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string pictureUrl { get; set; }
        public int Stock { get; set; }
        public double Price { get; set; }
        public int AuthorId { get; set; }
        public virtual BookAuthor Author { get; set; }
        public virtual ICollection<BookFormat> Formats { get; set; } = new HashSet<BookFormat>();
        public virtual ICollection<BookGenre> genres { get; set; } = new HashSet<BookGenre>();
      
    }
}
