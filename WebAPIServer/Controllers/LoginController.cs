using Microsoft.AspNetCore.Mvc;
using WebAPIServer.ModelReqRes;
using WebAPIServer.Services;
using WebAPIServer.Utils;

namespace WebAPIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    readonly IAccountDB _accountDB;
    readonly IMemoryDB _memoryDB;

    public LoginController(IAccountDB accountDB, IMemoryDB memoryDB)
    {
        _accountDB = accountDB;
        _memoryDB = memoryDB;
    }

    [HttpPost]
    public async Task<LoginResponse> Post(LoginRequest request)
    {
        var response = new LoginResponse();

        var errorCode = await _accountDB.Login(request.ID, request.Password);
        if(errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            return response;
        }

        var authToken = Security.GetSaltString();

        errorCode = await _memoryDB.RegisterUser(authToken, request.Password);
        if (errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            return response;
        }

        response.AuthToken = authToken;
        return response;
    }
}
