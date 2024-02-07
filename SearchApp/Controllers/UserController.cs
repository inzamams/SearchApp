using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SearchApp.Business.Interface;
using SearchApp.Core.Common;
using SearchApp.Model.User;
using ILogger = Serilog.ILogger;

namespace SearchApp.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IUserManager _userManager;
        private readonly IConfiguration _configuration;

        public UserController(ILogger logger, IUserManager userManager, IConfiguration configuration)
        {
            _logger = logger;
            _userManager = userManager;
            _configuration = configuration;

        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var requestParam = await _userManager.ValidateRequiredParameters(user.Email, user.Password);

            if (!requestParam)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        Message = Constants.Messages.MendatoryFields
                    });
            }

            var result = await _userManager.RegisterUser(user);

            if (result == null)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        Message = Constants.Messages.UserNotRegistered
                    });
            }

            return Ok(new
            {
                UserDetails = result,
                Message = Constants.Messages.UserRegistered
            });
        }

        [HttpPost]
        [ActionName("Login")]
        public async Task<IActionResult> LoginAndGetAccessToken([FromBody] UserCredential userCredential)
        {
            var requestParam = await _userManager.ValidateRequiredParameters(userCredential.Email, userCredential.Password);

            if (!requestParam)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        Message = Constants.Messages.MendatoryFields
                    });
            }

            var userAccessDetails = await _userManager.ValidateUserAndGetAccessToken(userCredential);

            if (userAccessDetails == null)
            {
                return Unauthorized();
            }

            HttpContext.Session.SetInt32(Constants.UserConstants.UserId, userAccessDetails.UserId);

            return Ok(new
            {
                UserDetails = userAccessDetails,
                Message = Constants.Messages.LoginSuccess
            });
        }
    }
}
