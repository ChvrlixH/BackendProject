using BackEndProject.Areas.Admin.ViewModels;
using BackEndProject.Models;
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
            return View(_appDb.Courses);
        }

        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Create()
        {
            CoursesVM coursesVM = new CoursesVM
            {
                Categories = _appDb.Categories.ToList()
            };
            return View(coursesVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Create(CoursesVM _courses)
        {
            CoursesVM coursesVM = new CoursesVM
            {
                Categories = _appDb.Categories.ToList()
            };

            if (!_courses.Photo.CheckFileSize(1500))
            {
                ModelState.AddModelError("Image", "Faylin hecmi 1.5 mb-dan kicik olmalidir.");
                return View(coursesVM);
            }
            if (!_courses.Photo.CheckFileType(ContentType.image.ToString()))
            {
                ModelState.AddModelError("Image", "Faylin tipi image olmalidir.");
                return View(coursesVM);
            }

            Course newCourse = new()
            {
                Title = _courses.Title,
                Description = _courses.Description,
                Starts = _courses.Starts,
                Duration = _courses.Duration,
                ClassDuration = _courses.ClassDuration,
                SkillLevel = _courses.SkillLevel,
                Language = _courses.Language,
                Students = _courses.Students,
                Assesments = _courses.Assesments,
                Fee = _courses.Fee
            };

            newCourse.Image = await _courses.Photo.SaveImg(_webHostEnvironment.WebRootPath, "admin", "images", "faces");

            List<Course> courses = _appDb.Courses.ToList();
            foreach (var coursesItem in courses)
            {
                if (coursesItem.Title == _courses.Title)
                {
                    ModelState.AddModelError("Title", "This title is available");
                    return View();
                }
            }

            List<CourseCategory> courseCategory1 = new List<CourseCategory>();
            string categories = Request.Form["states"];
            string[] arr = categories.Split(",");
            List<int> ids = new List<int>();
            foreach (string categoryId in arr)
            {
                ids.Add(Int32.Parse(categoryId));
            }
            foreach (int cId in ids)
            {
                courseCategory1.Add(new CourseCategory
                {
                    CourseId = newCourse.Id,
                    CategoryId = cId
                });
            }
            newCourse.Categories= courseCategory1;
            await _appDb.Courses.AddAsync(newCourse);
            await _appDb.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Read(int id)
            {
                if (id == null) { return NotFound(); }
                Course? course = _appDb.Courses.AsNoTracking().FirstOrDefault(c => c.Id == id);
                if (course is null) { return NotFound(); }

                return View(course);
            }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null) { return NotFound(); }
            Course? course = await _appDb.Courses.FirstOrDefaultAsync(c => c.Id == id);
            IList<CourseCategory> courseCategories = _appDb.CoursesCategories.Include(c => c.Category).Where(c => c.CourseId == course.Id).ToList();
            if (course is null) { return NotFound(); }

            return View(course);
        }

            [HttpPost]
            [ValidateAntiForgeryToken]
            [ActionName("Delete")]
            [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCourses(int id)
            {
            if (id == null) { return NotFound(); }
            Course? course = await _appDb.Courses.FirstOrDefaultAsync(c => c.Id == id);
            IList<CourseCategory> courseCategories = _appDb.CoursesCategories.Include(c => c.Category).Where(c => c.CourseId == course.Id).ToList();
            if (course is null) { return NotFound(); }

            FileCourse.DeleteFile(_webHostEnvironment.WebRootPath,  "admin", "images", "faces", course.Image);

            _appDb.Courses.Remove(course);
               await _appDb.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Update(int id)
            {
            if (id == null) { return NotFound(); }
            Course? course = await _appDb.Courses.Include(c=>c.Categories).FirstOrDefaultAsync(c => c.Id == id);
            IList<CourseCategory> courseCategories = _appDb.CoursesCategories.Include(c => c.Category).Where(c => c.CourseId == course.Id).ToList();
            if (course is null) { return NotFound(); }

            CoursesVM coursesVM = new()
            {
                Course= course,
                Categories = _appDb.Categories.ToList(),
                CourseCategories= courseCategories
            };


            return View(coursesVM);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Update(CoursesVM editedCourse, int id)
            {
            Course? course = await _appDb.Courses.Include(c => c.Categories).FirstOrDefaultAsync(c => c.Id == id);
            IList<CourseCategory> courseCategories = _appDb.CoursesCategories.Include(c => c.Category).Where(c => c.CourseId == course.Id).ToList();

            CoursesVM coursesVM = new()
            {
                Course = course,
                Categories = _appDb.Categories.ToList(),
                CourseCategories = courseCategories
            };

            if (!editedCourse.Photo.CheckFileType(ContentType.image.ToString()))
            {
                ModelState.AddModelError("Image", "Faylin tipi image olmalidir.");
                return View(coursesVM);
            }
            if (!editedCourse.Photo.CheckFileSize(1500))
            {
                ModelState.AddModelError("Image", "Faylin hecmi 1.5 mb-dan kicik olmalidir.");
                return View(coursesVM);
            }

            List<CourseCategory> courseCategory1 = new List<CourseCategory>();
            string categories = Request.Form["states"];
            string[] arr = categories.Split(",");
            List<int> ids = new List<int>();
            foreach (string categoryId in arr)
            {
                ids.Add(Int32.Parse(categoryId));
            }
            foreach (int cId in ids)
            {
                courseCategory1.Add(new CourseCategory
                {
                    CourseId = course.Id,
                    CategoryId = cId
                });
            }

            Utils.Enums.FileCourse.DeleteFile(_webHostEnvironment.WebRootPath, "admin", "images", "faces", course.Image);
            course.Image = await editedCourse.Photo.SaveImg(_webHostEnvironment.WebRootPath, "admin", "images", "faces");

            course.Title = editedCourse.Title;
            course.Description = editedCourse.Description;
            course.Starts = editedCourse.Starts;
            course.Duration = editedCourse.Duration;
            course.ClassDuration = editedCourse.ClassDuration;
            course.SkillLevel= editedCourse.SkillLevel;
            course.Language= editedCourse.Language;
            course.Students= editedCourse.Students;
            course.Assesments= editedCourse.Assesments;
            course.Fee= editedCourse.Fee;
            course.Categories = courseCategory1;

            await _appDb.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            }
        }
    }
