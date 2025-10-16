using Common.Interceptors;
using Data.Sql;
using Data.Sql.Models.Configs;
using DataProvider.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.Development.json");

builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<ExceptionInterceptor>();
});
builder.Services.Configure<DbConfig>(builder.Configuration.GetSection(nameof(DbConfig)));
builder.Services.AddDbContext<TripDbContext>();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

app.MapGrpcService<TripRecordsService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();