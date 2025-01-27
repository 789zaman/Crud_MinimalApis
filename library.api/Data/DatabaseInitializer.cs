using Dapper;

namespace library.api.Data
{
    public class DatabaseInitializer
    {
        private readonly IDBConnectionFactory _connectionFactory;
        public DatabaseInitializer(IDBConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task InitializeAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(@"CREATE TABLE IF NOT EXISTS BOOKS(
             Isbn TEXT PRIMARY KEY,
            Title TEXT NOT NULL,
            Author TEXT NOT NULL,
            ShortDescription TEXT NOT NULL,
            PageCount INTEGER,
            ReleaseDate TEXT NOT NULL

)"
);
        }

    }
}