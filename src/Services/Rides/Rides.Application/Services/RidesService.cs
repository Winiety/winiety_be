using Contracts.Events;
using System;
using System.Threading.Tasks;

namespace Rides.Application.Services
{
    public interface IRidesService
    {
        Task RegisterRide(CarRegistered car);
    }

    public class RidesService : IRidesService
    {
        public async Task RegisterRide(CarRegistered car)
        {
            Console.WriteLine($"Car registered. Plates: {car.PlateNumber}, Picture Id: {car.PictureId}");
        }
    }
}
