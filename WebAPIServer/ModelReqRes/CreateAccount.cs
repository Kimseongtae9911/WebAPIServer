using System.ComponentModel.DataAnnotations;

namespace WebAPIServer.ModelReqRes;

public class CreateAccountRequest
{
    [Required]
    [MinLength(1, ErrorMessage = "ID CANNOT BE EMPTY")]
    [StringLength(Constants.IdLength, ErrorMessage = "ID IS TOO LONG")]
    public string ID { get; set; } = string.Empty;

    [Required]
    [MinLength(1, ErrorMessage = "PASSWORD CANNOT BE EMPTY")]
    [StringLength(Constants.PasswordLength, ErrorMessage = "PASSWORD IS TOO LONG")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}

public class CreateAccountResponse : BaseResponse
{
}
