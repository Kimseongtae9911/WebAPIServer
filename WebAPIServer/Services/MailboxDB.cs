using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata.Execution;
using System.Data;
using WebAPIServer.Services.Interfaces;
using WebAPIServer.TableModel;

namespace WebAPIServer.Services;

public class MailboxDB : IMailboxDB
{
    readonly IOptions<DbConfig> _dbConfig;

    IDbConnection _dbConnection;
    SqlKata.Compilers.MySqlCompiler _compiler;
    QueryFactory _queryFactory;

    public MailboxDB(IOptions<DbConfig> dbConfig)
    {
        _dbConfig = dbConfig;

        _dbConnection = new MySqlConnection(_dbConfig.Value.MailboxDB);
        _dbConnection.Open();

        _compiler = new SqlKata.Compilers.MySqlCompiler();
        _queryFactory = new QueryFactory(_dbConnection, _compiler);
    }

    public void Dispose()
    {
        _dbConnection.Close();
    }

    public async Task<Tuple<ErrorCode, List<ItemInfo>>> LoadMailbox(string id)
    {
        var mailBox = new List<ItemInfo>();

        try
        {
            // Need to limit numver of mail
            var mails = await _queryFactory.Query("mailbox")
                       .Where("UserID", id)
                       .Where("IsDeleted", false)
                       .GetAsync();
                       //.GetAsync<List<ItemInfo>>();

            foreach(var mail in mails)
            {
                mailBox.Add(mail);
            } 

            Console.WriteLine($"[LoadMailbox] ID: {id}");
            return new (ErrorCode.None, mailBox);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error Msg: " + e.Message + ", ");
            Console.WriteLine($"[MailboxDB.LoadMailbox] ErrorCode: {nameof(ErrorCode.LoadMailboxException)}, ID: {id}");
            return new(ErrorCode.LoadMailboxException, mailBox);
        }        
    }

    public async Task<ErrorCode> SendMail(string sender, string receiver, Int16 mailType, Int16 mailDetail)
    {
        try
        {
            var mails = await _queryFactory.Query("mailbox")
                .InsertAsync(new
                {
                    Sender = sender,
                    UserID = receiver,
                    MailType = mailType,
                    MailDetail = mailDetail
                });

            Console.WriteLine($"[SendMail] Sender: {sender}, Receiver: {receiver}, MailType: {mailType}, MailDetail: {mailDetail}");
            return ErrorCode.None;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error Msg: " + e.Message + ", ");
            Console.WriteLine($"[MailboxDB.SendMail] ErrorCode: {nameof(ErrorCode.SendMailException)}, Sender: {sender}, Receiver: {receiver}, MailType: {mailType}, MailDetail: {mailDetail}");
            return ErrorCode.SendMailException;
        }
    }

    public async Task<Tuple<ErrorCode, ItemInfo>> RecvMail(string id, Int16 mailNum)
    {
        try
        {
            var recvMail = await _queryFactory.Query("mailbox")
                .Where("UserID", id)
                .Where("MailNum", mailNum)
                .Where("IsRecv", false)
                .Where("IsDelete", false)
                .FirstOrDefaultAsync();

            await _queryFactory.Query("mailbox")
                .Where("UserID", id)
                .Where("MailNum", mailNum)
                .Where("IsRecv", false)
                .Where("IsDelete", false)
                .UpdateAsync(new { IsRecv = true });

            Console.WriteLine($"[RecvMail] ID: {id}, MailNum: {mailNum}, RecvMailType: {recvMail.MailType}, RecvMailDetail: {recvMail.MailDetail}");
            return new (ErrorCode.None, new (recvMail.MailType, recvMail.MailDetail));
        }
        catch (Exception e)
        {
            Console.WriteLine("Error Msg: " + e.Message + ", ");
            Console.WriteLine($"[MailboxDB.RecvMail] ErrorCode: {nameof(ErrorCode.RecvMailException)}, ID: {id}, MailNum: {mailNum}");
            return new (ErrorCode.RecvMailException, new (-1, -1));
        }
    }

    public async Task<Tuple<ErrorCode, List<ItemInfo>>> RecvAllMail(string id)
    {
        //Need to return all mail and change IsRecv to true
        try
        {
            var recvMail = await _queryFactory.Query("mailbox")
                .Where("UserID", id)
                .Where("IsRecv", false)
                .Where("IsDelete", false)
                .GetAsync();

            Console.WriteLine($"[RecvAllMail] ID: {id}");
            return ErrorCode.None;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error Msg: " + e.Message + ", ");
            Console.WriteLine($"[MailboxDB.RecvAllMail] ErrorCode: {nameof(ErrorCode.RecvAllMailException)}, ID: {id}");
            return new(ErrorCode.RecvAllMailException, new List<ItemInfo>());
        }
    }

    public async Task<Tuple<ErrorCode, List<ItemInfo>>> DeleteRecvMail(string id)
    {
        try
        {
            var recvMails = await _queryFactory.Query("mailbox")
                .Where("UserID", id)
                .Where("IsRecv", true)
                .Where("IsDelete", false)
                .UpdateAsync(new { IsDelete = true });

            Console.WriteLine($"[DeleteRecvMail] ID: {id}");

            return await LoadMailbox(id);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error Msg: " + e.Message + ", ");
            Console.WriteLine($"[MailboxDB.DeleteRecvMail] ErrorCode: {nameof(ErrorCode.DeleteRecvMailException)}, ID: {id}");
            return new(ErrorCode.DeleteRecvMailException, new List<ItemInfo>());
        }
    }

    public async Task<Tuple<ErrorCode, List<ItemInfo>>> SeeUnRecvMail(string id)
    {
        var mailBox = new List<ItemInfo>();

        try
        {
            var mails = await _queryFactory.Query("mailbox")
                       .Where("UserID", id)
                       .Where("IsRecv", false)
                       .Where("IsDelete", false)
                       .GetAsync();

            foreach (var mail in mails)
            {
                mailBox.Add(new (mail.Item1, mail.Item2));
            }

            Console.WriteLine($"[SeeUnRecvMail] ID: {id}");
            return new (ErrorCode.None, mailBox);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error Msg: " + e.Message + ", ");
            Console.WriteLine($"[MailboxDB.SeeUnRecvMail] ErrorCode: {nameof(ErrorCode.SeeUnRecvMailException)}, ID: {id}");
            return new(ErrorCode.SeeUnRecvMailException, mailBox);
        }
    }

    public async Task<Tuple<ErrorCode, List<ItemInfo>>> OrganizeMail(string id, bool isAscending)
    {
        if(isAscending)
        {
            return await LoadMailbox(id);
        }
        else
        {
            //Decending order
            return new(ErrorCode.None, new List<ItemInfo>());
        }
    }
}
