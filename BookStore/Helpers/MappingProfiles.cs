using AutoMapper;
using BookStore.Core.Entities;
using BookStore.DTOs;
using BookStore.Services.DTOs;

namespace BookStore.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<BookFormat, BookFormatDto>();
            CreateMap<BookGenre, BookGenreDto>();
            CreateMap<BookGenre, GenreDto>();
            CreateMap<BookFormat, FormatDto>();
               // .ForMember(dto => dto.Books, opt => opt.MapFrom(src => src.Books));

            CreateMap<Book, BookToReturnDto>()
               .ForMember(dto => dto.AuthorName, opt => opt.MapFrom(b => b.Author.Name)) // Map AuthorName from Author.Name
               .ForMember(dto => dto.Formats, opt => opt.MapFrom(b => b.Formats)) // Map Formats collection
               .ForMember(dto => dto.genres, opt => opt.MapFrom(b => b.genres)) // Map genres collection
               .ForMember(dto => dto.pictureUrl, opt => opt.MapFrom<BookPictureUrlResolve>());

            CreateMap<BookToCreateDTO, Book>()
               .ForMember(b => b.Formats, opt => opt.Ignore()) 
               .ForMember(b => b.genres, opt => opt.Ignore()); 

        }
    }
}
