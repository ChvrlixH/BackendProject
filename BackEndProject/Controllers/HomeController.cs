using Microsoft.AspNetCore.Mvc;

namespace BackEndProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDb _appDb;

        public HomeController(AppDb appDb)
        {
            _appDb = appDb;
        }

        public IActionResult Index()
        {
            IEnumerable<Event> events = _appDb.Events.OrderByDescending(e=>e.Id).Take(4).ToList();
            IEnumerable<Course> courses = _appDb.Courses.OrderByDescending(c=>c.Id).Take(3).ToList();
            IEnumerable<Blog> blogs = _appDb.Blogs.OrderByDescending(b=>b.Id).Take(3).ToList();

            HomeVM homeVM = new()
            {
                Events= events,
                Courses = courses,
                Blogs = blogs
            };

            return View(homeVM);
        }
    }
}
