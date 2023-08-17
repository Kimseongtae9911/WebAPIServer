namespace WebAPIServer;

public enum ErrorCode : UInt16
{
    None = 0,

    // Common 1000 ~
    UnhandleException = 1001,

    // Account 2000 ~
    CreateAccountFailException = 2001,
    CreateAccountFail = 2002,
    LoginFailNoAccount = 2003,
    LoginFailWrongPassword = 2004,
    LoginFailException = 2005,
    LoginFailRegisterRedis = 2006,
    LoginFailRegisterRedisException = 2007,

    // Redis(Memory) 3000 ~
    VerifyFailException = 3000,
    NoExistingAuthToken = 3001,

    // Item 4000 ~
    InsertItemFailException = 4000,
}
