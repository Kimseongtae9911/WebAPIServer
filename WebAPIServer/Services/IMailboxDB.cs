using System.Collections;

namespace WebAPIServer.Services;

public interface IMailboxDB
{
    public Task<Tuple<ErrorCode, ArrayList>> LoadMailbox(string id);
}
