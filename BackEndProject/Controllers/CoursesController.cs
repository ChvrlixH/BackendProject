
using BackEndProject.Areas.Admin.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BackEndProject.Controllers
{
    public class CoursesController : Controller
    {
        private readonly AppDb _appDb;

        public CoursesController(AppDb appDb)
        {
            _appDb = appDb;
        }

        public IActionResult Index()
        {
            List<Course> courses = _appDb.Courses.Include(c=>c.Categories).ToList();

            CoursesVM courseVM = new()
            {
                Courses = courses,
            };

            return View(courseVM);
        }

        public async Task<IActionResult> Detail(int id)
        {
            if (id == null) { return NotFound(); }

            Course _course = await _appDb.Courses.FirstOrDefaultAsync(c => c.Id == id);
            if (_course == null) { return NotFound(); }

            List<CourseCategory> courseCategories = _appDb.CoursesCategories.Include(cc => cc.Category).Where(cc => cc.CategoryId == _course.Id).ToList();
            CoursesVM courseVM = new()
            {
                Course = _course,
                CourseCategories = courseCategories
            };

            return View(courseVM);
        }
    }
}
