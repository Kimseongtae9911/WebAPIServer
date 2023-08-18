namespace WebAPIServer.Middleware;

public interface IMiddleware
{
    Task InvokeAsync(HttpContext context);
}
