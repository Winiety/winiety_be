using Shared.Core.BaseModels.Responses;
using Shared.Core.Interfaces;
using System;
using System.Linq;

namespace Payment.Core.Mappings
{
    public class MapperProfile : AutoMapper.Profile
    {
        public MapperProfile()
        {
            //Payment
            CreateMap<Model.Entities.Payment, Model.DTOs.PaymentDTO>();
            CreateMap<Model.Entities.Payment, Model.DTOs.PaymentDetailDTO>();
            CreateMap<IPagedList<Model.Entities.Payment>, PagedResponse<Model.DTOs.PaymentDTO>>()
                .ForMember(d => d.Results, o => o.MapFrom(s => s.ToList()));
            CreateMap<IPagedList<Model.Entities.Payment>, PagedResponse<Model.DTOs.PaymentDetailDTO>>()
                .ForMember(d => d.Results, o => o.MapFrom(s => s.ToList()));
            CreateMap<Model.Entities.Payment, ResultResponse<Model.DTOs.PaymentDTO>>()
                .ForMember(d => d.Result, o => o.MapFrom(s => s));

            //Winieta
            CreateMap<Model.Entities.Winieta, Model.DTOs.WinietaDTO>()
                .ForMember(d => d.IsActive, o => o.MapFrom(s => s.ExpirationDate > DateTime.UtcNow && s.PaymentStatus == "COMPLETED"));
            CreateMap<IPagedList<Model.Entities.Winieta>, PagedResponse<Model.DTOs.WinietaDTO>>()
                .ForMember(d => d.Results, o => o.MapFrom(s => s.ToList()));
            CreateMap<Model.Entities.Winieta, ResultResponse<Model.DTOs.WinietaDTO>>()
                .ForMember(d => d.Result, o => o.MapFrom(s => s));
        }
    }
}
