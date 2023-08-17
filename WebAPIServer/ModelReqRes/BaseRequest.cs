namespace WebAPIServer.ModelReqRes;

public class BaseRequest
{
    public string AuthToken { get; set; } = string.Empty;
}
public class BaseResponse
{
    public ErrorCode Result { get; set; } = ErrorCode.None;
}