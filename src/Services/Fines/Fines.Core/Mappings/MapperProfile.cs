using Fines.Core.Model.DTOs.Complaint;
using Fines.Core.Model.DTOs.Fine;
using Fines.Core.Model.Entities;
using Shared.Core.BaseModels.Responses;
using Shared.Core.Interfaces;
using System.Linq;

namespace Fines.Core.Mappings
{
    public class MapperProfile : AutoMapper.Profile
    {
        public MapperProfile()
        {
            //Complaint
            CreateMap<Complaint, ComplaintDTO>();
            CreateMap<Complaint, ComplaintDetailDTO>();
            CreateMap<IPagedList<Complaint>, PagedResponse<ComplaintDTO>>()
                .ForMember(d => d.Results, o => o.MapFrom(s => s.ToList()));
            CreateMap<IPagedList<Complaint>, PagedResponse<ComplaintDetailDTO>>()
                .ForMember(d => d.Results, o => o.MapFrom(s => s.ToList()));

            //Fine
            CreateMap<Fine, FineDTO>();
            CreateMap<Fine, FineDetailDTO>();
            CreateMap<IPagedList<Fine>, PagedResponse<FineDTO>>()
                .ForMember(d => d.Results, o => o.MapFrom(s => s.ToList()));
            CreateMap<IPagedList<Fine>, PagedResponse<FineDetailDTO>>()
                .ForMember(d => d.Results, o => o.MapFrom(s => s.ToList()));
        }
    }
}
