using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace library.api.Data
{
    public class SQLiteConnectionFactory : IDBConnectionFactory
    {
        private readonly string _connectionString;
        public SQLiteConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<IDbConnection> CreateConnectionAsync()
        {
            var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}