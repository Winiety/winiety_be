using AutoMapper;
using Contracts.Commands;
using Contracts.Results;
using Fines.Core.Interfaces;
using Fines.Core.Model.DTOs.Complaint;
using Fines.Core.Model.Entities;
using Fines.Core.Services;
using MassTransit;
using Moq;
using Shared.Core.Interfaces;
using Shared.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fines.UnitTests.ComplaintServiceUnitTests
{
    public class ComplaintServiceSetup
    {
        protected readonly Mock<IComplaintRepository> _complaintRepository;
        protected readonly Mock<IUserContext> _userContext;
        protected readonly Mock<IBusControl> _bus;
        protected readonly Mock<IRequestClient<GetRide>> _requestClient;
        protected readonly Mock<IMapper> _mapper;
        protected readonly ComplaintService _complaintService;

        public ComplaintServiceSetup()
        {
            _complaintRepository = new Mock<IComplaintRepository>();
            _bus = new Mock<IBusControl>();
            _userContext = new Mock<IUserContext>();
            _requestClient = new Mock<IRequestClient<GetRide>>();
            _mapper = new Mock<IMapper>();
            _complaintService = new ComplaintService(_complaintRepository.Object, _bus.Object, _userContext.Object, _requestClient.Object, _mapper.Object);
        }

        public IEnumerable<Complaint> GetComplaints()
        {
            return new List<Complaint>
            {
                new Complaint
                {
                    Id = 1,
                    PlateNumber = "112233",
                    UserId = 1,
                    Description = "asd",
                    RideId = 1,
                    PictureId = 1,
                    CreateTime = DateTimeOffset.UtcNow
                },
                new Complaint
                {
                    Id = 2,
                    PlateNumber = "112233",
                    UserId = 1,
                    Description = "asd",
                    RideId = 1,
                    PictureId = 1,
                    CreateTime = DateTimeOffset.UtcNow
                },
                new Complaint
                {
                    Id = 3,
                    PlateNumber = "112233",
                    UserId = 2,
                    Description = "asd",
                    RideId = 1,
                    PictureId = 1,
                    CreateTime = DateTimeOffset.UtcNow
                },
                new Complaint
                {
                    Id = 4,
                    PlateNumber = "112233",
                    UserId = 2,
                    Description = "asd",
                    RideId = 1,
                    PictureId = 1,
                    CreateTime = DateTimeOffset.UtcNow
                }
            };
        }

        public IEnumerable<ComplaintDTO> GetComplaintsDTO(IEnumerable<Complaint> complaints)
        {
            return complaints.Select(c => new ComplaintDTO
            {
                Id = c.Id,
                PlateNumber = c.PlateNumber,
                CreateTime = c.CreateTime,
                PictureId = c.PictureId,
                RideId = c.RideId,
                Description = c.Description,
            });
        }

        public IEnumerable<ComplaintDetailDTO> GetComplaintDetailsDTO(IEnumerable<Complaint> complaints)
        {
            return complaints.Select(c => new ComplaintDetailDTO
            {
                Id = c.Id,
                UserId = c.UserId,
                PlateNumber = c.PlateNumber,
                CreateTime = c.CreateTime,
                PictureId = c.PictureId,
                RideId = c.RideId,
                Description = c.Description,
            });
        }

        public class PagedList<T> : List<T>, IPagedList<T>
        {
            public int PageSize { get; set; }
            public int CurrentPage { get; set; }
            public int TotalPages { get; set; }
            public int TotalCount { get; set; }
        }

        public class GetRideResultMock : GetRideResult
        {
            public int Id { get; set; }
            public int? UserId { get; set; }
            public int PictureId { get; set; }
            public string PlateNumber { get; set; }
            public DateTimeOffset RideDateTime { get; set; }
        }

        public class GetRideNotFoundMock : GetRideNotFound
        {
            public int RideId { get; set; }
        }

        public class ResponseMock<TResponse> : Response<TResponse> where TResponse : class
        {
            public ResponseMock(TResponse response)
            {
                Message = response;
            }

            public TResponse Message { get; }

            public Guid? MessageId { get; }

            public Guid? RequestId { get; }

            public Guid? CorrelationId { get; }

            public Guid? ConversationId { get; }

            public Guid? InitiatorId { get; }

            public DateTime? ExpirationTime { get; }

            public Uri SourceAddress { get; }

            public Uri DestinationAddress { get; }

            public Uri ResponseAddress { get; }

            public Uri FaultAddress { get; }

            public DateTime? SentTime { get; }

            public Headers Headers { get; }

            public HostInfo Host { get; }
        }
    }
}
