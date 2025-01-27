using Dapper;
using library.api.Data;
using library.api.Models;

namespace library.api.Services
{


    public class BookService : IBookService
    {
        private readonly IDBConnectionFactory _connectionFactory;
        public BookService(IDBConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<bool> CreateAsync(Book book)
        {
            var existingbook = await GetByIsbnAsync(book.Isbn);
            if (existingbook != null)
            {
                return false;
            }

            using var connection = await _connectionFactory.CreateConnectionAsync();
            var result = await connection.ExecuteAsync(@"INSERT INTO Books (Isbn,Title,Author,ShortDescription,PageCount,ReleaseDate)
                VALUES (@Isbn,@Title,@Author,@ShortDescription,@PageCount,@ReleaseDate)", book);

            return result > 0;
        }

        public async Task<bool> DeleteAsync(string isbn)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var result = await connection.ExecuteAsync(@"DELETE FROM BOOKS WHERE Isbn = @Isbn", new { Isbn = isbn });
            return result > 0;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<Book>("SELECT * FROM BOOKS");

        }

        public async Task<Book?> GetByIsbnAsync(string isbn)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return connection.QuerySingleOrDefault<Book>("SELECT * FROM BOOKS WHERE Isbn = @Isbn LIMIT 1", new { Isbn = isbn });


        }

        public async Task<IEnumerable<Book>> SearchByTitleAsync(string searchTerm)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryAsync<Book>("SELECT * FROM BOOKS WHERE Title LIKE '%' || @SearchTerm || '%' ", new { SearchTerm = searchTerm });
        }

        public async Task<bool> UpdateAsync(Book book)
        {
            var existingbook = await GetByIsbnAsync(book.Isbn);
            if (existingbook is null)
            {
                return false;
            }


            using var connection = await _connectionFactory.CreateConnectionAsync();
            var result = await connection.ExecuteAsync(@"UPDATE BOOKS SET Title = @Title, Author = @Author, ShortDescription = @ShortDescription, 
                    PageCount = @PageCount, ReleaseDate = @ReleaseDate WHERE Isbn = @Isbn", book);
            return result > 0;
        }
    }
}
