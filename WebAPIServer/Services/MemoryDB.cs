using CloudStructures;
using CloudStructures.Structures;
using Microsoft.Extensions.Options;
using WebAPIServer.Constants;
using WebAPIServer.Services.Interfaces;

namespace WebAPIServer.Services;

public class MemoryDB : IMemoryDB
{
    readonly IOptions<DbConfig> _dbConfig;
    RedisConnection _redisConnection;

    public MemoryDB(IOptions<DbConfig> dbConfig)
    {
        _dbConfig = dbConfig;        
        var config = new RedisConfig("default", _dbConfig.Value.MemoryDB);
        _redisConnection = new RedisConnection(config);
    }

    public async Task<ErrorCode> RegisterUser(string id, string authToken)
    {
        try
        {
            var redis = new RedisString<string>(_redisConnection, id, TimeSpan.FromDays(AccountConstants.AuthTokenDay));

            await redis.SetAsync(authToken);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error Msg: " + e.Message + ", ");
            Console.WriteLine($"[MemoryDB.RegisterUser] ErrorCode: {nameof(ErrorCode.CreateAccountFailException)}, ID: {id}");
            return ErrorCode.LoginFailRegisterRedis;
        }

        Console.WriteLine($"[RegisterUser] ID: {id}, AuthToken: {authToken}");
        return ErrorCode.None;
    }

    public async Task<Tuple<ErrorCode, string>> GetAuthToken(string id)
    {
        try
        {
            var redis = new RedisString<string>(_redisConnection, id, null);

            var authToken = await redis.GetAsync();

            if (authToken.HasValue)
            {
                return new Tuple<ErrorCode, string>(ErrorCode.None, authToken.Value);
            }
            else
            {
                return new Tuple<ErrorCode, string>(ErrorCode.None, "");
            }
            
        }
        catch (Exception e)
        {
            Console.WriteLine("Error Msg: " + e.Message + ", ");
            Console.WriteLine($"[MemoryDB.GetAuthToken] ErrorCode: {nameof(ErrorCode.NoExistingAuthToken)}, ID: {id}");
            return new Tuple<ErrorCode, string>(ErrorCode.NoExistingAuthToken, "");
        }
    }
}
