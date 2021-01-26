using MassTransit.Testing;
using Moq;
using Profile.Core.Models.DTOs.Car;
using Profile.Core.Models.Entities;
using Profile.Core.Models.Requests.Car;
using Shared.Core.BaseModels.Requests;
using Shared.Core.BaseModels.Responses;
using Shared.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Profile.UnitTests.CarServiceUnitTests
{
    public class CarServiceTests : CarServiceSetup
    {
        [Fact]
        public async Task GetCarsAsync_Should_ReturnUserCars()
        {
            var cars = GetCars();
            var carDtos = GetCarsDTO(cars);

            _userContext
                .Setup(c => c.GetUserId())
                .Returns(1);

            _carRepository
                .Setup(c => c.GetAllByAsync(It.IsAny<Expression<Func<Car, bool>>>(), false))
                .ReturnsAsync(cars);

            _mapper
                .Setup(c => c.Map<IEnumerable<CarDTO>>(It.IsAny<IEnumerable<Car>>()))
                .Returns(carDtos);

            var result = await _carService.GetCarsAsync();

            Assert.True(result.IsSuccess);
            Assert.Equal(carDtos.Count(), result.Results.Count());
            _userContext.Verify(c => c.GetUserId(), Times.Once());
        }

        [Fact]
        public async Task SearchCarsAsync_Should_ReturnUserCars()
        {
            var cars = GetCars();
            var searchRequest = new SearchRequest();
            var paged = new PagedList<Car>()
            {
                TotalPages = 1,
                CurrentPage = searchRequest.PageNumber,
                TotalCount = cars.Count(),
                PageSize = searchRequest.PageSize,
            };

            var response = new PagedResponse<CarDTO>()
            {
                TotalPages = paged.TotalPages,
                CurrentPage = searchRequest.PageNumber,
                TotalCount = paged.TotalCount,
                PageSize = searchRequest.PageSize,
                Results = GetCarsDTO(cars)
            };

            _userContext
                .Setup(c => c.GetUserId())
                .Returns(1);

            _carRepository
                .Setup(c => c.GetPagedByAsync(It.IsAny<Expression<Func<Car, bool>>>(), searchRequest.PageNumber, searchRequest.PageSize, false))
                .ReturnsAsync(paged);

            _mapper
                .Setup(c => c.Map(It.IsAny<IPagedList<Car>>(), It.IsAny<PagedResponse<CarDTO>>()))
                .Returns(response);

            var result = await _carService.SearchCarsAsync(searchRequest);

            Assert.True(result.IsSuccess);
            Assert.Equal(cars.Count(), result.TotalCount);
            _userContext.Verify(c => c.GetUserId(), Times.Once());
        }

        [Fact]
        public async Task GetUserIdByPlateAsync_Should_ReturnNull()
        {
            _carRepository
                .Setup(c => c.GetByAsync(It.IsAny<Expression<Func<Car, bool>>>(), false))
                .ReturnsAsync((Car)null);

            var result = await _carService.GetUserIdByPlateAsync("112233");

            Assert.False(result.HasValue);
        }

        [Fact]
        public async Task GetUserIdByPlateAsync_Should_ReturnUserId()
        {
            var car = new Car
            {
                UserId = 1
            };

            _carRepository
                .Setup(c => c.GetByAsync(It.IsAny<Expression<Func<Car, bool>>>(), false))
                .ReturnsAsync(car);

            var result = await _carService.GetUserIdByPlateAsync("112233");

            Assert.True(result.HasValue);
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task RemoveCarAsync_Should_RemoveCar()
        {
            var car = new Car
            {
                Id = 1,
                UserId = 1
            };

            _userContext
                .Setup(c => c.GetUserId())
                .Returns(1);

            _carRepository
                .Setup(c => c.GetAsync(It.IsAny<int>(), false))
                .ReturnsAsync(car);

            _carRepository
                .Setup(c => c.RemoveAsync(It.IsAny<Car>()));

            var result = await _carService.RemoveCarAsync(1);

            Assert.True(result.IsSuccess);
            _carRepository.Verify(c => c.RemoveAsync(It.IsAny<Car>()), Times.Once());
        }

        [Fact]
        public async Task RemoveCarAsync_Should_ReturnCarNotFound()
        {
            _userContext
                .Setup(c => c.GetUserId())
                .Returns(1);

            _carRepository
                .Setup(c => c.GetAsync(It.IsAny<int>(), false))
                .ReturnsAsync((Car)null);

            var result = await _carService.RemoveCarAsync(1);

            Assert.False(result.IsSuccess);
            Assert.Equal("Nie znaleziono samochodu", result.Errors.First().Message);
            _carRepository.Verify(c => c.RemoveAsync(It.IsAny<Car>()), Times.Never());
        }

        [Fact]
        public async Task RemoveCarAsync_Should_ReturnUserIdNotMatch()
        {
            var car = new Car
            {
                Id = 1,
                UserId = 2
            };

            _userContext
                .Setup(c => c.GetUserId())
                .Returns(1);

            _carRepository
                .Setup(c => c.GetAsync(It.IsAny<int>(), false))
                .ReturnsAsync(car);

            var result = await _carService.RemoveCarAsync(1);

            Assert.False(result.IsSuccess);
            Assert.Equal("User id nie pasuje", result.Errors.First().Message);
            _carRepository.Verify(c => c.RemoveAsync(It.IsAny<Car>()), Times.Never());
        }

        [Fact]
        public async Task UpdateCarAsync_Should_UpdateCar()
        {
            var request = new UpdateCarRequest
            {
                Id = 1,
                PlateNumber = "112233"
            };

            var car = new Car
            {
                Id = 1,
                UserId = 1,
                PlateNumber = "112233"
            };

            var carDTO = new CarDTO
            {
                Id = 1,
            };

            _carRepository
                .Setup(c => c.GetAsync(It.IsAny<int>(), false))
                .ReturnsAsync(car);

            _carRepository
                .Setup(c => c.GetByAsync(It.IsAny<Expression<Func<Car, bool>>>(), false))
                .ReturnsAsync(car);

            _mapper
                .Setup(c => c.Map(It.IsAny<UpdateCarRequest>(), It.IsAny<Car>()))
                .Returns(car);

            _mapper
                .Setup(c => c.Map<CarDTO>(It.IsAny<Car>()))
                .Returns(carDTO);

            _carRepository
                .Setup(c => c.UpdateAsync(It.IsAny<Car>()));

            var result = await _carService.UpdateCarAsync(request);

            Assert.True(result.IsSuccess);
            Assert.Equal(carDTO, result.Result);
            _carRepository.Verify(c => c.UpdateAsync(It.IsAny<Car>()), Times.Once());
        }

        [Fact]
        public async Task UpdateCarAsync_Should_ReturnCarNotFound()
        {
            var request = new UpdateCarRequest
            {
                Id = 1,
                PlateNumber="112233"
            };

            _carRepository
                .Setup(c => c.GetAsync(It.IsAny<int>(), false))
                .ReturnsAsync((Car)null);

            var result = await _carService.UpdateCarAsync(request);

            Assert.False(result.IsSuccess);
            Assert.Equal("Nie znaleziono samochodu", result.Errors.First().Message);
            _carRepository.Verify(c => c.UpdateAsync(It.IsAny<Car>()), Times.Never());
        }

        [Fact]
        public async Task CreateCarAsync_Should_CreateCar()
        {
            var request = new CreateCarRequest
            {
                PlateNumber = "112233"
            };

            var car = new Car
            {
                Id = 1,
                UserId = 1
            };

            var carDTO = new CarDTO
            {
                Id = 1,
            };

            _mapper
                .Setup(c => c.Map<Car>(It.IsAny<CreateCarRequest>()))
                .Returns(car);

            _userContext
                .Setup(c => c.GetUserId())
                .Returns(1);

            _carRepository
                .Setup(c => c.GetAsync(It.IsAny<int>(), false))
                .ReturnsAsync(car);

            _carRepository
                .Setup(c => c.AddAsync(It.IsAny<Car>()));

            _mapper
                .Setup(c => c.Map<CarDTO>(It.IsAny<Car>()))
                .Returns(carDTO);

            var result = await _carService.CreateCarAsync(request);

            Assert.True(result.IsSuccess);
            Assert.Equal(carDTO, result.Result);
            _carRepository.Verify(c => c.AddAsync(It.IsAny<Car>()), Times.Once());
        }
    }
}
