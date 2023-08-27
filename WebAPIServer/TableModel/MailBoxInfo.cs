namespace WebAPIServer.TableModel;

public class MailboxInfo
{
    public MailboxInfo() { }
    public MailboxInfo(Int16 type, Int16 detail) { this.MailType = type; this.MailDetail = detail; }
    public Int16 MailboxID { get; set; }
    public string Sender { get; set; } = string.Empty;
    public string UserID { get; set; } = string.Empty;
    public string ReceivedDate { get; set; } = string.Empty;
    public Int16 MailType { get; set; }
    public Int16 MailDetail { get; set; }
    public bool IsReceived { get; set; }
    public bool IsDeleted { get; set; }

}
