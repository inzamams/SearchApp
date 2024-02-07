using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace SearchApp.Core.Dapper
{
    public class DapperManager : IDapperManager
    {
        private readonly IConfiguration _configuration;
        private readonly DapperContext _context;

        public DapperManager(IConfiguration configuration, DapperContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public void Dispose()
        {
        }

        public async Task<T> GetAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<T>(sp, parms, commandType: commandType);
        }

        public async Task<List<T>> GetAllAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using var connection = _context.CreateConnection();
            return (await connection.QueryAsync<T>(sp, parms, commandType: commandType)).ToList();
        }

        public async Task<T> InsertAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleAsync<T>(sp, parms, commandType: commandType);
        }
    }
}
