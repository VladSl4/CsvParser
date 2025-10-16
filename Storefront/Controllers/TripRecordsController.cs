using System.Net;
using AutoMapper;
using CustomerService.Protos;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Storefront.Models;
using Swashbuckle.AspNetCore.Annotations;
using static CustomerService.Protos.RpcCustomerTripRecordsService;

namespace Storefront.Controllers;

[ApiController]
[Route("api/tripRecords")]
public class TripRecordsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly RpcCustomerTripRecordsServiceClient _rpcCustomerTripRecordsServiceClient;

    public TripRecordsController(IMapper mapper, RpcCustomerTripRecordsServiceClient rpcCustomerTripRecordsServiceClient)
    {
        _mapper = mapper;
        _rpcCustomerTripRecordsServiceClient = rpcCustomerTripRecordsServiceClient;
    }

    /// <summary>
    /// Отримати топ поїздок за дистанією.
    /// </summary>
    /// <param name="count">Кількість записів для отримання.</param>
    /// <returns>Список поїздок.</returns>
    [HttpGet("top-by-distance")]
    [SwaggerResponse((int)HttpStatusCode.OK, "")]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, "Невірний запит.")]
    [SwaggerResponse((int)HttpStatusCode.NotFound, "По запиту нічого не знайдено.")]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Внутрішня помилка сервера")]
    [SwaggerResponse((int)HttpStatusCode.ServiceUnavailable, "Сервіс недоступний")]
    public async Task<IEnumerable<TripRecord>> GetTopByDistance(CancellationToken cancellationToken, [FromQuery] int count = 100)
    {
        var rpcRequest = new RpcTopRequest { Count = count };

        var rpcResponse = await _rpcCustomerTripRecordsServiceClient.GetTopByDistanceAsync(rpcRequest, cancellationToken: cancellationToken);

        var response = _mapper.Map<IEnumerable<TripRecord>>(rpcResponse.Trips);

        return response;
    }

    /// <summary>
    /// Отримати топ поїздок за тривалістю.
    /// </summary>
    /// <param name="count">Кількість записів для отримання.</param>
    /// <returns>Список поїздок.</returns>
    [HttpGet("top-by-duration")]
    [SwaggerResponse((int)HttpStatusCode.OK, "")]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, "Невірний запит.")]
    [SwaggerResponse((int)HttpStatusCode.NotFound, "По запиту нічого не знайдено.")]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Внутрішня помилка сервера")]
    [SwaggerResponse((int)HttpStatusCode.ServiceUnavailable, "Сервіс недоступний")]
    public async Task<IEnumerable<TripRecord>> GetTopByDuration(CancellationToken cancellationToken, [FromQuery] int count = 100)
    {
        var rpcRequest = new RpcTopRequest { Count = count };
        var rpcResponse = await _rpcCustomerTripRecordsServiceClient.GetTopByDurationAsync(rpcRequest, cancellationToken: cancellationToken);
        var response = _mapper.Map<IEnumerable<TripRecord>>(rpcResponse.Trips);
        return response;
    }

    /// <summary>
    /// Знайти локацію із найвищим середнім рівнем чайових.
    /// </summary>
    [HttpGet("top-average-tip-location")]
    [SwaggerResponse((int)HttpStatusCode.OK, "")]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, "Невірний запит.")]
    [SwaggerResponse((int)HttpStatusCode.NotFound, "По запиту нічого не знайдено.")]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Внутрішня помилка сервера")]
    [SwaggerResponse((int)HttpStatusCode.ServiceUnavailable, "Сервіс недоступний")]
    public async Task<TopTipResponse> GetTopAverageTipLocation(CancellationToken cancellationToken)
    {
        var rpcResponse = await _rpcCustomerTripRecordsServiceClient.GetTopAverageTipAsync(new Empty(), cancellationToken: cancellationToken);
        var responseDto = _mapper.Map<TopTipResponse>(rpcResponse);
        return responseDto;
    }

    /// <summary>
    /// Знайти поїздки за ID локації посадки.
    /// </summary>
    /// <returns>Список поїздок.</returns>
    [HttpGet("by-pickup-location")]
    [SwaggerResponse((int)HttpStatusCode.OK, "")]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, "Невірний запит.")]
    [SwaggerResponse((int)HttpStatusCode.NotFound, "По запиту нічого не знайдено.")]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Внутрішня помилка сервера")]
    [SwaggerResponse((int)HttpStatusCode.ServiceUnavailable, "Сервіс недоступний")]
    public async Task<IEnumerable<TripRecord>> SearchByPickupId([FromQuery] SearchRequest request, CancellationToken cancellationToken)
    {
        var rpcRequest = _mapper.Map<RpcSearchRequest>(request);
        var rpcResponse = await _rpcCustomerTripRecordsServiceClient.SearchTripsByPickupIdAsync(rpcRequest, cancellationToken: cancellationToken);
        var response = _mapper.Map<IEnumerable<TripRecord>>(rpcResponse.Trips);
        return response;
    }
}