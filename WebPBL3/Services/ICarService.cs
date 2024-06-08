using NuGet.Packaging.Signing;
using WebPBL3.DTO;
using WebPBL3.Models;

namespace WebPBL3.Services
{
    public interface ICarService
    {
        Task<IEnumerable<CarDTO>> GetAllCars(int makeid, string searchtxt, int page);
        Task<Car> GetCarById(string id);
        Task AddCar(Car car);
        Task EditCar (Car car);
        Task DeleteCar(Car car);

        CarDTO ConvertToCarDTO(Car car);
        Car ConvertToCar(CarDTO cardto);


    }
}
