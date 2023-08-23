using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace WebAPIServer.ModelReqRes;

public class LoadMailboxRequest : BaseRequest
{

}

public class LoadMailboxResponse : BaseResponse
{
    public ArrayList mails = new ArrayList();
}
