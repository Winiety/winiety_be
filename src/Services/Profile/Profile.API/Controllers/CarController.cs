using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile.Core.Models.DTOs.Car;
using Profile.Core.Models.Requests.Car;
using Profile.Core.Services;
using Shared.Core.BaseModels.Requests;
using Shared.Core.BaseModels.Responses.Interfaces;

namespace Profile.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        public async Task<ActionResult<ICollectionResponse<CarDTO>>> GetCars()
        {
            var response = await _carService.GetCarsAsync();
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IPagedResponse<CarDTO>>> SearchCars([FromQuery] SearchRequest search)
        {
            var response = await _carService.SearchCarsAsync(search);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<IResultResponse<CarDTO>>> CreateCar([FromBody] CreateCarRequest car)
        {
            var response = await _carService.CreateCarAsync(car);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<IResultResponse<CarDTO>>> UpdateCar([FromBody] UpdateCarRequest car)
        {
            var response = await _carService.UpdateCarAsync(car);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<IBaseResponse>> DeleteCar(int id)
        {
            var response = await _carService.RemoveCarAsync(id);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
