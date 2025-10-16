using Common.Interceptors;
using DataProvider.Protos;
using ParserService.Interfaces;
using ParserService.MappingProfiles;
using ParserService.Models.Configs;
using ParserService.Protos;
using ParserService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.Development.json");
UriConfig configurationUri = new UriConfig();
builder.Configuration.GetSection(nameof(UriConfig)).Bind(configurationUri);

// Add services to the container.
builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<ExceptionInterceptor>();
});
builder.Services.AddGrpcClient<RpcTripRecordsService.RpcTripRecordsServiceClient>(o =>
{
    o.Address = configurationUri.DataProvider;
});

builder.Services.AddScoped<IParserService, CsvParserService>();
builder.Services.AddAutoMapper(typeof(Program));
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<InsertionService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();