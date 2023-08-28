using System.ComponentModel.DataAnnotations;
namespace WebAPIServer.HttpReqRes;

public class AccountConstants
{
    public const Int16 IDLength = 20;
    public const Int16 PasswordLength = 20;
}

public class BaseRequest
{
    [Required]
    [MinLength(1, ErrorMessage = "ID CANNOT BE EMPTY")]
    [StringLength(AccountConstants.IDLength, ErrorMessage = "ID IS TOO LONG")]
    public string ID { get; set; } = string.Empty;
    public string AuthToken { get; set; } = string.Empty;
}
public class BaseResponse
{
    public ErrorCode Result { get; set; } = ErrorCode.None;
}