using Pictures.Core.Model.DTOs;
using Pictures.Core.Model.Entities;
using Shared.Core.BaseModels.Responses;
using Shared.Core.Interfaces;
using System.Linq;

namespace Pictures.Core.Mappings
{
    public class MapperProfile : AutoMapper.Profile
    {
        public MapperProfile()
        {
            //Picture
            CreateMap<Picture, PictureDTO>();

            CreateMap<IPagedList<Picture>, PagedResponse<PictureDTO>>()
                .ForMember(d => d.Results, o => o.MapFrom(s => s.ToList()));
        }
    }
}
