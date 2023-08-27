using WebAPIServer.TableModel;

namespace WebAPIServer.Services.Interfaces;

public interface IMailboxDB
{
    public Task<Tuple<ErrorCode, List<MailboxInfo>>> LoadMailbox(string id);
    public Task<ErrorCode> SendMail(string sender, string receiver, Int16 mailType, Int16 mailDetail);
    public Task<Tuple<ErrorCode, MailboxInfo>> RecvMail(string id, Int16 mailNum);
    public Task<Tuple<ErrorCode, List<MailboxInfo>>> RecvAllMail(string id);
    public Task<Tuple<ErrorCode, List<MailboxInfo>>> DeleteRecvMail(string id);
    public Task<Tuple<ErrorCode, List<MailboxInfo>>> SeeUnRecvMail(string id);
    public Task<Tuple<ErrorCode, List<MailboxInfo>>> OrganizeMail(string id, bool isAscending);
    public Task<ErrorCode> UpdateMailbox(string id);
}
