using Profile.Core.Models.DTOs.Car;
using Profile.Core.Models.DTOs.UserProfile;
using Profile.Core.Models.Entities;
using Profile.Core.Models.Requests.Car;
using Profile.Core.Models.Requests.UserProfile;
using Shared.Core.BaseModels.Responses;
using Shared.Core.Interfaces;
using System.Linq;

namespace Profile.Core.Mappings
{
    public class MapperProfile : AutoMapper.Profile
    {
        public MapperProfile()
        {
            //Car
            CreateMap<CreateCarRequest, Car>();
            CreateMap<UpdateCarRequest, Car>();
            CreateMap<Car, CarDTO>();
            CreateMap<IPagedList<Car>, PagedResponse<CarDTO>>()
                .ForMember(d => d.Results, o => o.MapFrom(s => s.ToList()));

            //UserProfile
            CreateMap<UpdateUserProfileRequest, UserProfile>();
            CreateMap<UserProfile, UserProfileDTO>();
        }
    }
}
