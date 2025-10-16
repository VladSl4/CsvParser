using AutoMapper;
using Grpc.Core;
using CustomerService.Protos;
using Google.Protobuf.WellKnownTypes;
using static CustomerService.Protos.RpcCustomerTripRecordsService;
using static DataProvider.Protos.RpcTripRecordsService;
namespace CustomerService.Services;

public class TripRecordsService: RpcCustomerTripRecordsServiceBase
{
    private readonly RpcTripRecordsServiceClient _rpcTripRecordsServiceClient;
    private readonly IMapper _mapper;

    public TripRecordsService(RpcTripRecordsServiceClient rpcTripRecordsServiceClient, IMapper mapper)
    {
        _rpcTripRecordsServiceClient = rpcTripRecordsServiceClient;
        _mapper = mapper;
    }
    
    public override async Task<RpcTripRecords> GetTopByDistance(RpcTopRequest request, ServerCallContext context)
    {
        var providerRequest = _mapper.Map<DataProvider.Protos.RpcTopRequest>(request);

        var providerResponse = await _rpcTripRecordsServiceClient.GetTopByDistanceAsync(providerRequest, cancellationToken: context.CancellationToken);
        
        var customerResponse = _mapper.Map<RpcTripRecords>(providerResponse);

        return customerResponse;
    }    
    
    public override async Task<RpcTopTipResponse> GetTopAverageTip(Empty request, ServerCallContext context)
    {
        var providerResponse = await _rpcTripRecordsServiceClient.GetTopAverageTipAsync(request, cancellationToken: context.CancellationToken);
        
        var customerResponse = _mapper.Map<RpcTopTipResponse>(providerResponse);

        return customerResponse;
    } 
    
    public override async Task<RpcTripRecords> GetTopByDuration(RpcTopRequest request, ServerCallContext context)
    {
        var providerRequest = _mapper.Map<DataProvider.Protos.RpcTopRequest>(request);
        
        var providerResponse = await _rpcTripRecordsServiceClient.GetTopByDurationAsync(providerRequest, cancellationToken: context.CancellationToken);
        
        var customerResponse = _mapper.Map<RpcTripRecords>(providerResponse);

        return customerResponse;
    }

    public override async Task<RpcTripRecords> SearchTripsByPickupId(RpcSearchRequest request, ServerCallContext context)
    {
        var providerRequest = _mapper.Map<DataProvider.Protos.RpcSearchRequest>(request);
        
        var providerResponse = await _rpcTripRecordsServiceClient.SearchTripsByPickupIdAsync(providerRequest, cancellationToken: context.CancellationToken);
        
        var customerResponse = _mapper.Map<RpcTripRecords>(providerResponse);

        return customerResponse;
    }
}