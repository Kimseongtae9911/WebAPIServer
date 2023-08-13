using CloudStructures;
using CloudStructures.Structures;
using Microsoft.Extensions.Options;

namespace WebAPIServer.Services;

public class MemoryDB : IMemoryDB
{
    readonly IOptions<DbConfig> _dbConfig;
    RedisConnection _redisConnection;

    public MemoryDB(IOptions<DbConfig> dbConfig)
    {
        _dbConfig = dbConfig;
        var config = new RedisConfig("default", "127.0.0.1");
        _redisConnection = new RedisConnection(config);
    }

    public async Task<ErrorCode> RegisterUser(string id, string authToken)
    {
        try
        {
            var redis = new RedisString<string>(_redisConnection, id, TimeSpan.FromDays(Constants.AuthTokenDay));

            await redis.SetAsync(authToken);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error Msg: " + e.Message + ", ");
            Console.WriteLine($"[MemoryDB.RegisterUser] ErrorCode: {nameof(ErrorCode.CreateAccountFailException)}, ID: {id}");
            return ErrorCode.LoginFailRegisterRedis;
        }

        return ErrorCode.None;
    }
}
