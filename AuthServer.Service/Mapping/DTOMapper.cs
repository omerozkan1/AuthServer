using AuthServer.Core.DTOs;
using AuthServer.Core.Entity;
using AutoMapper;

namespace AuthServer.Service.Mapping
{
    class DTOMapper : Profile
    {
        public DTOMapper()
        {
            CreateMap<ProductDTO, Product>().ReverseMap();
            CreateMap<AppUserDTO, AppUser>().ReverseMap();
        }
    }
}
