using Glab.App.Accounts;
using Glab.App.Users;
using GLAB.Domains.Models.Users;


namespace GLAB.Implementation.Services.Accounts;

public class AccountService : IAccount
{
    private readonly IUserService userService;

    public AccountService(IUserService userService)
    {
        this.userService = userService;
        
    }

    public async Task<(IAccount.LoginStatus,User)> CheckCredentials(string username, string password)
    {
       
        
      var user= await userService.GetUserByUserName(username);

      if (user == null || user.State == UserState.Deleted)
          return (IAccount.LoginStatus.UserNotExists,null);

      if (user.State == UserState.Bloqued)
          return (IAccount.LoginStatus.UserBlocked,null);


      bool isPasswordCorrect = await userService.ValidatePassword(user.UserId, password);

      if (isPasswordCorrect)
      {
          return (IAccount.LoginStatus.CanLogin,user);  
      }

      return (IAccount.LoginStatus.WrongCredentials,null);
      
      
    }

    public async Task<bool> ChangePassword(string userid, string oldpassword, string newpassword)
    {
        throw new NotImplementedException();
    }
}