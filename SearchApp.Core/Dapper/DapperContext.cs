using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SearchApp.Core.Common;
using System.Data;

namespace SearchApp.Core.Dapper
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString(Constants.DapperConstants.Connectionstring);
        }
        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);
    }
}
