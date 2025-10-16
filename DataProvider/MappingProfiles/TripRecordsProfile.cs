using AutoMapper;
using Data.Sql.Models;
using DataProvider.Protos;
using Google.Protobuf.WellKnownTypes;

namespace DataProvider.MappingProfiles;

public class TripRecordsProfile:  Profile
{
    public TripRecordsProfile()
    {
        CreateMap<RpcTripRecord, TripRecord>()
            .ForMember(dest => dest.PickupDatetime, opt => opt.MapFrom(src => src.PickupDatetime.ToDateTime().ToUniversalTime()))
            .ForMember(dest => dest.DropoffDatetime, opt => opt.MapFrom(src => src.DropoffDatetime.ToDateTime().ToUniversalTime()));

        CreateMap<TripRecord, RpcTripRecord>()
            .ForMember(dest => dest.PickupDatetime,opt => opt.MapFrom(src => Timestamp.FromDateTime(src.PickupDatetime.ToUniversalTime())))
            .ForMember(dest => dest.DropoffDatetime,opt => opt.MapFrom(src => Timestamp.FromDateTime(src.DropoffDatetime.ToUniversalTime())));
    }
}