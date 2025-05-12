using BookTest.Data;
using BookTest.Models;
using Microsoft.EntityFrameworkCore;

namespace BookTest.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _context;
        public HomeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Genre>> Genres()
        {
            return await _context.Genres.ToListAsync();
        } 
        public async Task<IEnumerable<Book>> GetBooks(string sTerm ="", int? genreId = null)
        {
            sTerm = sTerm.ToLower();
            IEnumerable<Book> books = await (from book in _context.Books
                        join genre in _context.Genres on book.GenreId equals genre.Id
                        where (string.IsNullOrWhiteSpace(sTerm) ||
                        book.BookName.ToLower().Contains(sTerm) ||
                        book.AuthorName.ToLower().Contains(sTerm)) &&
                        (!genreId.HasValue || book.GenreId == genreId.Value)
                        orderby book.BookName
                        select new Book
                        {
                            Id = book.Id,
                            BookName = book.BookName,
                            AuthorName = book.AuthorName,
                            Price = book.Price,
                            Image = book.Image,
                            GenreId = book.GenreId,
                            GenreName = genre.GenreName
                        }
                        ).ToListAsync();
            return books;
        }
    }
}
