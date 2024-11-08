namespace BookStore.DTOs
{
    public class FormatDto : BookFormatDto
    {
        public List<BookToReturnDto> Books { get; set; }
    }
}
