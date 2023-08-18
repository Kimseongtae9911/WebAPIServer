using Microsoft.AspNetCore.Mvc;
using WebAPIServer.ModelReqRes;
using WebAPIServer.Services;
using WebAPIServer.Utils;

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
}


//우편함 로딩, 받기, 받기 완료 삭제, 모두 받기, 미수령만 보기, 정렬
