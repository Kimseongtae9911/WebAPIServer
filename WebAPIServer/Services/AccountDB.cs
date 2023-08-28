using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata.Execution;
using System.Data;
using System.Security.Principal;
using WebAPIServer.TableModel;
using WebAPIServer.Services.Interfaces;

namespace WebAPIServer.Services;

public class AccountDB : BaseMySqlDB, IAccountDB
{ 
    public AccountDB(IOptions<DbConfig> dbConfig) : base(dbConfig)
    {
    }

    public async Task<ErrorCode> CreateAccount(string id, string password)
    {
        try
        {
            var saltValue = Security.GetSaltString();
            var hashedPassword = Security.HashPassword(saltValue, password);

            var count = await _queryFactory.Query("account")
                .InsertAsync(new
                {
                    ID = id,
                    SaltValue = saltValue,
                    Password = hashedPassword
                });

            if (count != 1)
            {
                return ErrorCode.CreateAccountFail;
            }

            Console.WriteLine($"[CreateAccount] ID: {id}, SaltValue: {saltValue}, Password: {hashedPassword}");
            return ErrorCode.None;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error Msg: " + e.Message + ", ");
            Console.WriteLine($"[AccountDB.CreateAccount] ErrorCode: {nameof(ErrorCode.CreateAccountFailException)}, ID: {id}");
            return ErrorCode.CreateAccountFailException;
        }
    }

    public async Task<ErrorCode> Login(string id, string password)
    {
        try
        {
            var account = await _queryFactory.Query("account")
                .Where("ID", id)
                .FirstOrDefaultAsync<UserAccount>();

            if(account.Password == String.Empty)
            {
                Console.WriteLine($"[AccountDB.Login] ErrorCode: {nameof(ErrorCode.LoginFailNoAccount)}, ID: {id}");
                return ErrorCode.LoginFailNoAccount;
            }

            if(account.Password.Equals(Security.HashPassword(account.SaltValue, password)))
            {
                Console.WriteLine($"[Login] ID: {id}, Password: {nameof(account.Password)}");
                return ErrorCode.None;
            }
            else
            {
                Console.WriteLine($"[AccountDB.Login] ErrorCode: {nameof(ErrorCode.LoginFailWrongPassword)}, ID: {id}");
                return ErrorCode.LoginFailWrongPassword;
            }
        }
        catch(Exception e)
        {
            Console.WriteLine("Error Msg: " + e.Message + ", ");
            Console.WriteLine($"[AccountDB.Login] ErrorCode: {nameof(ErrorCode.LoginFailException)}, ID: {id}");
            return ErrorCode.LoginFailException;
        }
    }
}
