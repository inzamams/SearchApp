using SearchApp.Model.Search;

namespace SearchApp.Business.Interface
{
    public interface ISearchManager
    {
        Task<List<Flight>> GetFlightDetails(Filters filters, int userId);
        Task<List<Flight>> GetUserSearchHistory(int userId);
    }
}
