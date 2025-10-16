using Common.Interceptors;
using CustomerService.Protos;
using ParserService.Protos;
using Storefront.Middleware;
using Storefront.Models.Configs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Configuration.AddJsonFile("appsettings.Development.json");
UriConfig configurationUri = new UriConfig();
builder.Configuration.GetSection(nameof(UriConfig)).Bind(configurationUri);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<ExceptionInterceptor>();
});
builder.Services.AddGrpcClient<RpcParserService.RpcParserServiceClient>(o => o.Address = configurationUri.ParserService);
builder.Services.AddGrpcClient<RpcCustomerTripRecordsService.RpcCustomerTripRecordsServiceClient>(o => o.Address = configurationUri.CustomerService);
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ErrorHandlingMiddleware>();
app.MapControllers();

app.UseHttpsRedirection();

app.Run();