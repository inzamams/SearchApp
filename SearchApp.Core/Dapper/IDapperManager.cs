using Dapper;
using System.Data;

namespace SearchApp.Core.Dapper
{
    public interface IDapperManager : IDisposable
    {
        Task<T> GetAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<List<T>> GetAllAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<T> InsertAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
    }
}
