using System.Collections.Generic;
using System.Data;
using SqlKata.Execution;
using MySqlConnector;
using Microsoft.Extensions.Options;

public class DbConfig
{
    public string AccountDB { get; set; } = string.Empty;
    public string GameDB { get; set; } = string.Empty;

    public string MemoryDB { get; set; } = string.Empty;
}

namespace WebAPIServer.Services
{
    public class GameDB : IGameDB
    {
        readonly IOptions<DbConfig> _dbConfig;

        IDbConnection _dbConnection;
        SqlKata.Compilers.MySqlCompiler _compiler;
        QueryFactory m_queryFactory;

        public GameDB(IOptions<DbConfig> dbConfig)
        {
            _dbConfig = dbConfig;

            _dbConnection = new MySqlConnection(_dbConfig.Value.GameDB);
            _dbConnection.Open();

            _compiler = new SqlKata.Compilers.MySqlCompiler();
            m_queryFactory = new QueryFactory(_dbConnection, _compiler);
        }
        ~GameDB()
        {
            Dispose();
        }
        public void Dispose()
        {
            _dbConnection.Close();
        }

    }
}
