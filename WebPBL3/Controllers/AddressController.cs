using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPBL3.Models;

namespace WebPBL3.Controllers
{
    public class AddressController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AddressController(ApplicationDbContext db)
        {
            _context = db;
        }
        public JsonResult GetProvince()
        {
            var provinces = _context.Provinces.ToList();
            return new JsonResult(provinces);
        }

        public JsonResult GetDistrict(int id)
        {
            var districts = _context.Districts.Where(d => d.ProvinceID == id).Select(d => new { id = d.DistrictID, name = d.DistrictName }).ToList();
            return new JsonResult(districts);
        }

        public JsonResult GetWard(int id)
        {
            var wards = _context.Wards.Where(w => w.DistrictID == id).Select(w => new { id = w.WardID, name = w.WardName }).ToList();
            return new JsonResult(wards);
        }
    }
}
