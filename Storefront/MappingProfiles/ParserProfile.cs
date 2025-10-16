using AutoMapper;
using ParserService.Protos;
using Storefront.Models;

namespace Storefront.MappingProfiles;

public class ParserProfile: Profile
{
    public ParserProfile()
    {
        CreateMap<InsertRequest, RpcInsertRequest>();
    }
}