using BookStore.Core.Entities;

namespace BookStore.DTOs
{
    public class BookToReturnDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string pictureUrl { get; set; }
        public int Stock { get; set; }
        public double Price { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public List<BookFormatDto> Formats { get; set; } = new List<BookFormatDto>();
        public List<BookGenreDto> genres { get; set; } = new List<BookGenreDto>();
    }
}
