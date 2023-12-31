﻿using Microsoft.AspNetCore.Mvc;
using WebAPIServer.HttpReqRes;
using WebAPIServer.Services.Interfaces;

namespace WebAPIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class CreateAccountController : ControllerBase
{
    readonly IAccountDB _accountDB;

    public CreateAccountController(IAccountDB accountDB)
    {
        _accountDB = accountDB;
    }

    [HttpPost]
    public async Task<CreateAccountResponse> CreateAccount(CreateAccountRequest request)
    {
        var response = new CreateAccountResponse();

        var errorCode = await _accountDB.CreateAccount(request.ID, request.Password);

        response.Result = errorCode;
        return response;
    }
}
