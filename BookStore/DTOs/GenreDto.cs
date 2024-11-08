namespace BookStore.DTOs
{
    public class GenreDto : BookGenreDto
    {
        public List<BookToReturnDto> Books { get; set; }
    }
}
