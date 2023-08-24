using Microsoft.AspNetCore.Mvc;
using WebAPIServer.ModelReqRes;
using WebAPIServer.Services.Interfaces;

namespace WebAPIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class MailboxController
{
    readonly IMailboxDB _mailboxDB;

    public MailboxController(IMailboxDB mailboxDB)
    {
        _mailboxDB = mailboxDB;
    }

    [HttpPost("load")]
    public async Task<LoadMailboxResponse> LoadMailbox(LoadMailboxRequest request)
    {
        var response = new LoadMailboxResponse();

        (var errorCode, var mails) = await _mailboxDB.LoadMailbox(request.ID);

        response.Result = errorCode;
        response.mails = mails;

        return response;
    }

    [HttpPost("send")]
    public async Task<SendMailResponse> SendMail(SendMailRequest request)
    {
        var response = new SendMailResponse();


        return response;
    }

    [HttpPost("recv")]
    public async Task<RecvMailResponse> RecvMail(RecvMailRequest request)
    {
        var response = new RecvMailResponse();


        return response;
    }

    [HttpPost("recvAll")]
    public async Task<RecvAllMailResponse> RecvAllMail(RecvAllMailRequest request)
    {
        var response = new RecvAllMailResponse();


        return response;
    }

    [HttpPost("delete")]
    public async Task<DeleteRecvMailResponse> DeleteRecvMail(DeleteRecvMailRequest request)
    {
        var response = new DeleteRecvMailResponse();


        return response;
    }

    [HttpPost("see")]
    public async Task<DeleteRecvMailResponse> SeeUnRecvMail(DeleteRecvMailRequest request)
    {
        var response = new DeleteRecvMailResponse();


        return response;
    }

    [HttpPost("organize")]
    public async Task<DeleteRecvMailResponse> OrganizeMail(DeleteRecvMailRequest request)
    {
        var response = new DeleteRecvMailResponse();


        return response;
    }
}


//우편함 로딩, 받기, 받기 완료 삭제, 모두 받기, 미수령만 보기, 정렬
