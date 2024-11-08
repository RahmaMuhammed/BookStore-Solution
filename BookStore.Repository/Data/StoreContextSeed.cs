using BookStore.Core.Entities;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;


namespace BookStore.Repository.Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext dbContext)
        {
            if (!dbContext.BookGenres.Any())
            {
                var GenreData = File.ReadAllText("../BookStore.Repository/Data/DataSeeds/BookGenres.json");
                var Genres = JsonSerializer.Deserialize<List<BookGenre>>(GenreData);

                if (Genres?.Count > 0)
                {

                    foreach (var Genre in Genres)
                    {
                        if (string.IsNullOrWhiteSpace(Genre.Name))
                        {
                            throw new ArgumentException("Genre name cannot be null or empty");
                        }
                        await dbContext.Set<BookGenre>().AddAsync(Genre);
                    }
                }
                await dbContext.SaveChangesAsync();
            }
            if (!dbContext.BookFormats.Any())
            {
                var FormatData = File.ReadAllText("../BookStore.Repository/Data/DataSeeds/BookFormats.json");
                var Formats = JsonSerializer.Deserialize<List<BookFormat>>(FormatData);
                if (Formats?.Count > 0)
                {
                    foreach (var format in Formats)
                        await dbContext.Set<BookFormat>().AddAsync(format);
                }
                await dbContext.SaveChangesAsync();
            }
            if (!dbContext.BookAuthors.Any())
            {
                var AuthorData = File.ReadAllText("../BookStore.Repository/Data/DataSeeds/BookAuthors.json");
                var Authors = JsonSerializer.Deserialize<List<BookAuthor>>(AuthorData);
                if (Authors?.Count > 0)
                {
                    foreach (var author in Authors)
                        await dbContext.Set<BookAuthor>().AddAsync(author);
                }
                await dbContext.SaveChangesAsync();
            }
            if (!dbContext.Books.Any())
            {
                var BookData = File.ReadAllText("../BookStore.Repository/Data/DataSeeds/Books.json");
                var Books = JsonSerializer.Deserialize<List<Book>>(BookData);
                if (Books?.Count > 0)
                {
                    foreach (var book in Books)
                        await dbContext.Set<Book>().AddAsync(book);
                }
                await dbContext.SaveChangesAsync();
            }
           
        }
    }
}
