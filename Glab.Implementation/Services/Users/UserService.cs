using GLAB.Domains.Models.Users;
using Glab.Infrastructures.Storages.UserStorage;
using Glab.App.Users;

namespace Glab.Implementation.Services.Users;

public class UserService : IUserService

{

    private readonly IUserStorage userStorage;

    private PasswordHasher passwordHasher;

    public UserService(IUserStorage userStorage)
    {
        this.userStorage = userStorage;
        passwordHasher = new PasswordHasher();
    }

    public async Task<User?> GetUserById(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<User> GetUserByUserName(string userName)
    {
        return await userStorage.SelectUserByUserName(userName);

    }

    public async Task<string> GetUserPassword(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CreateUser(User user)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ValidatePassword(string userId, string userPassword)
    {
        string password = await userStorage.SelectUserPassword(userId);

        return BCrypt.CheckPassword(userPassword, password);
    }

    public async Task<List<ApplicationRole>> getUserRoles(string userId)
    {
        return null;
    }
}