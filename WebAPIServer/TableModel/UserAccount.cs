namespace WebAPIServer.TableModel;

public class UserAccount
{
    private string ID { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public String SaltValue { get; set; } = String.Empty;
}
