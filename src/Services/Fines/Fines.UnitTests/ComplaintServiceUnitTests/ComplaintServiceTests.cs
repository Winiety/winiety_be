using Contracts.Results;
using Fines.Core.Model.DTOs.Complaint;
using Fines.Core.Model.Entities;
using Fines.Core.Model.Requests.Complaint;
using MassTransit;
using Moq;
using Shared.Core.BaseModels.Requests;
using Shared.Core.BaseModels.Responses;
using Shared.Core.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace Fines.UnitTests.ComplaintServiceUnitTests
{
    public class ComplaintServiceTests : ComplaintServiceSetup
    {
        [Fact]
        public async Task GetComplaintsAsync_Should_ReturnComplaints()
        {
            var complaints = GetComplaints();
            var complaintDtos = GetComplaintDetailsDTO(complaints);

            var searchRequest = new SearchRequest();
            var paged = new PagedList<Complaint>()
            {
                TotalPages = 1,
                CurrentPage = searchRequest.PageNumber,
                TotalCount = complaints.Count(),
                PageSize = searchRequest.PageSize,
            };

            var response = new PagedResponse<ComplaintDetailDTO>()
            {
                TotalPages = 1,
                CurrentPage = searchRequest.PageNumber,
                TotalCount = complaints.Count(),
                PageSize = searchRequest.PageSize,
                Results = GetComplaintDetailsDTO(complaints)
            };

            paged.AddRange(complaints);

            _userContext
                .Setup(c => c.GetUserId())
                .Returns(1);

            _complaintRepository
               .Setup(c => c.GetQueryable())
               .Returns(complaints.AsQueryable());

            _complaintRepository
                .Setup(c => c.GetPagedByAsync(It.IsAny<IQueryable<Complaint>>(), searchRequest.PageNumber, searchRequest.PageSize))
                .ReturnsAsync(paged);

            _mapper
                .Setup(c => c.Map(It.IsAny<IPagedList<Complaint>>(), It.IsAny<PagedResponse<ComplaintDetailDTO>>()))
                .Returns(response);

            var result = await _complaintService.GetComplaintsAsync(searchRequest);

            Assert.True(result.IsSuccess);
            Assert.Equal(complaintDtos.Count(), result.Results.Count());
        }

        [Fact]
        public async Task GetUserComplaintsAsync_Should_ReturnUserComplaints()
        {
            var complaints = GetComplaints();
            var complaintDtos = GetComplaintDetailsDTO(complaints);

            var searchRequest = new SearchRequest();
            var paged = new PagedList<Complaint>()
            {
                TotalPages = 1,
                CurrentPage = searchRequest.PageNumber,
                TotalCount = complaints.Count(),
                PageSize = searchRequest.PageSize,
            };

            var response = new PagedResponse<ComplaintDTO>()
            {
                TotalPages = 1,
                CurrentPage = searchRequest.PageNumber,
                TotalCount = complaints.Count(),
                PageSize = searchRequest.PageSize,
                Results = GetComplaintsDTO(complaints)
            };

            paged.AddRange(complaints);

            _userContext
                .Setup(c => c.GetUserId())
                .Returns(1);

            _complaintRepository
               .Setup(c => c.GetQueryable())
               .Returns(complaints.AsQueryable());

            _complaintRepository
                .Setup(c => c.GetPagedByAsync(It.IsAny<IQueryable<Complaint>>(), searchRequest.PageNumber, searchRequest.PageSize))
                .ReturnsAsync(paged);

            _mapper
                .Setup(c => c.Map(It.IsAny<IPagedList<Complaint>>(), It.IsAny<PagedResponse<ComplaintDTO>>()))
                .Returns(response);

            var result = await _complaintService.GetUserComplaintsAsync(searchRequest);

            Assert.True(result.IsSuccess);
            Assert.Equal(complaintDtos.Count(), result.Results.Count());
            _userContext.Verify(c => c.GetUserId(), Times.Once());
        }

        [Fact]
        public async Task GetComplaintAsync_Should_ReturnComplaint()
        {
            var complaint = new Complaint
            {
                Id = 1,
                PlateNumber = "112233",
                UserId = 1,
                Description = "asd",
                RideId = 1,
                PictureId = 1,
                CreateTime = DateTimeOffset.UtcNow
            };

            var complaintDto = new ComplaintDetailDTO
            {
                Id = 1,
                PlateNumber = "112233",
                UserId = 1,
                Description = "asd",
                RideId = 1,
                PictureId = 1,
                CreateTime = DateTimeOffset.UtcNow
            };

            _complaintRepository
                .Setup(c => c.GetAsync(It.IsAny<int>(), false))
                .ReturnsAsync(complaint);

            _mapper
                .Setup(c => c.Map<ComplaintDTO>(complaint))
                .Returns(complaintDto);

            var result = await _complaintService.GetComplaintAsync(1);

            Assert.True(result.IsSuccess);
            Assert.Equal(complaintDto, result.Result);
        }


        [Fact]
        public async Task GetComplaintAsync_Should_ReturnComplaintNotFound()
        {
            var complaint = new Complaint
            {
                Id = 1,
                PlateNumber = "112233",
                UserId = 1,
                Description = "asd",
                RideId = 1,
                PictureId = 1,
                CreateTime = DateTimeOffset.UtcNow
            };

            var complaintDto = new ComplaintDetailDTO
            {
                Id = 1,
                PlateNumber = "112233",
                UserId = 1,
                Description = "asd",
                RideId = 1,
                PictureId = 1,
                CreateTime = DateTimeOffset.UtcNow
            };

            _complaintRepository
                .Setup(c => c.GetAsync(It.IsAny<int>(), false))
                .ReturnsAsync((Complaint)null);

            var result = await _complaintService.GetComplaintAsync(1);

            Assert.False(result.IsSuccess);
            Assert.Equal(1, result.Errors.Count);
            Assert.Equal("Nie znaleziono skargi", result.Errors.First().Message);
        }

        [Fact]
        public async Task CreateComplaintAsync_Should_AddComplaint()
        {
            var createComplaintRequest = new CreateComplaintRequest
            {
                Description = "asdasd",
                RideId = 1
            };

            var complaintDto = new ComplaintDTO
            {
                Id = 1,
                PlateNumber = "112233",
                Description = "asd",
                RideId = 1,
                PictureId = 1,
                CreateTime = DateTimeOffset.UtcNow
            };

            Response<GetRideResult> responseMock = new ResponseMock<GetRideResult>(new GetRideResultMock
            {
                Id = 1,
                PictureId = 1,
                PlateNumber = "112233",
                RideDateTime = DateTimeOffset.UtcNow,
                UserId = 1
            });

            var responseTaskMock = Task.FromResult(responseMock);
            var notFoundResponseTaskMock = Task.Run<Response<GetRideNotFound>>(async () => { throw new InvalidOperationException(); });

            _userContext
               .Setup(c => c.GetUserId())
               .Returns(1);

            _requestClient
                .Setup(c => c.GetResponse<GetRideResult, GetRideNotFound>(It.IsAny<object>(), default, RequestTimeout.After(null, null, 5, null, null)))
                .ReturnsAsync((responseTaskMock, notFoundResponseTaskMock));

            _complaintRepository
                .Setup(c => c.AddAsync(It.IsAny<Complaint>()));

            _mapper
                .Setup(c => c.Map<ComplaintDTO>(It.IsAny<Complaint>()))
                .Returns(complaintDto);

            var result = await _complaintService.CreateComplaintAsync(createComplaintRequest);

            Assert.True(result.IsSuccess);
            Assert.Equal(complaintDto, result.Result);
            _complaintRepository.Verify(c => c.AddAsync(It.IsAny<Complaint>()), Times.Once());
        }

        [Fact]
        public async Task CreateComplaintAsync_Should_ReturnRideNotFound()
        {
            var createComplaintRequest = new CreateComplaintRequest
            {
                Description = "asdasd",
                RideId = 1
            };

            Response<GetRideResult> responseMock = new ResponseMock<GetRideResult>(new GetRideResultMock
            {
                Id = 1,
                PictureId = 1,
                PlateNumber = "112233",
                RideDateTime = DateTimeOffset.UtcNow,
                UserId = 1
            });

            Response<GetRideNotFound> notFoundResponseMock = new ResponseMock<GetRideNotFound>(new GetRideNotFoundMock
            {
                RideId = 1
            });

            var responseTaskMock = Task.FromResult(responseMock);
            var notFoundResponseTaskMock = Task.FromResult(notFoundResponseMock);
            
            _requestClient
                .Setup(c => c.GetResponse<GetRideResult, GetRideNotFound>(It.IsAny<object>(), default, default))
                .ReturnsAsync((responseTaskMock, notFoundResponseTaskMock));

            var result = await _complaintService.CreateComplaintAsync(createComplaintRequest);

            Assert.False(result.IsSuccess);
            Assert.Equal(1, result.Errors.Count);
            Assert.Equal("Nie znaleziono przejazdu", result.Errors.First().Message);
        }

        [Fact]
        public async Task CreateComplaintAsync_Should_ReturnUserIdEmpty()
        {
            var createComplaintRequest = new CreateComplaintRequest
            {
                Description = "asdasd",
                RideId = 1
            };

            Response<GetRideResult> responseMock = new ResponseMock<GetRideResult>(new GetRideResultMock
            {
                Id = 1,
                PictureId = 1,
                PlateNumber = "112233",
                RideDateTime = DateTimeOffset.UtcNow,
                UserId = null
            });

            var responseTaskMock = Task.FromResult(responseMock);
            var notFoundResponseTaskMock = Task.Run<Response<GetRideNotFound>>(async () => { throw new InvalidOperationException(); });

            _requestClient
                .Setup(c => c.GetResponse<GetRideResult, GetRideNotFound>(It.IsAny<object>(), default, RequestTimeout.After(null, null, 5, null, null)))
                .ReturnsAsync((responseTaskMock, notFoundResponseTaskMock));

            var result = await _complaintService.CreateComplaintAsync(createComplaintRequest);

            Assert.False(result.IsSuccess);
            Assert.Equal(1, result.Errors.Count);
            Assert.Equal("User id jest pusty dla zarejestrowanego przejazdu", result.Errors.First().Message);
        }

        [Fact]
        public async Task RemoveComplaintAsync_Should_RemoveComplaint()
        {
            var complaint = new Complaint
            {
                Id = 1,
                PlateNumber = "112233",
                UserId = 1,
                Description = "asd",
                RideId = 1,
                PictureId = 1,
                CreateTime = DateTimeOffset.UtcNow
            };

            _userContext
               .Setup(c => c.GetUserId())
               .Returns(1);

            _complaintRepository
                .Setup(c => c.GetAsync(It.IsAny<int>(), false))
                .ReturnsAsync(complaint);

            _complaintRepository
                .Setup(c => c.RemoveAsync(It.IsAny<Complaint>()));

            var result = await _complaintService.RemoveComplaintAsync(1);

            Assert.True(result.IsSuccess);
            _complaintRepository.Verify(c => c.RemoveAsync(It.IsAny<Complaint>()), Times.Once());
        }

        [Fact]
        public async Task RemoveComplaintAsync_Should_ReturnComplaintNotFound()
        {
            _complaintRepository
                .Setup(c => c.GetAsync(It.IsAny<int>(), false))
                .ReturnsAsync((Complaint)null);

            var result = await _complaintService.RemoveComplaintAsync(1);

            Assert.False(result.IsSuccess);
            Assert.Equal("Nie znaleziono skargi", result.Errors.First().Message);
            _complaintRepository.Verify(c => c.RemoveAsync(It.IsAny<Complaint>()), Times.Never());
        }

        [Fact]
        public async Task RemoveComplaintAsync_Should_ReturnUserIdNotMatch()
        {
            var complaint = new Complaint
            {
                Id = 1,
                PlateNumber = "112233",
                UserId = 1,
                Description = "asd",
                RideId = 1,
                PictureId = 1,
                CreateTime = DateTimeOffset.UtcNow
            };

            _userContext
              .Setup(c => c.GetUserId())
              .Returns(2);

            _complaintRepository
                .Setup(c => c.GetAsync(It.IsAny<int>(), false))
                .ReturnsAsync(complaint);

            var result = await _complaintService.RemoveComplaintAsync(1);

            Assert.False(result.IsSuccess);
            Assert.Equal("User id nie pasuje", result.Errors.First().Message);
            _complaintRepository.Verify(c => c.RemoveAsync(It.IsAny<Complaint>()), Times.Never());
        }

        [Fact]
        public async Task UpdateComplaintAsync_Should_UpdateComplaint()
        {
            var request = new UpdateComplaintRequest
            {
                Id = 1,
                Description = "123"
            };

            var complaint = new Complaint
            {
                Id = 1,
                PlateNumber = "112233",
                UserId = 1,
                Description = "asd",
                RideId = 1,
                PictureId = 1,
                CreateTime = DateTimeOffset.UtcNow
            };

            var complaintDto = new ComplaintDTO
            {
                Id = 1,
                PlateNumber = "112233",
                Description = "asd",
                RideId = 1,
                PictureId = 1,
                CreateTime = DateTimeOffset.UtcNow
            };

            _userContext
               .Setup(c => c.GetUserId())
               .Returns(1);

            _complaintRepository
                .Setup(c => c.GetAsync(It.IsAny<int>(), false))
                .ReturnsAsync(complaint);

            _mapper
                .Setup(c => c.Map<ComplaintDTO>(It.IsAny<Complaint>()))
                .Returns(complaintDto);

            _complaintRepository
                .Setup(c => c.UpdateAsync(It.IsAny<Complaint>()));

            var result = await _complaintService.UpdateComplaintAsync(request);

            Assert.True(result.IsSuccess);
            Assert.Equal(complaintDto, result.Result);
            _complaintRepository.Verify(c => c.UpdateAsync(It.IsAny<Complaint>()), Times.Once());
        }

        [Fact]
        public async Task UpdateComplaintAsync_Should_ReturnComplaintNotFound()
        {
            var request = new UpdateComplaintRequest
            {
                Id = 1,
                Description = "123"
            };

            _complaintRepository
                .Setup(c => c.GetAsync(It.IsAny<int>(), false))
                .ReturnsAsync((Complaint)null);

            var result = await _complaintService.UpdateComplaintAsync(request);

            Assert.False(result.IsSuccess);
            Assert.Equal("Nie znaleziono skargi", result.Errors.First().Message);
            _complaintRepository.Verify(c => c.UpdateAsync(It.IsAny<Complaint>()), Times.Never());
        }

        [Fact]
        public async Task UpdateComplaintAsync_Should_ReturnUserIdNotMatch()
        {
            var request = new UpdateComplaintRequest
            {
                Id = 1,
                Description = "123"
            };

            var complaint = new Complaint
            {
                Id = 1,
                PlateNumber = "112233",
                UserId = 1,
                Description = "asd",
                RideId = 1,
                PictureId = 1,
                CreateTime = DateTimeOffset.UtcNow
            };

            _userContext
              .Setup(c => c.GetUserId())
              .Returns(2);

            _complaintRepository
                .Setup(c => c.GetAsync(It.IsAny<int>(), false))
                .ReturnsAsync(complaint);

            var result = await _complaintService.UpdateComplaintAsync(request);

            Assert.False(result.IsSuccess);
            Assert.Equal("User id nie pasuje", result.Errors.First().Message);
            _complaintRepository.Verify(c => c.UpdateAsync(It.IsAny<Complaint>()), Times.Never());
        }
    }
}
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
