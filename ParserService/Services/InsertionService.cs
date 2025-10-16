using AutoMapper;
using DataProvider.Protos;
using Grpc.Core;
using ParserService.Interfaces;
using ParserService.Protos;
using static DataProvider.Protos.RpcTripRecordsService;
using static ParserService.Protos.RpcParserService;
using RpcInsertResponse = ParserService.Protos.RpcInsertResponse;

namespace ParserService.Services;

public class InsertionService : RpcParserServiceBase
{
    private readonly RpcTripRecordsServiceClient _rpcTripRecordsServiceClient;
    private readonly IParserService _parserService;
    private readonly IMapper _mapper;

    public InsertionService(RpcTripRecordsServiceClient rpcTripRecordsServiceClient, IParserService parserService, IMapper mapper)
    {
        _rpcTripRecordsServiceClient = rpcTripRecordsServiceClient;
        _parserService = parserService;
        _mapper = mapper;
    }

    public override async Task<RpcInsertResponse> Insert(RpcInsertRequest request, ServerCallContext context)
    {
        var records = await _parserService.ParseCsvAsync(request.FilePath, context.CancellationToken);
        if(!records.Any())
            throw new RpcException(new Status(StatusCode.DataLoss, "No records found in CSV file."));
        
        var tripList = new RpcTripRecords();
        tripList.Trips.AddRange(_mapper.Map<IEnumerable<RpcTripRecord>>(records));

        var response = await _rpcTripRecordsServiceClient.InsertTripsAsync(tripList, cancellationToken: context.CancellationToken);
        return new RpcInsertResponse { Inserted = response.Inserted };
    }
}