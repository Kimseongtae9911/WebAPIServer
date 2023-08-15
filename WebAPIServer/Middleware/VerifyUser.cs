using WebAPIServer.Services;

namespace WebAPIServer.Middleware;

public class VerifyUser : IMiddleware
{
    readonly IMemoryDB _memoryDB;

    public VerifyUser(IMemoryDB memoryDB)
    {
        _memoryDB = memoryDB;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        string authToken = context.Request.Headers["AuthToken"];

        if(context.Request.Path.StartsWithSegments("/CreateAccout") || context.Request.Path.StartsWithSegments("/Login"))
        {
            await next(context);
            return;
        }

        var result = await _memoryDB.GetAuthToken(context.Request.Headers["id"]);
        if (result.Item1 != ErrorCode.None)
        {
            Console.Write("VerifyUser: AuthToken Not Valid");
            return;
        }
        else if (result.Item2 == authToken)
        {
            await next(context);
            return;
        }

        Console.Write("MiddleWare: VerifyUser Error");
    }
}
