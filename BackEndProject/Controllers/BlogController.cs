using Microsoft.AspNetCore.Mvc;

namespace BackEndProject.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDb _appDb;

        public BlogController(AppDb appDb)
        {
            _appDb = appDb;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail()
        {
            List<Blog> blogs = _appDb.Blogs.ToList();

            BlogVM blogVM = new()
            {
                blog = blogs
            };

            return View(blogVM);
        }
    }
}
