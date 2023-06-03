using BackEndProject.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            List<Blog> blogs = _appDb.Blogs.ToList();

            BlogVM blogVM = new()
            {
                blog = blogs
            };
            return View(blogVM);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) { return NotFound(); }
            Blog blogs =  await _appDb.Blogs.FirstOrDefaultAsync(b=>b.Id==id);
            if (blogs == null) { return NotFound(); }
            List<Blog> _blog = _appDb.Blogs.Where(b=>b.Id==blogs.Id).ToList();

            BlogVM blogVM = new()
            {
                blogs = blogs,
                blog = _blog
            };

            return View(blogVM);
        }
    }
}
