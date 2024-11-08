using BookStore.Errors;

namespace BookStore.Extentions
{
    public class ApiExeptionResponse : ApiResponse
    {
        public string? Details { get; set; }
        public ApiExeptionResponse(int StatusCode, string? Message = null, string? details = null):base(StatusCode, Message)
        {
            Details = details;
        }
    }
}
