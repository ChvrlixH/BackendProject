using BackEndProject.Areas.Admin.ViewModels;
using BackEndProject.Utils;
using BackEndProject.Utils.Enums;
using BackEndProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net.Mime;
using ContentType = BackEndProject.Utils.Enums.ContentType;

namespace BackEndProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class CourseController : Controller
    {
        private readonly AppDb _appDb;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CourseController(AppDb appDb, IWebHostEnvironment webHostEnvironment)
        {
            _appDb = appDb;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Course> courses = _appDb.Courses.ToList();
            return View(courses);
        }
        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Create(CoursesVM coursesVM)
        {
            if (!ModelState.IsValid)
                return View();

            if (coursesVM.Image == null)
            {
                ModelState.AddModelError("Image", "Image bos olmamalidir.");
                return View();
            }

            if (!coursesVM.Image.CheckFileSize(100))
            {
                ModelState.AddModelError("Image", "Faylin hecmi 100 kb-dan kicik olmalidir.");
                return View();
            }
            if (!coursesVM.Image.CheckFileType(ContentType.image.ToString()))
            {
                ModelState.AddModelError("Image", "Faylin tipi image olmalidir.");
                return View();
            }

            List<Course> courses = _appDb.Courses.ToList();
            foreach (var coursesItem in courses)
            {
                if (coursesItem.Title == coursesVM.Title)
                {
                    ModelState.AddModelError("Title", "This title is available");
                    return View();
                }
            }

            string fileName = $"{Guid.NewGuid()}-{coursesVM.Image.FileName}";
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "admin", "images", "faces", fileName);
            using (FileStream stream = new(path, FileMode.Create))
            {
                await coursesVM.Image.CopyToAsync(stream);
            }

            Course course = new()
            {
                Title = coursesVM.Title,
                Description = coursesVM.Description,
                Image = fileName
            };

            await _appDb.Courses.AddAsync(course);
            await _appDb.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Read(int id)
            {
                Course? course = _appDb.Courses.AsNoTracking().FirstOrDefault(c => c.Id == id);
                if (course is null) { return NotFound(); }

                return View(course);
            }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
            {
                Course? course = await _appDb.Courses.FirstOrDefaultAsync(c => c.Id == id);

                if (course is null) { return NotFound(); }

                return View(course);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            [ActionName("Delete")]
            [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCourses(int id)
            {
                Course? course = await _appDb.Courses.FirstOrDefaultAsync(c => c.Id == id);

                if (course is null) { return NotFound(); }

            FileCourse.DeleteFile(_webHostEnvironment.WebRootPath,  "admin", "images", "faces", course.Image);

            _appDb.Courses.Remove(course);
               await _appDb.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Update(int id)
            {
                Course? course = _appDb.Courses.FirstOrDefault(c => c.Id == id);

                if (course is null) { return NotFound(); }

                return View(course);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
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
