using System.ComponentModel.DataAnnotations;

namespace WebAPIServer.ModelReqRes;

public class InsertItemRequest : BaseRequest
{
    [Required]
    [MinLength(1, ErrorMessage = "ID CANNOT BE EMPTY")]
    [StringLength(Constants.IdLength, ErrorMessage = "ID IS TOO LONG")]
    public string ID { get; set; } = string.Empty;
    public Int16 ItemCode { get; set; }
}

public class InsertItemResponse : BaseResponse
{
}