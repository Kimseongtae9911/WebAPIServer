using System.Collections.Generic;
using System.Data;
using SqlKata.Execution;
using MySqlConnector;
using Microsoft.Extensions.Options;

public class DbConfig
{
    public string AccountDB { get; set; } = string.Empty;
    public string ItemDB { get; set; } = string.Empty;

    public string MemoryDB { get; set; } = string.Empty;
}

namespace WebAPIServer.Services
{
    public class ItemDB : IItemDB
    {
        readonly IOptions<DbConfig> _dbConfig;

        IDbConnection _dbConnection;
        SqlKata.Compilers.MySqlCompiler _compiler;
        QueryFactory _queryFactory;

        public ItemDB(IOptions<DbConfig> dbConfig)
        {
            _dbConfig = dbConfig;

            _dbConnection = new MySqlConnection(_dbConfig.Value.ItemDB);
            _dbConnection.Open();

            _compiler = new SqlKata.Compilers.MySqlCompiler();
            _queryFactory = new QueryFactory(_dbConnection, _compiler);
        }
        ~ItemDB()
        {
            Dispose();
        }
        public void Dispose()
        {
            _dbConnection.Close();
        }

        public async Task<ErrorCode> InsertItem(string id, Int16 itemCode)
        {
            try
            {
                var existingRecord = await _queryFactory.Query("item")
                    .Where("ID", id)
                    .Where("ItemCode", itemCode)
                    .FirstOrDefaultAsync();

                if (existingRecord != null)
                {
                    await _queryFactory.Query("item")
                        .Where("ID", id)
                        .Where("ItemCode", itemCode)
                        .IncrementAsync("Count", 1);
                }
                else
                {
                    await _queryFactory.Query("item").InsertAsync(new
                    {
                        id = id,
                        itemCode = itemCode,
                        count = 1
                    });
                }
                Console.WriteLine($"[InsertItem] ID: {id}, ItemCode: {itemCode}");
                return ErrorCode.None;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Msg: " + e.Message + ", ");
                Console.WriteLine($"[ItemDB.InsertItem] ErrorCode: {nameof(ErrorCode.InsertItemFailException)}, ID: {id}, ItemCode: {itemCode}");
                return ErrorCode.InsertItemFailException;
            }

            
        }
    }
}
