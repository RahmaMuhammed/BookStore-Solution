using AutoMapper;
using BookStore.Core.Entities;
using BookStore.Core.Repositories;
using BookStore.Errors;
using BookStore.Services.DTOs;
using BookStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services.Implementations
{
    public class BookService : IBookServices
    {
        private readonly IGenericRepository<Book> _bookRepo;
        private readonly IGenericRepository<BookGenre> _genrereRepo;
        private readonly IGenericRepository<BookFormat> _formateRepo;
        private readonly IGenericRepository<BookFormat> _authorRepo;
        private readonly IMapper _mapper;

        public BookService(IGenericRepository<Book> repository, IGenericRepository<BookGenre> genrerepository, IGenericRepository<BookFormat> formaterepository, IGenericRepository<BookFormat> authorRepo)
        {
            _bookRepo = repository;
            _genrereRepo = genrerepository;
            _formateRepo = formaterepository;
            _authorRepo = authorRepo;
        }

        public async Task<Book> CreateBookAsync(BookToCreateDTO book)
        {
            

            // Fetch genres based on GenreIds from the database
            var genres = await _genrereRepo.GetRelatedEntitiesByIds(book.genresIDs);

            var formats =await _formateRepo.GetRelatedEntitiesByIds(book.FormatsIDs);

            var author = await _authorRepo.GetRelatedEntitiesById(book.AuthorId);

            if (author == null)
            {
                throw new ApiResponse(400, "Invalid Author ID provided.");
            }

            if (!genres.Any() && genres.Count != book.genresIDs.Count)
            {
                throw new ApiResponse(400, "Invalid Genre IDs provided.");
            }

            if (!formats.Any() && formats.Count != book.FormatsIDs.Count)
            {
               throw new ApiResponse(400, "Invalid Format IDs provided.");
            }

            var newBook = _mapper.Map<BookToCreateDTO, Book>(book);

            // Associate genres with the book
            newBook.genres = genres;
            newBook.Formats = formats;
            newBook.Author.Id = author.Id;

            return newBook;
        }

        public Task<Book> DeleteBookAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Book> GetBookByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Book>> GetBooksAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Book> UpdateBookAsync(Book book)
        {
            throw new NotImplementedException();
        }
    }
}
