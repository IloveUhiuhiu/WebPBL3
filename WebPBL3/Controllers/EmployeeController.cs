﻿using Microsoft.AspNetCore.Mvc;

namespace WebPBL3.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
