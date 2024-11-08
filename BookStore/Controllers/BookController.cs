using AutoMapper;
using BookStore.Core.Entities;
using BookStore.Core.Repositories;
using BookStore.Core.Specifications;
using BookStore.DTOs;
using BookStore.Errors;
using BookStore.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Reflection.Metadata.BlobBuilder;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookStore.Controllers
{
    public class BookController : ApiBaseController
    {
        private readonly IGenericRepository<Book> _bookRepo;
        private readonly IMapper _mapper;

        public BookController(IGenericRepository<Book> bookRepo, IMapper mapper) 
        {
            _bookRepo = bookRepo;
            _mapper = mapper;
        }
        // Get All Books
        [HttpGet]
        [ProducesResponseType(typeof(BookToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<BookToReturnDto>>> GetBooks([FromQuery] BookSpecParams Params)
        {
            var spec = new BookSpecifications(Params);
            var Books = await _bookRepo.GetAllAsync(spec);
            var MapBooks = _mapper.Map<IReadOnlyList<Book>, IReadOnlyList<BookToReturnDto>>(Books);
            var CountSpec = new BookWithFiltrationForCountAsync(Params);
            var Count = await  _bookRepo.GetCountWithSpecAsync(CountSpec);
            return Ok(new Pagination<BookToReturnDto>(Params.PageSize, Params.PageIndex, MapBooks, Count));
        }

        // Get Book By Id
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BookToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookToReturnDto>> GetBookById(int id)
        {
            var spec = new BookSpecifications(id);
            var book = await _bookRepo.GetByIdAsync(spec);
            if (book == null) return NotFound(new ApiResponse(404));
            var MapBook = _mapper.Map<Book,BookToReturnDto>(book);
            return Ok(MapBook);
        }
    }
}
