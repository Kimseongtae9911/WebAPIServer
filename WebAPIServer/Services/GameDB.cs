using System.Collections.Generic;
using System.Data;
using SqlKata.Execution;
using MySqlConnector;
using Microsoft.Extensions.Options;

public class DbConfig
{
    public String AccountDB { get; set; }
    public String GameDB { get; set; }
}

namespace WebAPIServer.Services
{
    public class GameDB : IGameDB
    {
        readonly IOptions<DbConfig> m_dbConfig;

        IDbConnection m_dbConnection;
        SqlKata.Compilers.MySqlCompiler m_compiler;
        QueryFactory m_queryFactory;

        public GameDB(IOptions<DbConfig> dbConfig)
        {
            m_dbConfig = dbConfig;

            m_dbConnection = new MySqlConnection(m_dbConfig.Value.GameDB);
            m_dbConnection.Open();

            m_compiler = new SqlKata.Compilers.MySqlCompiler();
            m_queryFactory = new QueryFactory(m_dbConnection, m_compiler);
        }
        ~GameDB()
        {
            Dispose();
        }
        public void Dispose()
        {
            m_dbConnection.Close();
        }

    }
}
