﻿using Microsoft.AspNetCore.Mvc;

namespace BackEndProject.Controllers
{
    public class EventController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
