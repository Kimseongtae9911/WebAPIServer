using System.Collections.Generic;
using System.Data;
using SqlKata.Execution;
using MySqlConnector;
using Microsoft.Extensions.Options;
using Dapper;
using SqlKata;
using System.Collections;
using WebAPIServer.Services.Interfaces;

namespace WebAPIServer.Services;

public class DbConfig
{
    public string AccountDB { get; set; } = string.Empty;
    public string ItemDB { get; set; } = string.Empty;
    public string MailboxDB { get; set; } = string.Empty;
    public string MemoryDB { get; set; } = string.Empty;
}

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
            var compiledQuery = _compiler.Compile(_queryFactory.Query("item").AsInsert(new { ID = id, ItemCode = itemCode, Count = 1 }));
            var onDuplicatedKeySql = compiledQuery.Sql + " ON DUPLICATE KEY UPDATE Count=Count+1";

            await _dbConnection.ExecuteAsync(onDuplicatedKeySql, compiledQuery.NamedBindings);

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

    public async Task<Tuple<ErrorCode, List<Tuple<Int16, Int16>>>> LoadItem(string id)
    {
        var list = new List<Tuple<Int16, Int16>>();

        try
        {
            var items = await _queryFactory.Query("item")
                       .Where("ID", id)
                       .GetAsync();

            foreach (var item in items)
            {
                list.Add(new (item.ItemCode, item.Count));
            }

            Console.WriteLine($"[LoadItem] ID: {id}");
            return new(ErrorCode.None, list);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error Msg: " + e.Message + ", ");
            Console.WriteLine($"[ItemDB.LoadItem] ErrorCode: {nameof(ErrorCode.LoadMailboxException)}, ID: {id}");
            return new (ErrorCode.LoadMailboxException, list);
        }

    }
}
