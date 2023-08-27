using System.ComponentModel.DataAnnotations;

namespace WebAPIServer.HttpReqRes;

public class InsertItemRequest : BaseRequest
{
    public Int16 ItemCode { get; set; }
}

public class InsertItemResponse : BaseResponse
{
}