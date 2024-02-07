using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SearchApp.Business.Interface;
using SearchApp.Data.Interface;
using SearchApp.Model.User;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SearchApp.Business.Services
{
    public class UserManager : IUserManager
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public UserManager(ILogger logger, IConfiguration configuration, IUserRepository userRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _userRepository = userRepository;
        }

        public async Task<UserAccessDTO> ValidateUserAndGetAccessToken(UserCredential userCredential)
        {
            var userAccessDTO = new UserAccessDTO();
            var userDetails = await _userRepository.ValidateUser(userCredential);

            if (userDetails != null)
            {
                userAccessDTO.UserId = userDetails.Id;
                var issuer = _configuration["Jwt:Issuer"];
                var audience = _configuration["Jwt:Audience"];
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, userCredential.Email),
                new Claim(JwtRegisteredClaimNames.Email, userCredential.Email),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
            }),
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials
                    (new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                userAccessDTO.AccessToken = tokenHandler.WriteToken(token);
            }
            return userAccessDTO;
        }

        public async Task<UserDTO> RegisterUser(User user)
        {
            var userId = await _userRepository.RegisterUser(user);

            if (userId == 0)
            {
                return new UserDTO();
            }

            return new UserDTO
            {
                Id = userId,
                Email = user.Email,
                Address = user.Address
            };
        }

        public async Task<bool> ValidateRequiredParameters(string email, string password)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                return true;
            }

            return false;
        }
    }
}
