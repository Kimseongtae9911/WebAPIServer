namespace WebAPIServer.Services
{
    public interface IAccountDB
    {
        public Task<ErrorCode> ClientSignUp(string id, string password);
    }
}
