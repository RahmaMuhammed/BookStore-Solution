using AutoMapper;
using BookStore.Core.Entities;
using BookStore.Core.Repositories;
using BookStore.Core.Specifications;
using BookStore.DTOs;
using BookStore.Errors;
using BookStore.Helpers;
using BookStore.Repository.Data;
using BookStore.Services.DTOs;
using BookStore.Services.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookStore.Controllers
{
    public class BookController : ApiBaseController
    {
        private readonly IGenericRepository<Book> _bookRepo;
        private readonly IMapper _mapper;
        private readonly StoreContext _context;
        private readonly BookService _bookService;

        public BookController(IGenericRepository<Book> bookRepo, IMapper mapper, StoreContext context, BookService bookService)
        {
            _bookRepo = bookRepo;
            _mapper = mapper;
            _context = context;
            _bookService = bookService;
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
            var Count = await _bookRepo.GetCountWithSpecAsync(CountSpec);
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
            var MapBook = _mapper.Map<Book, BookToReturnDto>(book);
            return Ok(MapBook);
        }

        // Add New Book
        [HttpPost]
        [ProducesResponseType(typeof(BookToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookToReturnDto>> AddNewBook([FromBody] BookToCreateDTO bookDto)
        {
            try
            {
                var addedBook = _bookService.CreateBookAsync(bookDto);
                return CreatedAtAction(nameof(GetBookById), new { id = addedBook.Id }, addedBook);
            }
            catch (ApiResponse ex)
            {
                // Catch the ApiResponse exception and return the appropriate BadRequest with message
                return BadRequest(new ApiResponse(ex.StatusCode, ex.Message));
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, new ApiResponse(500, "An unexpected error occurred."));
            }
        }
        // Update book
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BookToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookToReturnDto>> UpdateBook(int id, [FromBody] BookToUpdateDTO book)
        {
            var existingBook = await _bookRepo.GetByIdAsync(new BookSpecifications(id));
            if (existingBook == null) return NotFound(new ApiResponse(404, "This Book Not Found"));

            if (!string.IsNullOrEmpty(book.Title) && existingBook.Title != book.Title)
                existingBook.Title = book.Title;


            if (!string.IsNullOrEmpty(book.Description) && existingBook.Description != book.Description)
                existingBook.Description = book.Description;

            if (book.Stock.HasValue && book.Stock != -1 && existingBook.Stock != book.Stock)
                existingBook.Stock = book.Stock.Value;

            if (book.Price.HasValue && book.Price != -1 && existingBook.Price != book.Price)
                existingBook.Price = book.Price.Value;

            if (book.pictureUrl != null && existingBook.pictureUrl != book.pictureUrl)
                existingBook.pictureUrl = book.pictureUrl;


            if (book.genresIDs != null && book.genresIDs.Any())
            {
                var genres = await _context.BookGenres
                                  .Where(f => book.genresIDs.Contains(f.Id))
                                  .ToListAsync();

                if (book.genresIDs.Count() != genres.Count())
                {
                    return BadRequest(new ApiResponse(400, "Invalid Genres IDs provided."));
                }

                // Remove old genres that are not in the new list
                var genresToRemove = existingBook.genres
                                                  .Where(f => !book.genresIDs.Contains(f.Id))
                                                  .ToList();
                foreach (var genre in genresToRemove)
                {
                    existingBook.genres.Remove(genre);
                }

                // Add new genres that are not already associated
                foreach (var genre in genres)
                {
                    if (!existingBook.genres.Any(f => f.Id == genre.Id))
                    {
                        existingBook.genres.Add(genre);
                    }
                }
            }



            if (book.FormatsIDs != null && book.FormatsIDs.Any())
            {
                var formats = await _context.BookFormats
                                   .Where(f => book.FormatsIDs.Contains(f.Id))
                                   .ToListAsync();

                if (book.FormatsIDs.Count() != formats.Count())
                {
                    return BadRequest(new ApiResponse(400, "Invalid Format IDs provided."));
                }

                // Remove old formats that are not in the new list
                var formatsToRemove = existingBook.Formats
                                                  .Where(f => !book.FormatsIDs.Contains(f.Id))
                                                  .ToList();
                foreach (var format in formatsToRemove)
                {
                    existingBook.Formats.Remove(format);
                }

                // Add new formats that are not already associated
                foreach (var format in formats)
                {
                    if (!existingBook.Formats.Any(f => f.Id == format.Id))
                    {
                        existingBook.Formats.Add(format);
                    }
                }
            }

            if (book.AuthorId != null)
            {
                var Author = await _context.BookAuthors
                                        .Where(A => A.Id == book.AuthorId)
                                        .FirstOrDefaultAsync();
                if (Author == null)
                {
                    return BadRequest(new ApiResponse(400, "Invalid Genre IDs provided."));
                }
                existingBook.Author = Author;
            }

            var UpdatedBook = await _bookRepo.Update(existingBook);
            if (UpdatedBook == null)
            {
                return BadRequest(new ApiResponse(400, "Failed to update the book."));
            }
            var BookToReturn = _mapper.Map<Book, BookToReturnDto>(UpdatedBook);
            return Ok(BookToReturn);
        }

        // Delete Book
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteBook(int id)
        {
            var existingBook = await _bookRepo.GetByIdAsync(new BookSpecifications(id));
            if (existingBook == null) return NotFound(new ApiResponse(404, "This Book Not Found"));
            var Formats = existingBook.Formats.Where(A => A.Id == id).ToList();
            foreach (var format in Formats)
            {
                existingBook.Formats.Remove(format);
            }

            var Genres = existingBook.genres.Where(A => A.Id == id).ToList();
            foreach (var genre in Genres)
            {
                existingBook.genres.Remove(genre);
            }

            var DeletedBook = await _bookRepo.Delete(existingBook);
            if (DeletedBook == null) return BadRequest(new ApiResponse(400, "Failed to deleted the book."));

            var BookToReturn = _mapper.Map<Book, BookToReturnDto>(DeletedBook);
            return Ok(BookToReturn);

        }

    }
}