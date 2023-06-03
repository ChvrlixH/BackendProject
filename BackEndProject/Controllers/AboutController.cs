using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEndProject.Controllers
{
    public class AboutController : Controller
    {
        private readonly AppDb _appDb;

        public AboutController(AppDb appDb)
        {
            _appDb = appDb;
        }

        public IActionResult Index()
        {
            IEnumerable<Teacher> teachers = _appDb.Teachers.Include(t => t.SocialMedias).Include(t => t.Skills).OrderByDescending(b => b.Id).Take(4).ToList();
            return View(teachers);
        }
    }
}
