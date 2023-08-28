using WebAPIServer.TableModel;

namespace WebAPIServer.Services.Interfaces;

public interface IMailboxDB
{
    public Task<(ErrorCode, List<MailboxInfo>)> LoadMailbox(string id, Int16 pageNum);
    public Task<ErrorCode> SendMail(string sender, string receiver, Int16 mailType, Int16 mailDetail);
    public Task<(ErrorCode, MailboxInfo)> RecvMail(string id, Int16 mailNum);
    public Task<(ErrorCode, List<MailboxInfo>)> RecvAllMail(string id);
    public Task<(ErrorCode, List<MailboxInfo>)> DeleteRecvMail(string id);
    public Task<(ErrorCode, List<MailboxInfo>)> SeeUnRecvMail(string id);
    public Task<(ErrorCode, List<MailboxInfo>)> OrganizeMail(string id, Int16 pageNum, bool isAscending);
    public Task<ErrorCode> UpdateMailbox(string id);
}
