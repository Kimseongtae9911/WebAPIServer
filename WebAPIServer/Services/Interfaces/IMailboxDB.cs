using System.Collections;

namespace WebAPIServer.Services.Interfaces;

public interface IMailboxDB
{
    public Task<Tuple<ErrorCode, ArrayList>> LoadMailbox(string id);
}
