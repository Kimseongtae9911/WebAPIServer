namespace WebAPIServer.Services.Interfaces;

public interface IMemoryDB
{
    public Task<ErrorCode> RegisterUser(string id, string authToken);
    public Task<Tuple<ErrorCode, string>> GetAuthToken(string id);
}
