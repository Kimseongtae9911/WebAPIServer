namespace WebAPIServer.Services.Interfaces;

public interface IMemoryDB
{
    public Task<ErrorCode> RegisterUser(string id, string authToken);
    public Task<(ErrorCode, string)> GetAuthToken(string id);
    public Task<ErrorCode> LockUserRequest(string id, string authToken);
    public Task<ErrorCode> UnlockUserRequest(string id, string authToken);
}
