using WebAPIServer.TableModel;

namespace WebAPIServer.HttpReqRes;

//Load
public class LoadMailboxRequest : BaseRequest
{

}

public class LoadMailboxResponse : BaseResponse
{
    public List<MailboxInfo> Mails { get; set; } = new List<MailboxInfo>();
}

//Send
public class SendMailRequest : BaseRequest
{
    public string Receiver { get; set; } = String.Empty;
    public Int16 MailType { get; set; }
    public Int16 MailDetail { get; set; }
}

public class SendMailResponse : BaseResponse
{

}

//Recv
public class RecvMailRequest : BaseRequest
{
    public Int16 MailboxID { get; set; }
}

public class RecvMailResponse : BaseResponse
{
    public MailboxInfo Mail { get; set; } = new MailboxInfo(-1, -1);
}

//RecvAll
public class RecvAllMailRequest : BaseRequest
{

}

public class RecvAllMailResponse : BaseResponse
{
    public List<MailboxInfo> Mails { get; set; } = new List<MailboxInfo>();
}

//DeleteRecv
public class DeleteRecvMailRequest : BaseRequest
{

}

public class DeleteRecvMailResponse : BaseResponse
{
    public List<MailboxInfo> Mails { get; set; } = new List<MailboxInfo>();
}

//SeeUnRecv
public class SeeUnRecvMailRequest : BaseRequest
{

}

public class SeeUnRecvMailResponse : BaseResponse
{
    public List<MailboxInfo> Mails { get; set; } = new List<MailboxInfo>();
}

//Organize
public class OrganizeMailRequest : BaseRequest
{
    public bool IsAscending { get; set; }
}

public class OrganizeMailResponse : BaseResponse
{
    public List<MailboxInfo> Mails { get; set; } = new List<MailboxInfo>();
}