using SearchApp.Model.Search;

namespace SearchApp.Data.Interface
{
    public interface ISearchRepository
    {
        Task<List<Flight>> GetFlightDetails(Filters filters, int userId);
        Task<List<Flight>> GetUserSearchHistory(int userId);
    }
}
