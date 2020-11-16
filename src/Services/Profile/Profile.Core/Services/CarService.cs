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
        Task<IResponse<CarDTO>> CreateCarAsync(CreateCarRequest car);
        Task<IResponse<CarDTO>> UpdateCarAsync(UpdateCarRequest car);
        Task<ICollectionResponse<CarDTO>> GetCarsAsync();
        Task<IPagedResponse<CarDTO>> SearchCarsAsync(SearchRequest search);
        Task<IBaseResponse> RemoveCarAsync(int carId);
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

        public async Task<IResponse<CarDTO>> CreateCarAsync(CreateCarRequest car)
        {
            var response = new Response<CarDTO>();

            var carEntity = _mapper.Map<Car>(car);

            var currentUserId = _userContext.GetUserId();
            carEntity.UserId = currentUserId;

            await _carRepository.AddAsync(carEntity);

            response.Result = _mapper.Map<CarDTO>(carEntity);

            return response;
        }

        public async Task<IResponse<CarDTO>> UpdateCarAsync(UpdateCarRequest car)
        {
            var response = new Response<CarDTO>();

            var carEntity = await _carRepository.GetAsync(car.Id);

            if (carEntity == null)
            {
                response.AddError(new Error
                {
                    Message = "Car not found"
                });

                return response;
            }

            _mapper.Map(car, carEntity);

            await _carRepository.UpdateAsync(carEntity);

            response.Result = _mapper.Map<CarDTO>(carEntity);

            return response;
        }

        public async Task<IBaseResponse> RemoveCarAsync(int carId)
        {
            var response = new BaseResponse();

            var car = await _carRepository.GetAsync(carId);
            var currentUserId = _userContext.GetUserId();

            if (car == null)
            {
                response.AddError(new Error
                {
                    Message = "Car not found"
                });

                return response;
            }

            if (car.UserId != currentUserId)
            {
                response.AddError(new Error
                {
                    Message = "User id does not match"
                });
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

            _mapper.Map(cars, response);

            return response;
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
