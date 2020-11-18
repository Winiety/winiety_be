using Rides.Core.Model.DTOs;
using Rides.Core.Model.Entities;
using Shared.Core.BaseModels.Responses;
using Shared.Core.Interfaces;
using System.Linq;

namespace Rides.Core.Mappings
{
    public class MapperProfile : AutoMapper.Profile
    {
        public MapperProfile()
        {
            //Ride
            CreateMap<Ride, RideDTO>();
            CreateMap<Ride, RideDetailDTO>();

            CreateMap<IPagedList<Ride>, PagedResponse<RideDTO>>()
                .ForMember(d => d.Results, o => o.MapFrom(s => s.ToList()));
            CreateMap<IPagedList<Ride>, PagedResponse<RideDetailDTO>>()
                .ForMember(d => d.Results, o => o.MapFrom(s => s.ToList()));
        }
    }
}
