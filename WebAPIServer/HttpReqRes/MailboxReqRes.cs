using WebAPIServer.TableModel;

namespace WebAPIServer.ModelReqRes;

//Load
public class LoadMailboxRequest : BaseRequest
{

}

public class LoadMailboxResponse : BaseResponse
{
    public List<ItemInfo> Mails = new List<ItemInfo>();
}

//Send
public class SendMailRequest : BaseRequest
{
    public string Sender = String.Empty;
    public string Receiver = String.Empty;
    public Int16 MailType;
    public Int16 MailDetail;
}

public class SendMailResponse : BaseResponse
{

}

//Recv
public class RecvMailRequest : BaseRequest
{
    public Int16 MailNum;
}

public class RecvMailResponse : BaseResponse
{
    public ItemInfo Mail = new ItemInfo(-1, -1);
}

//RecvAll
public class RecvAllMailRequest : BaseRequest
{

}

public class RecvAllMailResponse : BaseResponse
{
    public List<ItemInfo> Mails = new List<ItemInfo>();
}

//DeleteRecv
public class DeleteRecvMailRequest : BaseRequest
{

}

public class DeleteRecvMailResponse : BaseResponse
{
    public List<ItemInfo> Mails = new List<ItemInfo>();
}

//SeeUnRecv
public class SeeUnRecvMailRequest : BaseRequest
{

}

public class SeeUnRecvMailResponse : BaseResponse
{
    public List<ItemInfo> Mails = new List<ItemInfo>();
}

//Organize
public class OrganizeMailRequest : BaseRequest
{
    public bool IsAscending;
}

public class OrganizeMailResponse : BaseResponse
{
    public List<ItemInfo> Mails = new List<ItemInfo>();
}