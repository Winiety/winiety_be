using AutoMapper;
using Profile.Core.Interfaces;
using Profile.Core.Models.DTOs.Car;
using Profile.Core.Models.Entities;
using Profile.Core.Models.Requests.Car;
using Shared.Core.BaseModels.Requests;
using Shared.Core.BaseModels.Responses;
using Shared.Core.BaseModels.Responses.Interfaces;
using Shared.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Profile.Core.Services
{
    public interface ICarService
    {
        Task<IResultResponse<CarDTO>> CreateCarAsync(CreateCarRequest car);
        Task<IResultResponse<CarDTO>> UpdateCarAsync(UpdateCarRequest car);
        Task<ICollectionResponse<CarDTO>> GetCarsAsync();
        Task<IPagedResponse<CarDTO>> SearchCarsAsync(SearchRequest search);
        Task<IBaseResponse> RemoveCarAsync(int carId);
        Task<int?> GetUserIdByPlateAsync(string plateNumber);
    }

    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;
        private readonly IUserContext _userContext;
        private readonly IMapper _mapper;

        public CarService(ICarRepository carRepository, IUserContext userContext, IMapper mapper)
        {
            _carRepository = carRepository;
            _userContext = userContext;
            _mapper = mapper;
        }

        public async Task<IResultResponse<CarDTO>> CreateCarAsync(CreateCarRequest car)
        {
            var response = new ResultResponse<CarDTO>();

            var carEntity = await _carRepository.GetByAsync(c => c.PlateNumber == car.PlateNumber);

            if (carEntity != null)
            {
                response.AddError(new Error
                {
                    Message = "Car with given plate number already exists"
                });

                return response;
            }

            carEntity = _mapper.Map<Car>(car);

            var currentUserId = _userContext.GetUserId();
            carEntity.UserId = currentUserId;

            await _carRepository.AddAsync(carEntity);

            response.Result = _mapper.Map<CarDTO>(carEntity);

            return response;
        }

        public async Task<IResultResponse<CarDTO>> UpdateCarAsync(UpdateCarRequest car)
        {
            var response = new ResultResponse<CarDTO>();

            var actualCarEntity = await _carRepository.GetAsync(car.Id);

            if (actualCarEntity == null)
            {
                response.AddError(new Error
                {
                    Message = "Car not found"
                });

                return response;
            }

            var carEntity = await _carRepository.GetByAsync(c => c.PlateNumber == car.PlateNumber);

            if (actualCarEntity.PlateNumber != carEntity.PlateNumber)
            {
                response.AddError(new Error
                {
                    Message = "Car with given plate number already exists"
                });

                return response;
            }

            actualCarEntity = _mapper.Map(car, actualCarEntity);

            await _carRepository.UpdateAsync(actualCarEntity);

            response.Result = _mapper.Map<CarDTO>(actualCarEntity);

            return response;
        }

        public async Task<IBaseResponse> RemoveCarAsync(int carId)
        {
            var response = new BaseResponse();

            var car = await _carRepository.GetAsync(carId);

            if (car == null)
            {
                response.AddError(new Error
                {
                    Message = "Car not found"
                });

                return response;
            }

            var currentUserId = _userContext.GetUserId();

            if (car.UserId != currentUserId)
            {
                response.AddError(new Error
                {
                    Message = "User id does not match"
                });

                return response;
            }

            await _carRepository.RemoveAsync(car);

            return response;
        }

        public async Task<ICollectionResponse<CarDTO>> GetCarsAsync()
        {
            var response = new CollectionResponse<CarDTO>();

            var currentUserId = _userContext.GetUserId();

            var result = await _carRepository.GetAllByAsync(c => c.UserId == currentUserId);

            response.Results = _mapper.Map<IEnumerable<CarDTO>>(result);

            return response;
        }

        public async Task<IPagedResponse<CarDTO>> SearchCarsAsync(SearchRequest search)
        {
            var response = new PagedResponse<CarDTO>();

            var cars = await _carRepository.GetPagedByAsync(
                CreateSearchExpression(search),
                search.PageNumber,
                search.PageSize);

            response = _mapper.Map(cars, response);

            return response;
        }

        public async Task<int?> GetUserIdByPlateAsync(string plateNumber)
        {
            var result = await _carRepository.GetByAsync(c => c.PlateNumber == plateNumber);

            return result?.UserId;
        }

        private Expression<Func<Car, bool>> CreateSearchExpression(SearchRequest search)
        {
            var currentUserId = _userContext.GetUserId();
            var query = search.Query;

            return c => c.UserId == currentUserId &&
                (c.PlateNumber.Contains(query) ||
                 c.Model.Contains(query) ||
                 c.Brand.Contains(query) ||
                 c.Color.Contains(query) ||
                 c.Year.Contains(query));
        }
    }
}
