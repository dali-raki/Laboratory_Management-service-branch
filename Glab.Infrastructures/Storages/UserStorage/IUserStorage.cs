using GLAB.Domains.Models.Users;

namespace Glab.Infrastructures.Storages.UserStorage;

public interface IUserStorage
{
    ValueTask<User?> SelectUserById(string userId);

    ValueTask<User> SelectUserByUserName(string userName);

    ValueTask<string> SelectUserPassword(string userId);

    ValueTask<bool> InsertUser(User user);


}