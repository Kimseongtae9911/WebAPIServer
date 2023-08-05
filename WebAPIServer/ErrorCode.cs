namespace WebAPIServer
{
    public enum ErrorCode : UInt16
    {
        None = 0,

        // Common 1000 ~
        UnhandleException = 1001,

        // Account 2000 ~
        SignUpFailException = 2001,
        SignUpFail = 2002,
    }
}
