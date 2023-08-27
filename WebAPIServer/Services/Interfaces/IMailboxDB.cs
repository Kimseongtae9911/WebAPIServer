using WebAPIServer.TableModel;

namespace WebAPIServer.Services.Interfaces;

public interface IMailboxDB
{
    public Task<Tuple<ErrorCode, List<ItemInfo>>> LoadMailbox(string id);
    public Task<ErrorCode> SendMail(string sender, string receiver, Int16 mailType, Int16 mailDetail);
    public Task<Tuple<ErrorCode, ItemInfo>> RecvMail(string id, Int16 mailNum);
    public Task<Tuple<ErrorCode, List<ItemInfo>>> RecvAllMail(string id);
    public Task<Tuple<ErrorCode, List<ItemInfo>>> DeleteRecvMail(string id);
    public Task<Tuple<ErrorCode, List<ItemInfo>>> SeeUnRecvMail(string id);
    public Task<Tuple<ErrorCode, List<ItemInfo>>> OrganizeMail(string id, bool isAscending);
}
