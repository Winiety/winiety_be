using Notification.Core.Model.DTOs;
using Notification.Core.Model.Entities;
using Shared.Core.BaseModels.Responses;
using Shared.Core.Interfaces;
using System.Linq;

namespace Notification.Core.Mappings
{
    public class MapperProfile : AutoMapper.Profile
    {
        public MapperProfile()
        {
            //Notification
            CreateMap<NotificationModel, NotificationDTO>();

            CreateMap<IPagedList<NotificationModel>, PagedResponse<NotificationDTO>>()
                .ForMember(d => d.Results, o => o.MapFrom(s => s.ToList()));
        }
    }
}
