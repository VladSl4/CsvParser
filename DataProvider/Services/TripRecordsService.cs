using AutoMapper;
using Data.Sql;
using Data.Sql.Models;
using DataProvider.Protos;
using EFCore.BulkExtensions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using static DataProvider.Protos.RpcTripRecordsService;

namespace DataProvider.Services;

public class TripRecordsService : RpcTripRecordsServiceBase
{
    private readonly TripDbContext _dbContext;
    private readonly IMapper _mapper;

    public TripRecordsService(TripDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public override async Task<RpcInsertResponse> InsertTrips(RpcTripRecords request, ServerCallContext context)
    {
        var dbEntities = _mapper.Map<List<TripRecord>>(request.Trips);
        
        await _dbContext.BulkInsertAsync(dbEntities, cancellationToken: context.CancellationToken);
        return new RpcInsertResponse { Inserted = dbEntities.Count };
    }

    public override async Task<RpcTopTipResponse> GetTopAverageTip(Empty request, ServerCallContext context)
    {
        var result = await _dbContext.TripRecords
            .AsNoTracking()
            .GroupBy(t => t.PULocationID)
            .Select(g => new { PULocationId = g.Key, AvgTip = g.Average(t => t.TipAmount) })
            .OrderByDescending(x => x.AvgTip)
            .FirstOrDefaultAsync(context.CancellationToken);

        if (result == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Данные о поездках отсутствуют."));
        }

        return new RpcTopTipResponse
        {
            PuLocationId = result.PULocationId,
            AvgTip = (double)result.AvgTip
        };
    }

    public override async Task<RpcTripRecords> GetTopByDistance(RpcTopRequest request, ServerCallContext context)
    {
        var trips = await _dbContext.TripRecords
            .AsNoTracking()
            .OrderByDescending(t => t.TripDistance)
            .Take(request.Count)
            .ToListAsync(context.CancellationToken);

        var response = new RpcTripRecords();
        response.Trips.AddRange(_mapper.Map<IEnumerable<RpcTripRecord>>(trips));
        return response;
    }

    public override async Task<RpcTripRecords> GetTopByDuration(RpcTopRequest request, ServerCallContext context)
    {
        var trips = await _dbContext.TripRecords
            .AsNoTracking()
            .OrderByDescending(t => EF.Functions.DateDiffSecond(t.PickupDatetime, t.DropoffDatetime))
            .Take(request.Count)
            .ToListAsync(context.CancellationToken);
        
        var response = new RpcTripRecords();
        response.Trips.AddRange(_mapper.Map<IEnumerable<RpcTripRecord>>(trips));
        return response;
    }
    
    public override async Task<RpcTripRecords> SearchTripsByPickupId(RpcSearchRequest request, ServerCallContext context)
    {
        var trips = await _dbContext.TripRecords
            .AsNoTracking()
            .Where(t => t.PULocationID == request.PuLocationId)
            .Take(request.Count)
            .ToListAsync(context.CancellationToken);

        var response = new RpcTripRecords();
        response.Trips.AddRange(_mapper.Map<IEnumerable<RpcTripRecord>>(trips));
        return response;
    }
}