using AutoMapper;
using BookStore.Core.Entities;
using BookStore.DTOs;

namespace BookStore.Helpers
{
    public class BookPictureUrlResolve : IValueResolver<Book, BookToReturnDto, string>
    {
        private readonly IConfiguration _configuration;

        public BookPictureUrlResolve(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Book source, BookToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.pictureUrl))
                return $"{_configuration["ApiBaseUrl"]}{source.pictureUrl}";
            return "";

        }
    }
}
