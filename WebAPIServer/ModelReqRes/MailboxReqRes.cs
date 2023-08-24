using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace WebAPIServer.ModelReqRes;

//Load
public class LoadMailboxRequest : BaseRequest
{

}

public class LoadMailboxResponse : BaseResponse
{
    public ArrayList mails = new ArrayList();
}

//Send
public class SendMailRequest : BaseRequest
{

}

public class SendMailResponse : BaseResponse
{

}

//Recv
public class RecvMailRequest : BaseRequest
{

}

public class RecvMailResponse : BaseResponse
{

}

//RecvAll
public class RecvAllMailRequest : BaseRequest
{

}

public class RecvAllMailResponse : BaseResponse
{

}

//DeleteRecv
public class DeleteRecvMailRequest : BaseRequest
{

}

public class DeleteRecvMailResponse : BaseResponse
{

}

//SeeUnRecv
public class SeeUnRecvMailRequest : BaseRequest
{

}

public class SeeUnRecvMailResponse : BaseResponse
{

}

//Organize
public class OrganizeMailRequest : BaseRequest
{

}

public class OrganizeMailResponse : BaseResponse
{

}