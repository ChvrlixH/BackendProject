using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEndProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {
        private readonly AppDb _appDb;

        public CourseController(AppDb appDb)
        {
            _appDb = appDb;
        }

        public IActionResult Index()
        {
            List<Course> courses = _appDb.Courses.ToList();
            return View(courses);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Course course)
        {
            if (!ModelState.IsValid)
                return View();

            List<Course> courses = _appDb.Courses.ToList();
            foreach (var coursesItem in courses)
            {
                if (coursesItem.Title == course.Title)
                {
                    ModelState.AddModelError("Title", "This title is available");
                    return View();
                }
            }

            _appDb.Courses.Add(course);
            _appDb.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Read(int id)
        {
            Course? course = _appDb.Courses.AsNoTracking().FirstOrDefault(c => c.Id == id);
            if (course is null) { return NotFound(); }

            return View(course);
        }


        public IActionResult Delete (int id)
        {
            Course? course = _appDb.Courses.FirstOrDefault(c => c.Id == id);

            if (course is null) { return NotFound();}

            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeleteCourses(int id)
        {
            Course? course = _appDb.Courses.FirstOrDefault(c => c.Id == id);

            if (course is null) { return NotFound(); }

            _appDb.Courses.Remove(course);
            _appDb.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {
            Course? course = _appDb.Courses.FirstOrDefault(c => c.Id == id);

            if (course is null) { return NotFound(); }

            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Course course, int id)
        {
            Course? dBcourse = _appDb.Courses.AsNoTracking().FirstOrDefault(c => c.Id == id);

            if (course is null) { return NotFound(); }


            _appDb.Courses.Update(course);
            _appDb.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
