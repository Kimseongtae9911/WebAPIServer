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

    public async Task<Tuple<ErrorCode, List<MailboxInfo>>> LoadMailbox(string id)
    {
        try
        {
            var mails = await _queryFactory.Query("mailbox")
                       .Where("UserID", id)
                       .Where("IsDeleted", false)
                       .Limit(Constants.MailboxConstants.MailNumInPage)
                       .GetAsync<MailboxInfo>();

            Console.WriteLine($"[LoadMailbox] ID: {id}");
            return new (ErrorCode.None, mails.ToList());
        }
        catch (Exception e)
        {
            Console.WriteLine("Error Msg: " + e.Message + ", ");
            Console.WriteLine($"[MailboxDB.LoadMailbox] ErrorCode: {nameof(ErrorCode.LoadMailboxException)}, ID: {id}");
            return new(ErrorCode.LoadMailboxException, new List<MailboxInfo>());
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
                    MailDetail = mailDetail,
                    IsReceived = false,
                    IsDeleted = false,
                    ReceivedDate = DateTime.Now
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

    public async Task<Tuple<ErrorCode, MailboxInfo>> RecvMail(string id, Int16 mailboxID)
    {
        try
        {
            var recvMail = await _queryFactory.Query("mailbox")
                .Where("UserID", id)
                .Where("MailboxID", mailboxID)
                .Where("IsReceived", false)
                .Where("IsDeleted", false)
                .FirstOrDefaultAsync() ?? new MailboxInfo(-1, -1);

            await _queryFactory.Query("mailbox")
                .Where("UserID", id)
                .Where("MailboxID", mailboxID)
                .UpdateAsync(new { IsReceived = true });

            Console.WriteLine($"[RecvMail] ID: {id}, MailboxID: {mailboxID}, RecvMailType: {recvMail.MailType}, RecvMailDetail: {recvMail.MailDetail}");
            return new (ErrorCode.None, new ((Int16)recvMail.MailType, (Int16)recvMail.MailDetail));
        }
        catch (Exception e)
        {
            Console.WriteLine("Error Msg: " + e.Message + ", ");
            Console.WriteLine($"[MailboxDB.RecvMail] ErrorCode: {nameof(ErrorCode.RecvMailException)}, ID: {id}, MailNum: {mailboxID}");
            return new (ErrorCode.RecvMailException, new (-1, -1));
        }
    }

    public async Task<Tuple<ErrorCode, List<MailboxInfo>>> RecvAllMail(string id)
    {
        try
        {
            var recvMails = await _queryFactory.Query("mailbox")
                .Where("UserID", id)
                .Where("IsReceived", false)
                .Where("IsDeleted", false)
                .GetAsync<MailboxInfo>();

            if (recvMails.Any())
            {
                await _queryFactory.Query("mailbox")
                    .Where("UserID", id)
                    .Where("IsReceived", false)
                    .UpdateAsync(new { IsReceived = true });

                Console.WriteLine($"[RecvAllMail] ID: {id}");
                return new(ErrorCode.None, recvMails.ToList());
            }
            else
            {
                Console.WriteLine($"[RecvAllMail] No mails to receive for ID: {id}");
                return new (ErrorCode.None, new List<MailboxInfo>());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error Msg: " + e.Message + ", ");
            Console.WriteLine($"[MailboxDB.RecvAllMail] ErrorCode: {nameof(ErrorCode.RecvAllMailException)}, ID: {id}");
            return new(ErrorCode.RecvAllMailException, new List<MailboxInfo>());
        }
    }

    public async Task<Tuple<ErrorCode, List<MailboxInfo>>> DeleteRecvMail(string id)
    {
        try
        {
            await _queryFactory.Query("mailbox")
                .Where("UserID", id)
                .Where("IsReceived", true)
                .Where("IsDeleted", false)
                .UpdateAsync(new { IsDeleted = true });

            Console.WriteLine($"[DeleteRecvMail] ID: {id}");

            return await LoadMailbox(id);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error Msg: " + e.Message + ", ");
            Console.WriteLine($"[MailboxDB.DeleteRecvMail] ErrorCode: {nameof(ErrorCode.DeleteRecvMailException)}, ID: {id}");
            return new(ErrorCode.DeleteRecvMailException, new List<MailboxInfo>());
        }
    }

    public async Task<Tuple<ErrorCode, List<MailboxInfo>>> SeeUnRecvMail(string id)
    {
        try
        {
            var mails = await _queryFactory.Query("mailbox")
                       .Where("UserID", id)
                       .Where("IsReceived", false)
                       .Where("IsDeleted", false)
                       .GetAsync<MailboxInfo>();


            Console.WriteLine($"[SeeUnRecvMail] ID: {id}");
            return new (ErrorCode.None, mails.ToList());
        }
        catch (Exception e)
        {
            Console.WriteLine("Error Msg: " + e.Message + ", ");
            Console.WriteLine($"[MailboxDB.SeeUnRecvMail] ErrorCode: {nameof(ErrorCode.SeeUnRecvMailException)}, ID: {id}");
            return new(ErrorCode.SeeUnRecvMailException, new List<MailboxInfo>());
        }
    }

    public async Task<Tuple<ErrorCode, List<MailboxInfo>>> OrganizeMail(string id, bool isAscending)
    {
        if(isAscending)
        {
            return await LoadMailbox(id);
        }
        else
        {
            try
            {
                var mails = await _queryFactory.Query("mailbox")
                    .Where("UserID", id)
                    .Where("IsDeleted", false)
                    .OrderByDesc("ReceivedDate")
                    .Limit(Constants.MailboxConstants.MailNumInPage)
                    .GetAsync<MailboxInfo>();

                Console.WriteLine($"[OrganizeMail] ID: {id}, IsAscending: {isAscending}");
                return new(ErrorCode.None, mails.ToList());
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error Msg: " + ex.Message + ", ");
                Console.WriteLine($"[MailboxDB.OrganizeMail] ErrorCode: {nameof(ErrorCode.OrganizeMailException)}, ID: {id}");
                return new(ErrorCode.OrganizeMailException, new List<MailboxInfo>());
            } 
        }
    }
}
