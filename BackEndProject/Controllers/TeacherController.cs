using Microsoft.AspNetCore.Mvc;

namespace BackEndProject.Controllers
{
    public class TeacherController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
