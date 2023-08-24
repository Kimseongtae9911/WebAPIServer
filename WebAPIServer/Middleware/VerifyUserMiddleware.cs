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
        if(context.Request.Path.StartsWithSegments("/CreateAccout") || context.Request.Path.StartsWithSegments("/Login"))
        {
            await _next(context);
            return;
        }

        try
        {
            using (StreamReader reader = new StreamReader(context.Request.Body))
            {
                string requestBody = await reader.ReadToEndAsync();
                MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(requestBody));

                var json = JsonDocument.Parse(memoryStream);
                var authToken = json.RootElement.GetProperty("AuthToken").GetString();
                var userID = json.RootElement.GetProperty("ID").GetString();

                memoryStream.Seek(0, SeekOrigin.Begin);
                context.Request.Body = memoryStream;

                var result = await _memoryDB.GetAuthToken(userID);
                if (result.Item1 != ErrorCode.None)
                {
                    Console.Write($"[VerifyUserMiddleware]: No Client Info In Redis");
                    return;
                }
                else if (result.Item2 == authToken)
                {
                    await _next(context);
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
}
