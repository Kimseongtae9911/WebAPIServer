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
        response.Mails = mails;

        return response;
    }

    [HttpPost("send")]
    public async Task<SendMailResponse> SendMail(SendMailRequest request)
    {
        var response = new SendMailResponse();

        response.Result = await _mailboxDB.SendMail(request.Sender, request.Receiver, request.MailType, request.MailDetail);

        return response;
    }

    [HttpPost("recv")]
    public async Task<RecvMailResponse> RecvMail(RecvMailRequest request)
    {
        var response = new RecvMailResponse();

        (var errorCode, var mail) = await _mailboxDB.RecvMail(request.ID, request.MailNum);

        //Logic for received mail item process

        response.Result = errorCode;
        response.Mail = mail;

        return response;
    }

    [HttpPost("recvAll")]
    public async Task<RecvAllMailResponse> RecvAllMail(RecvAllMailRequest request)
    {
        var response = new RecvAllMailResponse();

        (var errorCode, var mails) = await _mailboxDB.RecvAllMail(request.ID);

        //Logic for received mail item process

        response.Result = errorCode;
        response.Mails = mails;

        return response;
    }

    [HttpPost("delete")]
    public async Task<DeleteRecvMailResponse> DeleteRecvMail(DeleteRecvMailRequest request)
    {
        var response = new DeleteRecvMailResponse();

        (var errorCode, var mails) = await _mailboxDB.DeleteRecvMail(request.ID);

        response.Result = errorCode;
        response.Mails = mails;

        return response;
    }

    [HttpPost("see")]
    public async Task<SeeUnRecvMailResponse> SeeUnRecvMail(SeeUnRecvMailRequest request)
    {
        var response = new SeeUnRecvMailResponse();

        (var errorCode, var mails) = await _mailboxDB.SeeUnRecvMail(request.ID);

        response.Result = errorCode;
        response.Mails = mails;

        return response;
    }

    [HttpPost("organize")]
    public async Task<OrganizeMailResponse> OrganizeMail(OrganizeMailRequest request)
    {
        var response = new OrganizeMailResponse();

        (var errorCode, var mails) = await _mailboxDB.OrganizeMail(request.ID, request.IsAscending);

        response.Result = errorCode;
        response.Mails = mails;

        return response;
    }
}