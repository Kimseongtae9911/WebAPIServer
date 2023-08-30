using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using WebAPIServer.Services.Interfaces;

namespace WebAPIServer.Middleware;

public class VerifyUserMiddleware : IMiddleware
{
    private readonly RequestDelegate _next;
    readonly IMemoryDB _memoryDB;

    public VerifyUserMiddleware(RequestDelegate next, IMemoryDB memoryDB)
    {
        _next = next;
        _memoryDB = memoryDB;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if(context.Request.Path.StartsWithSegments("/CreateAccount") || context.Request.Path.StartsWithSegments("/Login"))
        {
            await _next(context);
            return;
        }

        try
        {
            context.Request.EnableBuffering();

            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
            {
                var requestBody = await reader.ReadToEndAsync();

                if(string.IsNullOrEmpty(requestBody))
                {
                    Console.Write($"[VerifyUserMiddleware]: Invalid Request Body");
                    return;
                }


                var jsonDocument = JsonDocument.Parse(requestBody);

                (var userID, var authToken) = ExtractIDAndAuthToken(jsonDocument);

                if(userID == null)
                {
                    return;
                }

                (var errorCode, var registerdToken) = await _memoryDB.GetAuthToken(userID);
                if (errorCode != ErrorCode.None)
                {
                    Console.Write($"[VerifyUserMiddleware]: No Client Info In Redis");
                    return;
                }
                else if (registerdToken == authToken)
                {
                    if(ErrorCode.None != await _memoryDB.LockUserRequest(userID, authToken))
                    {
                        return;
                    }

                    context.Request.Body.Position = 0;
                    await _next(context);

                    await _memoryDB.UnlockUserRequest(userID, authToken);
                    return;
                }
            }

            Console.WriteLine($"[VerifyUserMiddleware]: AuthToken Not Valid");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error Msg: " + e.Message);
            Console.WriteLine($"[VerifyUserMiddleware] ErrorCode: {nameof(ErrorCode.UnhandleException)}");
        }
        
    }

    (string userID, string authToken) ExtractIDAndAuthToken(JsonDocument document)
    {
        try
        {
            var userID = document.RootElement.GetProperty("ID").GetString();
            var authToken = document.RootElement.GetProperty("AuthToken").GetString();

            if(string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(authToken))
            {
                Console.WriteLine("[VerifyUserMiddleware]: ID or AuthToken is null");
                return (string.Empty, string.Empty);
            }

            return (userID, authToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Msg: " + ex.Message);
            return (string.Empty, string.Empty);
        }
    }
}
