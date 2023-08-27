using System.ComponentModel.DataAnnotations;

namespace WebAPIServer.ModelReqRes;

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
    public List<Tuple<Int16, Int16>> Items { get; set; } = new List<Tuple<Int16, Int16>>();
}
