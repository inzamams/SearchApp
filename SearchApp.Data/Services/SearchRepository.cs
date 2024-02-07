using Dapper;
using SearchApp.Core.Common;
using SearchApp.Core.Dapper;
using SearchApp.Data.Interface;
using SearchApp.Model.Search;
using System.Data;

namespace SearchApp.Data.Services
{
    public class SearchRepository : ISearchRepository
    {
        private readonly IDapperManager _dapperManager;

        public SearchRepository(IDapperManager dapperManager)
        {
            _dapperManager = dapperManager;
        }

        public async Task<List<Flight>> GetFlightDetails(Filters filters, int userId)
        {
            var flightParams = new DynamicParameters();
            flightParams.Add("Name", filters.Name, DbType.String, ParameterDirection.Input);
            flightParams.Add("Source", filters.Source, DbType.String, ParameterDirection.Input);
            flightParams.Add("Destination", filters.Destination, DbType.String, ParameterDirection.Input);
            flightParams.Add("PriceFrom", filters.PriceFrom, DbType.Int64, ParameterDirection.Input);
            flightParams.Add("PriceTo", filters.PriceTo, DbType.Int64, ParameterDirection.Input);
            flightParams.Add("Date", filters.Date, DbType.String, ParameterDirection.Input);
            flightParams.Add("UserId", userId, DbType.Int64, ParameterDirection.Input);

            return await _dapperManager.GetAllAsync<Flight>(Constants.SqlConstants.Sp_GetFlightDetails, flightParams);
        }

        public async Task<List<Flight>> GetUserSearchHistory(int userId)
        {
            var filterParams = new DynamicParameters();
            filterParams.Add("UserId", userId, DbType.Int64, ParameterDirection.Input);

            return await _dapperManager.GetAllAsync<Flight>(Constants.SqlConstants.Sp_GetUserSearchHistory, filterParams);
        }
    }
}
