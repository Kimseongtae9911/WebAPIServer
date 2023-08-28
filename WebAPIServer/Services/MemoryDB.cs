using CloudStructures;
using CloudStructures.Structures;
using Microsoft.Extensions.Options;
using WebAPIServer.Constants;
using WebAPIServer.Services.Interfaces;

namespace WebAPIServer.Services;

public class MemoryDB : IMemoryDB
{
    readonly IOptions<DbConfig> _dbConfig;
    readonly RedisConnection _redisConnection;

    readonly TimeSpan _authTokenExpireDay = TimeSpan.FromDays(1);
    readonly TimeSpan _lockExpireSecond = TimeSpan.FromSeconds(10);

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
            var redis = new RedisString<string>(_redisConnection, id, _authTokenExpireDay);

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

    public async Task<(ErrorCode, string)> GetAuthToken(string id)
    {
        try
        {
            var redis = new RedisString<string>(_redisConnection, id, null);

            var authToken = await redis.GetAsync();

            if (authToken.HasValue)
            {
                return (ErrorCode.None, authToken.Value);
            }
            else
            {
                return (ErrorCode.NoExistingAuthToken, string.Empty);
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Msg: " + ex.Message + ", ");
            Console.WriteLine($"[MemoryDB.GetAuthToken] ErrorCode: {nameof(ErrorCode.VerifyFailException)}, ID: {id}");
            return (ErrorCode.VerifyFailException, string.Empty);
        }
    }

    public async Task<ErrorCode> LockUserRequest(string id, string authToken)
    {
        try
        {
            var redis = new RedisString<string>(_redisConnection, id + authToken, _lockExpireSecond);

            if (false == await redis.SetAsync(id, _lockExpireSecond, StackExchange.Redis.When.NotExists))
            {
                Console.WriteLine($"[MemoryDB.LockUserRequest] ErrorCode: {nameof(ErrorCode.LockUserRequestFail)}, ID: {id}, AuthToken: {authToken}");
                return ErrorCode.LockUserRequestFail;
            }
            return ErrorCode.None;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Msg: " + ex.Message + ", ");
            Console.WriteLine($"[MemoryDB.LockUserRequest] ErrorCode: {nameof(ErrorCode.LockUserRequestFailException)}, ID: {id}, AuthToken: {authToken}");
            return ErrorCode.LockUserRequestFailException;
        }
    }

    public async Task<ErrorCode> UnlockUserRequest(string id, string authToken)
    {
        try
        {
            var redis = new RedisString<string>(_redisConnection, id + authToken, null);

            if(false == await redis.DeleteAsync())
            {
                Console.WriteLine($"[MemoryDB.UnlockUserRequest] ErrorCode: {nameof(ErrorCode.UnlockUserRequestFail)}, ID: {id}, AuthToken: {authToken}");
                return ErrorCode.UnlockUserRequestFail;
            }
            return ErrorCode.None;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Msg: " + ex.Message + ", ");
            Console.WriteLine($"[MemoryDB.UnlockUserRequest] ErrorCode: {nameof(ErrorCode.UnlockUserRequestFailException)}, ID: {id}, AuthToken: {authToken}");
            return ErrorCode.UnlockUserRequestFailException;
        }
    }
}
