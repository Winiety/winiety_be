using Contracts.Events;
using System;
using System.Threading.Tasks;

namespace Rides.Core.Services
{
    public interface IRideService
    {
        Task RegisterRide(CarRegistered car);
    }

    public class RideService : IRideService
    {
        public async Task RegisterRide(CarRegistered car)
        {
            Console.WriteLine($"Car registered. Plates: {car.PlateNumber}, Picture Id: {car.PictureId}");
        }
    }
}
