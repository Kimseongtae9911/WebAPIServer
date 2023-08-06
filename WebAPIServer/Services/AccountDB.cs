using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata.Execution;
using System.Data;
using System.Security.Principal;

namespace WebAPIServer.Services
{
    public class AccountDB : IAccountDB
    {
        readonly IOptions<DbConfig> m_dbConfig;

        IDbConnection m_dbConnection;
        SqlKata.Compilers.MySqlCompiler m_compiler;
        QueryFactory m_queryFactory;

        public AccountDB(IOptions<DbConfig> dbConfig)
        {
            m_dbConfig = dbConfig;

            m_dbConnection = new MySqlConnection(m_dbConfig.Value.AccountDB);
            m_dbConnection.Open();

            m_compiler = new SqlKata.Compilers.MySqlCompiler();
            m_queryFactory = new QueryFactory(m_dbConnection, m_compiler);
        }
        ~AccountDB()
        {
            Dispose();
        }
        public void Dispose()
        {
            m_dbConnection.Close();
        }

        public async Task<ErrorCode> ClientSignUp(string id, string password)
        {
            try
            {
                var count = await m_queryFactory.Query("account").InsertAsync(new
                {
                    ID = id,
                    Password = password
                });

                if (count != 1)
                {
                    return ErrorCode.SignUpFail;
                }

                Console.WriteLine($"[ClientSignUp] ID: {id}, Password: {password}");
                return ErrorCode.None;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Msg: " + e.Message + ", ");
                Console.WriteLine($"[AccountDB.ClientSignUp] ErrorCode: {ErrorCode.SignUpFailException}, ID: {id}");
                return ErrorCode.SignUpFailException;
            }
        }

        public async Task<ErrorCode> Login(string id, string password)
        {
            try
            {
                var account = await m_queryFactory.Query("account")
                    .Where("ID", id)
                    .FirstOrDefaultAsync();

                if(account == null)
                {
                    Console.WriteLine($"[AccountDB.Login] ErrorCode: {ErrorCode.LoginFailNoAccount}, ID: {id}");
                    return ErrorCode.LoginFailNoAccount;
                }

                if(password.Equals(account.Password))
                {
                    Console.WriteLine($"[Login] ID: {id}, Password: {password}");
                    return ErrorCode.None;
                }
                else
                {
                    Console.WriteLine($"[AccountDB.Login] ErrorCode: {ErrorCode.LoginFailWrongPassword}, ID: {id}");
                    return ErrorCode.LoginFailWrongPassword;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Error Msg: " + e.Message + ", ");
                Console.WriteLine($"[AccountDB.Login] ErrorCode: {ErrorCode.LoginFailException}, ID: {id}");
                return ErrorCode.LoginFailException;
            }
        }
    }
}
