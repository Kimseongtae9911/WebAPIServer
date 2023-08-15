namespace WebAPIServer.ModelReqRes;

public class BaseResponse
{
    public ErrorCode Result { get; set; } = ErrorCode.None;
}
