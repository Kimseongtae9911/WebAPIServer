using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace WebAPIServer.ModelReqRes;

public class LoadMailboxRequest : BaseRequest
{
    [Required]
    [MinLength(1, ErrorMessage = "ID CANNOT BE EMPTY")]
    [StringLength(Constants.IdLength, ErrorMessage = "ID IS TOO LONG")]
    public string ID { get; set; } = string.Empty;
}

public class LoadMailboxResponse : BaseResponse
{
    public ArrayList mails = new ArrayList();
}
