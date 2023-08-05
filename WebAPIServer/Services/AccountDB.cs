using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata.Execution;
using System.Data;

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

                Console.WriteLine($"[CreateAccount] ID: {id}, Password: {password}");
                return ErrorCode.None;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Msg: " + e.Message + ", ");
                Console.WriteLine($"[AccountDb.CreateAccount] ErrorCode: {ErrorCode.SignUpFailException}, ID: {id}");
                return ErrorCode.SignUpFailException;
            }
        }
    }
}
