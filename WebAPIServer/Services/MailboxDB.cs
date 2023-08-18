using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata.Execution;
using System.Collections;
using System.Data;

namespace WebAPIServer.Services;

public class MailboxDB : IMailboxDB
{
    readonly IOptions<DbConfig> _dbConfig;

    IDbConnection _dbConnection;
    SqlKata.Compilers.MySqlCompiler _compiler;
    QueryFactory _queryFactory;

    public MailboxDB(IOptions<DbConfig> dbConfig)
    {
        _dbConfig = dbConfig;

        _dbConnection = new MySqlConnection(_dbConfig.Value.AccountDB);
        _dbConnection.Open();

        _compiler = new SqlKata.Compilers.MySqlCompiler();
        _queryFactory = new QueryFactory(_dbConnection, _compiler);
    }

    ~MailboxDB()
    {
        Dispose();
    }
    public void Dispose()
    {
        _dbConnection.Close();
    }

    public async Task<Tuple<ErrorCode, ArrayList>> LoadMailbox(string id)
    {
        var arr = new ArrayList();

        try
        {
            var mails = await _queryFactory.Query("mailbox")
                       .Where("UserID", id)
                       .GetAsync();

            arr.AddRange(mails.ToArray());

            Console.WriteLine($"[LoadMailbox] ID: {id}");
            return new Tuple<ErrorCode, ArrayList>(ErrorCode.None, arr);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error Msg: " + e.Message + ", ");
            Console.WriteLine($"[MailboxDB.LoadMailbox] ErrorCode: {nameof(ErrorCode.LoadMailboxException)}, ID: {id}");
            return new Tuple<ErrorCode, ArrayList>(ErrorCode.LoadMailboxException, arr);
        }        
    }
}
