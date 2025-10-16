using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using CustomerProtos = CustomerService.Protos;
using DataProviderProtos = DataProvider.Protos;

namespace CustomerService.MappingProfiles;

public class TripRecordsProfile : Profile
{
    public TripRecordsProfile()
    {
        CreateMap<CustomerProtos.RpcTopRequest, DataProviderProtos.RpcTopRequest>();
        CreateMap<CustomerProtos.RpcSearchRequest, DataProviderProtos.RpcSearchRequest>();

        CreateMap<CustomerProtos.RpcTripRecord, DataProviderProtos.RpcTripRecord>();

        CreateMap<CustomerProtos.RpcTripRecords, DataProviderProtos.RpcTripRecords>();

        CreateMap<DataProviderProtos.RpcTopTipResponse, CustomerProtos.RpcTopTipResponse>();

        CreateMap<DataProviderProtos.RpcTripRecord, CustomerProtos.RpcTripRecord>();

        CreateMap<DataProviderProtos.RpcTripRecords, CustomerProtos.RpcTripRecords>();
    }
}