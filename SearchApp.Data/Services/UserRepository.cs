using Dapper;
using SearchApp.Core.Common;
using SearchApp.Core.Dapper;
using SearchApp.Data.Interface;
using SearchApp.Model.User;
using System.Data;

namespace SearchApp.Data.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly IDapperManager _dapperManager;

        public UserRepository(IDapperManager dapperManager)
        {
            _dapperManager = dapperManager;
        }

        public async Task<int> RegisterUser(User user)
        {
            var userParams = new DynamicParameters();
            userParams.Add("Email", user.Email, DbType.String, ParameterDirection.Input);
            userParams.Add("Password", user.Password, DbType.String, ParameterDirection.Input);
            userParams.Add("Address", user.Address, DbType.String, ParameterDirection.Input);

            return await _dapperManager.InsertAsync<int>(Constants.SqlConstants.Sp_InsertUser, userParams);


        }

        public async Task<UserDTO> ValidateUser(UserCredential userCredential)
        {
            var userParams = new DynamicParameters();
            userParams.Add("Email", userCredential.Email, DbType.String, ParameterDirection.Input);
            userParams.Add("Password", userCredential.Password, DbType.String, ParameterDirection.Input);

            return await _dapperManager.GetAsync<UserDTO>(Constants.SqlConstants.Sp_ValidateUser, userParams);
        }
    }
}
