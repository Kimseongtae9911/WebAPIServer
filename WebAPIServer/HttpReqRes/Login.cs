using System.ComponentModel.DataAnnotations;
using WebAPIServer.TableModel;

namespace WebAPIServer.HttpReqRes;

public class LoginRequest : BaseRequest
{
    [Required]
    [MinLength(1, ErrorMessage = "PASSWORD CANNOT BE EMPTY")]
    [StringLength(AccountConstants.PasswordLength, ErrorMessage = "PASSWORD IS TOO LONG")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}

public class LoginResponse : BaseResponse
{
    public string AuthToken { get; set; } = string.Empty;
    public List<ItemInfo> Items { get; set; } = new List<ItemInfo>();
}
