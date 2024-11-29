namespace BookStore.DTOs
{
    public class BookToCreateDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string pictureUrl { get; set; }
        public int Stock { get; set; }
        public double Price { get; set; }
        public int AuthorId { get; set; }
        public List<int> FormatsIDs { get; set; } = new List<int>();
        public List<int> genresIDs { get; set; } = new List<int>();
    }
}
