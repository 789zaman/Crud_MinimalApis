using System.Data;
using System.Threading.Tasks;

namespace library.api.Data
{
    public interface IDBConnectionFactory
    {
        Task<IDbConnection> CreateConnectionAsync();
    }
}
