using System.ComponentModel.DataAnnotations;
using WebAPIServer.Constants;

namespace WebAPIServer.HttpReqRes;

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