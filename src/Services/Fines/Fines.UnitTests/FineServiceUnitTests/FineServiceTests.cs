using Contracts.Results;
using Fines.Core.Model.DTOs.Fine;
using Fines.Core.Model.Entities;
using Fines.Core.Model.Requests.Fine;
using MassTransit;
using MassTransit.Testing;
using Moq;
using Shared.Core.BaseModels.Requests;
using Shared.Core.BaseModels.Responses;
using Shared.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Fines.UnitTests.FineServiceUnitTests
{
    public class FineServiceTests : FineServiceSetup
    {
        [Fact]
        public async Task GetFinesAsync_Should_ReturnFines()
        {
            var fines = GetFines();
            var fineDtos = GetFineDetailsDTO(fines);

            var searchRequest = new SearchRequest();
            var paged = new PagedList<Fine>()
            {
                TotalPages = 1,
                CurrentPage = searchRequest.PageNumber,
                TotalCount = fines.Count(),
                PageSize = searchRequest.PageSize,
            };

            var response = new PagedResponse<FineDetailDTO>()
            {
                TotalPages = 1,
                CurrentPage = searchRequest.PageNumber,
                TotalCount = fines.Count(),
                PageSize = searchRequest.PageSize,
                Results = GetFineDetailsDTO(fines)
            };

            paged.AddRange(fines);

            _userContext
                .Setup(c => c.GetUserId())
                .Returns(1);

            _fineRepository
               .Setup(c => c.GetQueryable())
               .Returns(fines.AsQueryable());

            _fineRepository
                .Setup(c => c.GetPagedByAsync(It.IsAny<IQueryable<Fine>>(), searchRequest.PageNumber, searchRequest.PageSize))
                .ReturnsAsync(paged);

            _mapper
                .Setup(c => c.Map(It.IsAny<IPagedList<Fine>>(), It.IsAny<PagedResponse<FineDetailDTO>>()))
                .Returns(response);

            var result = await _fineService.GetFinesAsync(searchRequest);

            Assert.True(result.IsSuccess);
            Assert.Equal(fineDtos.Count(), result.Results.Count());
        }

        [Fact]
        public async Task GetUserFinesAsync_Should_ReturnUserFines()
        {
            var fines = GetFines();
            var fineDtos = GetFineDetailsDTO(fines);

            var searchRequest = new SearchRequest();
            var paged = new PagedList<Fine>()
            {
                TotalPages = 1,
                CurrentPage = searchRequest.PageNumber,
                TotalCount = fines.Count(),
                PageSize = searchRequest.PageSize,
            };

            var response = new PagedResponse<FineDTO>()
            {
                TotalPages = 1,
                CurrentPage = searchRequest.PageNumber,
                TotalCount = fines.Count(),
                PageSize = searchRequest.PageSize,
                Results = GetFinesDTO(fines)
            };

            paged.AddRange(fines);

            _userContext
                .Setup(c => c.GetUserId())
                .Returns(1);

            _fineRepository
               .Setup(c => c.GetQueryable())
               .Returns(fines.AsQueryable());

            _fineRepository
                .Setup(c => c.GetPagedByAsync(It.IsAny<IQueryable<Fine>>(), searchRequest.PageNumber, searchRequest.PageSize))
                .ReturnsAsync(paged);

            _mapper
                .Setup(c => c.Map(It.IsAny<IPagedList<Fine>>(), It.IsAny<PagedResponse<FineDTO>>()))
                .Returns(response);

            var result = await _fineService.GetUserFinesAsync(searchRequest);

            Assert.True(result.IsSuccess);
            Assert.Equal(fineDtos.Count(), result.Results.Count());
            _userContext.Verify(c => c.GetUserId(), Times.Once());
        }

        [Fact]
        public async Task GetFineAsync_Should_ReturnFine()
        {
            var fine = new Fine
            {
                Id = 1,
                PlateNumber = "112233",
                UserId = 1,
                Cost = 1,
                Description = "asd",
                RideId = 1,
                PictureId = 1,
                CreateTime = DateTimeOffset.UtcNow
            };

            var fineDto = new FineDetailDTO
            {
                Id = 1,
                PlateNumber = "112233",
                UserId = 1,
                Cost = 1,
                Description = "asd",
                RideId = 1,
                PictureId = 1,
                CreateTime = DateTimeOffset.UtcNow
            };

            _fineRepository
                .Setup(c => c.GetAsync(It.IsAny<int>(), false))
                .ReturnsAsync(fine);

            _mapper
                .Setup(c => c.Map<FineDTO>(fine))
                .Returns(fineDto);

            var result = await _fineService.GetFineAsync(1);

            Assert.True(result.IsSuccess);
            Assert.Equal(fineDto, result.Result);
        }


        [Fact]
        public async Task GetFineAsync_Should_ReturnFineNotFound()
        {
            var fine = new Fine
            {
                Id = 1,
                PlateNumber = "112233",
                UserId = 1,
                Cost = 1,
                Description = "asd",
                RideId = 1,
                PictureId = 1,
                CreateTime = DateTimeOffset.UtcNow
            };

            var fineDto = new FineDetailDTO
            {
                Id = 1,
                PlateNumber = "112233",
                UserId = 1,
                Cost = 1,
                Description = "asd",
                RideId = 1,
                PictureId = 1,
                CreateTime = DateTimeOffset.UtcNow
            };

            _fineRepository
                .Setup(c => c.GetAsync(It.IsAny<int>(), false))
                .ReturnsAsync((Fine)null);

            var result = await _fineService.GetFineAsync(1);

            Assert.False(result.IsSuccess);
            Assert.Equal(1, result.Errors.Count);
            Assert.Equal("Fine not found", result.Errors.First().Message);
        }

        [Fact]
        public async Task CreateFineAsync_Should_AddFine()
        {
            var createFineRequest = new CreateFineRequest
            {
                Cost = 1,
                Description = "asdasd",
                RideId = 1
            };

            var fineDto = new FineDTO
            {
                Id = 1,
                PlateNumber = "112233",
                Cost = 1,
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
            
            _requestClient
                .Setup(c => c.GetResponse<GetRideResult, GetRideNotFound>(It.IsAny<object>(), default, default))
                .ReturnsAsync((responseTaskMock, notFoundResponseTaskMock));

            _fineRepository
                .Setup(c => c.AddAsync(It.IsAny<Fine>()));

            _mapper
                .Setup(c => c.Map<FineDTO>(It.IsAny<Fine>()))
                .Returns(fineDto);

            var result = await _fineService.CreateFineAsync(createFineRequest);

            Assert.True(result.IsSuccess);
            Assert.Equal(fineDto, result.Result);
            _fineRepository.Verify(c => c.AddAsync(It.IsAny<Fine>()), Times.Once());
        }

        [Fact]
        public async Task CreateFineAsync_Should_ReturnRideNotFound()
        {
            var createFineRequest = new CreateFineRequest
            {
                Cost = 1,
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

            var result = await _fineService.CreateFineAsync(createFineRequest);

            Assert.False(result.IsSuccess);
            Assert.Equal(1, result.Errors.Count);
            Assert.Equal("Ride not found", result.Errors.First().Message);
        }

        [Fact]
        public async Task CreateFineAsync_Should_ReturnUserIdEmpty()
        {
            var createFineRequest = new CreateFineRequest
            {
                Cost = 1,
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
                .Setup(c => c.GetResponse<GetRideResult, GetRideNotFound>(It.IsAny<object>(), default, default))
                .ReturnsAsync((responseTaskMock, notFoundResponseTaskMock));

            var result = await _fineService.CreateFineAsync(createFineRequest);

            Assert.False(result.IsSuccess);
            Assert.Equal(1, result.Errors.Count);
            Assert.Equal("User id is empty in registered ride", result.Errors.First().Message);
        }

        [Fact]
        public async Task RemoveFineAsync_Should_RemoveFine()
        {
            var fine = new Fine
            {
                Id = 1,
                PlateNumber = "112233",
                UserId = 1,
                Cost = 1,
                Description = "asd",
                RideId = 1,
                PictureId = 1,
                CreateTime = DateTimeOffset.UtcNow
            };

            _fineRepository
                .Setup(c => c.GetAsync(It.IsAny<int>(), false))
                .ReturnsAsync(fine);

            _fineRepository
                .Setup(c => c.RemoveAsync(It.IsAny<Fine>()));

            var result = await _fineService.RemoveFineAsync(1);

            Assert.True(result.IsSuccess);
            _fineRepository.Verify(c => c.RemoveAsync(It.IsAny<Fine>()), Times.Once());
        }

        [Fact]
        public async Task RemoveFineAsync_Should_ReturnFineNotFound()
        {
            _fineRepository
                .Setup(c => c.GetAsync(It.IsAny<int>(), false))
                .ReturnsAsync((Fine)null);

            var result = await _fineService.RemoveFineAsync(1);

            Assert.False(result.IsSuccess);
            Assert.Equal("Fine not found", result.Errors.First().Message);
            _fineRepository.Verify(c => c.RemoveAsync(It.IsAny<Fine>()), Times.Never());
        }

        [Fact]
        public async Task UpdateFineAsync_Should_UpdateFine()
        {
            var request = new UpdateFineRequest
            {
                Id = 1,
                Cost = 123,
                Description = "123"
            };

            var fine = new Fine
            {
                Id = 1,
                PlateNumber = "112233",
                UserId = 1,
                Cost = 1,
                Description = "asd",
                RideId = 1,
                PictureId = 1,
                CreateTime = DateTimeOffset.UtcNow
            };

            var fineDto = new FineDTO
            {
                Id = 1,
                PlateNumber = "112233",
                Cost = 1,
                Description = "asd",
                RideId = 1,
                PictureId = 1,
                CreateTime = DateTimeOffset.UtcNow
            };

            _fineRepository
                .Setup(c => c.GetAsync(It.IsAny<int>(), false))
                .ReturnsAsync(fine);

            _mapper
                .Setup(c => c.Map<FineDTO>(It.IsAny<Fine>()))
                .Returns(fineDto);

            _fineRepository
                .Setup(c => c.UpdateAsync(It.IsAny<Fine>()));

            var result = await _fineService.UpdateFineAsync(request);

            Assert.True(result.IsSuccess);
            Assert.Equal(fineDto, result.Result);
            _fineRepository.Verify(c => c.UpdateAsync(It.IsAny<Fine>()), Times.Once());
        }

        [Fact]
        public async Task UpdateFineAsync_Should_ReturnFineNotFound()
        {
            var request = new UpdateFineRequest
            {
                Id = 1,
                Cost = 123,
                Description = "123"
            };

            _fineRepository
                .Setup(c => c.GetAsync(It.IsAny<int>(), false))
                .ReturnsAsync((Fine)null);

            var result = await _fineService.UpdateFineAsync(request);

            Assert.False(result.IsSuccess);
            Assert.Equal("Fine not found", result.Errors.First().Message);
            _fineRepository.Verify(c => c.UpdateAsync(It.IsAny<Fine>()), Times.Never());
        }
    }
}
