using SearchApp.Model.User;

namespace SearchApp.Business.Interface
{
    public interface IUserManager
    {
        Task<UserAccessDTO> ValidateUserAndGetAccessToken(UserCredential appUser);
        Task<UserDTO> RegisterUser(User user);
        Task<bool> ValidateRequiredParameters(string email, string password);
    }
}
