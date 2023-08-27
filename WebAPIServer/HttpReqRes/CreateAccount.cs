using System.ComponentModel.DataAnnotations;
using WebAPIServer.Constants;

namespace WebAPIServer.HttpReqRes;

public class CreateAccountRequest : BaseRequest
{   
    [Required]
    [MinLength(1, ErrorMessage = "PASSWORD CANNOT BE EMPTY")]
    [StringLength(AccountConstants.PasswordLength, ErrorMessage = "PASSWORD IS TOO LONG")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}

public class CreateAccountResponse : BaseResponse
{
}
