using Microsoft.AspNetCore.Mvc;
using WebAPIServer.ModelReqRes;
using WebAPIServer.Services;
using WebAPIServer.Services.Interfaces;

namespace WebAPIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    readonly IAccountDB _accountDB;
    readonly IMemoryDB _memoryDB;
    readonly IItemDB _itemDB;

    public LoginController(IAccountDB accountDB, IItemDB itemDB, IMemoryDB memoryDB)
    {
        _accountDB = accountDB;
        _itemDB = itemDB;
        _memoryDB = memoryDB;
    }

    [HttpPost]
    public async Task<LoginResponse> Login(LoginRequest request)
    {
        var response = new LoginResponse();

        var errorCode = await _accountDB.Login(request.ID, request.Password);
        if(errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            return response;
        }

        var authToken = Security.GetSaltString();

        errorCode = await _memoryDB.RegisterUser(request.ID, authToken);
        if (errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            return response;
        }

        (errorCode, var list) = await _itemDB.LoadItem(request.ID);
        if (errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            return response;
        }

        foreach ((var itemCode, var count) in list)
        {
            response.Items.Add(new(itemCode, count));
        }

        response.AuthToken = authToken;
        return response;
    }
}
