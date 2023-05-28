
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
            List<Course> courses = _appDb.Courses.ToList();

            CourseVM courseVM = new()
            {
                course = courses,
            };

            return View(courseVM);
        }

        public IActionResult Detail()
        {
            return View();
        }
    }
}
