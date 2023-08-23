using WebAPIServer.Middleware;
using WebAPIServer.Services;
using WebAPIServer.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

builder.Services.Configure<DbConfig>(configuration.GetSection(nameof(DbConfig)));

builder.Services.AddTransient<IAccountDB, AccountDB>();
builder.Services.AddTransient<IItemDB, ItemDB>();
builder.Services.AddTransient<IMailboxDB, MailboxDB>();
builder.Services.AddSingleton<IMemoryDB, MemoryDB>();
builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();

app.UseMiddleware<VerifyUserMiddleware>();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run(configuration["ServerAddress"]);