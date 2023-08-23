using System.ComponentModel.DataAnnotations;

namespace WebAPIServer.ModelReqRes;

public class CreateAccountRequest : BaseRequest
{   
    [Required]
    [MinLength(1, ErrorMessage = "PASSWORD CANNOT BE EMPTY")]
    [StringLength(Constants.PasswordLength, ErrorMessage = "PASSWORD IS TOO LONG")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}

public class CreateAccountResponse : BaseResponse
{
}
