using SearchApp.Model.User;

namespace SearchApp.Data.Interface
{
    public interface IUserRepository
    {
        Task<UserDTO> ValidateUser(UserCredential userCredential);
        Task<int> RegisterUser(User user);
    }
}
