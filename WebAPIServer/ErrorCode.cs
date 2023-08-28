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
    LockUserRequestFailException = 3002,
    UnlockUserRequestFailException = 3003,
    LockUserRequestFail = 3004,
    UnlockUserRequestFail = 3005,

    // Item 4000 ~
    InsertItemFailException = 4000,
    LoadItemFailException = 4001,

    // Mailbox 5000 ~
    LoadMailboxException = 5000,
    SendMailException = 5001,
    RecvMailException = 5002,
    RecvAllMailException = 5003,
    DeleteRecvMailException = 5004,
    SeeUnRecvMailException = 5005,
    OrganizeMailException = 5006,
    UpdateMailboxException = 5007,
    NoMatchingMail = 5008,
}
