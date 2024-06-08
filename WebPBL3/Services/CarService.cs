using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebPBL3.DTO;
using WebPBL3.Models;

namespace WebPBL3.Services
{
    public class CarService : ICarService
    {
        private readonly ApplicationDbContext _db;
        private IWebHostEnvironment _environment;
        public CarService(ApplicationDbContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }

        public async Task AddCar(Car car)
        {
            try
            {
                _db.Cars.Add(car);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi xuất hiện khi thêm xe: ", ex);
            }
        }

        public Car ConvertToCar(CarDTO cardto)
        {   
            try
            {
                Car car = new Car
                {
                    CarID = cardto.CarID,
                    CarName = cardto.CarName,
                    Photo = cardto.Photo,

                    Capacity = cardto.Capacity,
                    FuelConsumption = cardto.FuelConsumption,
                    Color = cardto.Color,

                    Description = cardto.Description,
                    Dimension = cardto.Dimension,
                    Engine = cardto.Engine,

                    Origin = cardto.Origin,
                    Price = cardto.Price,
                    Quantity = cardto.Quantity,

                    Seat = cardto.Seat,
                    Topspeed = cardto.Topspeed,
                    Year = cardto.Year,

                    MakeID = cardto.MakeID,

                };
                return car;
            } catch (Exception ex)
            {
                throw new Exception("Lỗi trong khi chuyển đổi CarDTO thành Car: ", ex);
            }
            

        }

        public CarDTO ConvertToCarDTO(Car car)
        {
            try
            {
                CarDTO cardto = new CarDTO
                {   
                    CarID = car.CarID,
                    CarName = car.CarName,
                    Photo = car.Photo,
                    Capacity = car.Capacity,
                    FuelConsumption = car.FuelConsumption,
                    Color = car.Color,
                    Description = car.Description,
                    Dimension = car.Dimension,
                    Engine = car.Engine,
                    Origin = car.Origin,
                    Price = car.Price,
                    Quantity = car.Quantity,
                    Seat = car.Seat,
                    Topspeed = car.Topspeed,
                    Year = car.Year,
                    MakeID = car.MakeID,
                };
                return cardto;
            
            } catch (Exception ex)
            {
                throw new Exception("Lỗi trong khi chuyển đổi Car thành CarDTO: ", ex);
            }
        }

        public async Task DeleteCar(Car car)
        {
            try
            {
                car.Flag = true;
                _db.Cars.Update(car);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi xuất hiện khi xóa xe: ", ex);
            }
        }

        public async Task EditCar(Car car)
        {
            try
            {
                _db.Cars.Update(car);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi xuất hiện khi cập nhật xe: ", ex);
            }
        }

        public async Task<IEnumerable<CarDTO>> GetAllCars(int makeid, string searchtxt, int page)
        {   
            try
            {
                List<CarDTO> cars = await _db.Cars
                .Include(c => c.Make)
                .OrderBy(c => c.CarID)
                .Where(c => c.Flag == false && (makeid == 0 || c.MakeID == makeid) && (searchtxt.IsNullOrEmpty() || c.CarName.Contains(searchtxt)))
                .Select(c => new CarDTO
                {
                    CarID = c.CarID,
                    CarName = c.CarName,
                    Photo = c.Photo,

                    Capacity = c.Capacity,
                    FuelConsumption = c.FuelConsumption,
                    Color = c.Color,

                    Description = c.Description,
                    Dimension = c.Dimension,
                    Engine = c.Engine,

                    Origin = c.Origin,
                    Price = c.Price,
                    Quantity = c.Quantity,

                    Seat = c.Seat,
                    Topspeed = c.Topspeed,
                    Year = c.Year,

                    MakeID = c.MakeID,
                    MakeName = c.Make.MakeName,
                }).ToListAsync();
                return cars;
            } catch (Exception ex)
            {
                throw new Exception("Lỗi trong khi lấy danh sách xe: ", ex);
            }
            
            
        }

        public async Task<Car> GetCarById(string id)
        {
            try
            {
                Car? car = await _db.Cars.FirstOrDefaultAsync(u => u.CarID == id);
                return car;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi xảy ra khi truy vấn xe: ", ex);
            }
        }
    }
}
