using SearchApp.Business.Interface;
using SearchApp.Core.Common;
using SearchApp.Data.Interface;
using SearchApp.Model.Search;
using Serilog;
using System.Globalization;

namespace SearchApp.Business.Services
{
    public class SearchManager : ISearchManager
    {
        private readonly ISearchRepository _searchRepository;
        private readonly ILogger _logger;
        public SearchManager(ISearchRepository searchRepository, ILogger logger)
        {
            _searchRepository = searchRepository;
            _logger = logger;
        }

        public async Task<List<Flight>> GetFlightDetails(Filters filters, int userId)
        {
            DateTime dateValue;
            DateTime.TryParseExact(!string.IsNullOrEmpty(filters.Date) ? filters.Date : DateTime.Now.ToString(), Constants.DateConstants.DateFormats,
                          new CultureInfo("en-US"),
                          DateTimeStyles.None,
                          out dateValue);

            filters.Date = dateValue.ToString(Constants.DateConstants.AcceptedFormats);

            return await _searchRepository.GetFlightDetails(filters, userId);
        }

        public async Task<List<Flight>> GetUserSearchHistory(int userId)
        {
            return await _searchRepository.GetUserSearchHistory(userId);
        }
    }
}
