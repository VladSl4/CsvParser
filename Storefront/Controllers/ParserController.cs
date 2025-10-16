using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ParserService.Protos;
using Storefront.Models;
using Swashbuckle.AspNetCore.Annotations;
using static ParserService.Protos.RpcParserService;

namespace Storefront.Controllers;

[ApiController]
[Route("api/insert")]
public class ParserController : ControllerBase
{
    private readonly RpcParserServiceClient _rpcParserServiceClient;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public ParserController(RpcParserServiceClient rpcParserServiceClient, IMapper mapper, IWebHostEnvironment hostingEnvironment)
    {
        _rpcParserServiceClient = rpcParserServiceClient;
        _mapper = mapper;
        _hostingEnvironment = hostingEnvironment;
    }
    
    /// <summary>
    /// Наповнити БД даними із CSV-файлу.
    /// </summary>
    [HttpPost("run")]
    [SwaggerResponse((int)HttpStatusCode.OK, "")]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, "Невірний запит.")]
    [SwaggerResponse((int)HttpStatusCode.NotFound, "По запиту нічого не знайдено.")]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Внутрішня помилка сервера")]
    [SwaggerResponse((int)HttpStatusCode.ServiceUnavailable, "Сервіс недоступний")]
    public async Task<IActionResult> LoadCsv(CancellationToken cancellationToken)
    {
        string contentRootPath = Directory.GetParent(_hostingEnvironment.ContentRootPath).FullName;
        string filePath = Path.Combine(contentRootPath, "Data", "sample-cab-data.csv");
        InsertRequest request = new() { FilePath = filePath };
        RpcInsertRequest rpcRequest = _mapper.Map<RpcInsertRequest>(request);

        RpcInsertResponse insertedCount = await _rpcParserServiceClient.InsertAsync(rpcRequest, cancellationToken: cancellationToken);

        return Ok(insertedCount.Inserted);
    }
}