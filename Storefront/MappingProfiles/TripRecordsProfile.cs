using AutoMapper;
using CustomerService.Protos;
using Storefront.Models;

namespace Storefront.MappingProfiles;

public class TripRecordsProfile : Profile
{
    public TripRecordsProfile()
    {
        CreateMap<SearchRequest, RpcSearchRequest>();
        
        CreateMap<RpcTripRecord, TripRecord>()
            .ForMember(dest => dest.PickupDatetime, opt => opt.MapFrom(src => src.PickupDatetime.ToDateTime().ToUniversalTime()))
            .ForMember(dest => dest.DropoffDatetime, opt => opt.MapFrom(src => src.DropoffDatetime.ToDateTime().ToUniversalTime()));

        CreateMap<RpcTopTipResponse, TopTipResponse>()
            .ForMember(dest => dest.PuLocationId, opt => opt.MapFrom(src => src.PuLocationId))
            .ForMember(dest => dest.AvgTip, opt => opt.MapFrom(src => src.AvgTip));
    }
}