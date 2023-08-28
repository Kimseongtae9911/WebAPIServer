using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata.Compilers;
using SqlKata.Execution;
using System.Data;

namespace WebAPIServer.Services;
public class DbConfig
{
    public string AccountDB { get; set; } = string.Empty;
    public string ItemDB { get; set; } = string.Empty;
    public string MailboxDB { get; set; } = string.Empty;
    public string MemoryDB { get; set; } = string.Empty;
}

public class BaseMySqlDB
{
    protected readonly IOptions<DbConfig> _dbConfig;
    protected readonly SqlKata.Compilers.MySqlCompiler _compiler;
    protected readonly IDbConnection _dbConnection;
    protected readonly QueryFactory _queryFactory;

    public BaseMySqlDB(IOptions<DbConfig> dbConfig)
    {
        _dbConfig = dbConfig;

        _dbConnection = new MySqlConnection(_dbConfig.Value.MailboxDB);
        _dbConnection.Open();

        _compiler = new SqlKata.Compilers.MySqlCompiler();
        _queryFactory = new QueryFactory(_dbConnection, _compiler);
    }

    public void Dispose()
    {
        _dbConnection.Close();
    }
}
