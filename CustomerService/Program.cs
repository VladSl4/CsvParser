using Common.Interceptors;
using CustomerService.Models.Configs;
using CustomerService.Services;
using DataProvider.Protos;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.Development.json");

UriConfig configurationUri = new UriConfig();
builder.Configuration.GetSection(nameof(UriConfig)).Bind(configurationUri);
// Add services to the container.
builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<ExceptionInterceptor>();
});
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddGrpcClient<RpcTripRecordsService.RpcTripRecordsServiceClient>(o => o.Address = configurationUri.DataProvider);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<TripRecordsService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();