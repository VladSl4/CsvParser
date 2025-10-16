using AutoMapper;
using DataProvider.Protos;
using Google.Protobuf.WellKnownTypes;
using ParserService.Models;

namespace ParserService.MappingProfiles;

public class TripRecordProfile : Profile
{
    public TripRecordProfile()
    {
        {
            CreateMap<TripRecord, RpcTripRecord>()
                .ForMember(dest => dest.PickupDatetime,opt => opt.MapFrom(src => Timestamp.FromDateTime(src.PickupDatetime.ToUniversalTime())))
                .ForMember(dest => dest.DropoffDatetime, opt => opt.MapFrom(src => Timestamp.FromDateTime(src.DropoffDatetime.ToUniversalTime())));
            
            CreateMap<List<TripRecord>, RpcTripRecords>()
                .ForMember(dest => dest.Trips, opt => opt.MapFrom(src => src));
        }
    }
}