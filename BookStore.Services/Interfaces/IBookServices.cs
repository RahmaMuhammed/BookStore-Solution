using BookStore.Core.Entities;
using BookStore.Core.Repositories;
using BookStore.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BookStore.Services.Interfaces
{
    public interface IBookServices
    {
        Task<IReadOnlyList<Book>> GetBooksAsync();
        Task<Book> GetBookByIdAsync(int id);
        Task<Book> CreateBookAsync(BookToCreateDTO book);
        Task<Book> UpdateBookAsync(Book book);
        Task<Book> DeleteBookAsync(int id);
    }
}
