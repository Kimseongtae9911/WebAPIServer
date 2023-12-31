﻿namespace WebAPIServer.Services.Interfaces;

public interface IAccountDB
{
    public Task<ErrorCode> CreateAccount(string id, string password);
    public Task<ErrorCode> Login(string id, string password);
}
