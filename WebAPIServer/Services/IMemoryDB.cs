namespace WebAPIServer.Services;

public interface IMemoryDB
{
    public Task<ErrorCode> RegisterUser(string id, string authToken);
}
