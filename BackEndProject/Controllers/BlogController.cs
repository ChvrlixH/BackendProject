using Microsoft.AspNetCore.Mvc;

namespace BackEndProject.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
