using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SearchApp.Business.Interface;
using SearchApp.Core.Common;
using SearchApp.Model.Search;
using ILogger = Serilog.ILogger;

namespace SearchApp.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchManager _searchManager;
        private readonly ILogger _logger;

        public SearchController(ISearchManager searchManager, ILogger logger)
        {
            _searchManager = searchManager;
            _logger = logger;
        }

        [HttpGet]
        [ActionName("FlightDetails")]
        public async Task<IActionResult> Search([FromQuery] Filters filters)
        {
            var userId = HttpContext.Session.GetInt32(Constants.UserConstants.UserId) ?? 0;

            if (userId == 0)
            {
                return StatusCode(
                    StatusCodes.Status401Unauthorized,
                    new
                    {
                        Message = Constants.Messages.ReLogin
                    });
            }

            var flightDetails = await _searchManager.GetFlightDetails(filters, userId);

            if (flightDetails?.Count == 0)
            {
                return Ok(Constants.Messages.FlightNotAvailable);
            }

            return Ok(flightDetails);
        }

        [HttpGet]
        [ActionName("UserSearchHistory")]
        public async Task<IActionResult> SearchHistory()
        {
            var userId = HttpContext.Session.GetInt32(Constants.UserConstants.UserId) ?? 0;

            if (userId == 0)
            {
                return StatusCode(
                    StatusCodes.Status401Unauthorized,
                    new
                    {
                        Message = Constants.Messages.ReLogin
                    });
            }
            var userHistory = await _searchManager.GetUserSearchHistory(userId);

            if (userHistory?.Count == 0)
            {
                return Ok(Constants.Messages.HistoryNotAvailable);
            }

            return Ok(userHistory);
        }
    }
}
