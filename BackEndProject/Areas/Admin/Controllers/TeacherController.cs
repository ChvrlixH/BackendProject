using BackEndProject.Areas.Admin.ViewModels;
using BackEndProject.Models;
using BackEndProject.Utils;
using BackEndProject.Utils.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using System.Data;

namespace BackEndProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class TeacherController : Controller
    {
        private readonly AppDb _appDb;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TeacherController(AppDb appDb, IWebHostEnvironment webHostEnvironment)
        {
            _appDb = appDb;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(_appDb.Teachers);
        }

        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Create(Teacher teacher)
        {
            if (!teacher.Photo.CheckFileType(ContentType.image.ToString()))
            {
                ModelState.AddModelError("Image", "Faylin tipi image olmalidir.");
                return View();
            }
            if (!teacher.Photo.CheckFileSize(1000))
            {
                ModelState.AddModelError("Image", "Faylin hecmi 1 mb-dan kicik olmalidir.");
                return View();
            }

            Teacher newTeacher = new()
            {
                Fullname = teacher.Fullname,
                Profession = teacher.Profession,
                Description = teacher.Description,
                Degree = teacher.Degree,
                Experience = teacher.Experience,
                Hobbies = teacher.Hobbies,
                Faculty = teacher.Faculty
            };

            newTeacher.Image = await teacher.Photo.SaveImg(_webHostEnvironment.WebRootPath, "admin", "images", "faces");

            SocialMedia socialMedia = new()
            {
                Mail = teacher.SocialMedias.Mail,
                Number = teacher.SocialMedias.Number,
                Instagram = teacher.SocialMedias.Instagram,
                Facebook = teacher.SocialMedias.Facebook,
                Pinterest = teacher.SocialMedias.Pinterest,
                Twitter = teacher.SocialMedias.Twitter
            };

            Skill skill = new()
            {
                Language = teacher.Skills.Language,
                TeamLeader = teacher.Skills.TeamLeader,
                Development = teacher.Skills.Development,
                Design = teacher.Skills.Design,
                Communication = teacher.Skills.Communication,
                Innovation = teacher.Skills.Innovation,
            };

            newTeacher.Skills = skill;
            newTeacher.SocialMedias= socialMedia;

            await _appDb.Teachers.AddAsync(newTeacher);
            await _appDb.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Read(int id)
        {
            if (id == null) return NotFound();
            Teacher? teacher = await _appDb.Teachers.AsNoTracking().Include(t => t.Skills).Include(t => t.SocialMedias).FirstOrDefaultAsync(t => t.Id == id);
            if (teacher is null) { return NotFound(); }

            return View(teacher);
        }

        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            Teacher teacher = await _appDb.Teachers.Include(t => t.Skills).Include(t => t.SocialMedias).FirstOrDefaultAsync(t => t.Id == id);
            if (teacher is null) { return NotFound(); }

            return View(teacher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]

        public async Task<IActionResult> Update(int id, Teacher editedTeacher)
        {
            if (id == null) return NotFound();
            Teacher teacher = await _appDb.Teachers.Include(t => t.Skills).Include(t => t.SocialMedias).FirstOrDefaultAsync(t => t.Id == id);
            if (teacher is null) { return NotFound(); }

            if (!editedTeacher.Photo.CheckFileType(ContentType.image.ToString()))
            {
                ModelState.AddModelError("Image", "Faylin tipi image olmalidir.");
                return View(teacher);
            }
            if (!editedTeacher.Photo.CheckFileSize(1000))
            {
                ModelState.AddModelError("Image", "Faylin hecmi 1 mb-dan kicik olmalidir.");
                return View(teacher);
            }

           
                Utils.Enums.FileCourse.DeleteFile(_webHostEnvironment.WebRootPath, "admin", "images", "faces", teacher.Image);
                teacher.Image = await editedTeacher.Photo.SaveImg(_webHostEnvironment.WebRootPath, "admin", "images", "faces");
            teacher.Fullname = editedTeacher.Fullname;
            teacher.Profession = editedTeacher.Profession;
            teacher.Description = editedTeacher.Description;
            teacher.Degree = editedTeacher.Degree;
            teacher.Experience = editedTeacher.Experience;
            teacher.Hobbies = editedTeacher.Hobbies;
            teacher.Faculty = editedTeacher.Faculty;
            teacher.SocialMedias.Mail = editedTeacher.SocialMedias.Mail;
            teacher.SocialMedias.Number = editedTeacher.SocialMedias.Number;
            teacher.SocialMedias.Instagram = editedTeacher.SocialMedias.Instagram;
            teacher.SocialMedias.Facebook = editedTeacher.SocialMedias.Facebook;
            teacher.SocialMedias.Pinterest = editedTeacher.SocialMedias.Pinterest;
            teacher.SocialMedias.Twitter = editedTeacher.SocialMedias.Twitter;
            teacher.Skills.Language = editedTeacher.Skills.Language;
            teacher.Skills.TeamLeader = editedTeacher.Skills.TeamLeader;
            teacher.Skills.Development = editedTeacher.Skills.Development;
            teacher.Skills.Design = editedTeacher.Skills.Design;
            teacher.Skills.Communication = editedTeacher.Skills.Communication;
            teacher.Skills.Innovation = editedTeacher.Skills.Innovation;

            await _appDb.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Teacher teachers = await _appDb.Teachers.Include(t => t.Skills).Include(t => t.SocialMedias).FirstOrDefaultAsync(t => t.Id == id);
            if (teachers is null) { return NotFound(); }

            return View(teachers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName(nameof(Delete))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTeacher(int? id)
        {
            if (id == null) return NotFound();
            Teacher teachers = await _appDb.Teachers.Include(t => t.Skills).Include(t => t.SocialMedias).FirstOrDefaultAsync(t => t.Id == id);
            if (teachers is null) { return NotFound(); }

            Utils.Enums.FileCourse.DeleteFile(_webHostEnvironment.WebRootPath, "admin", "images", "faces", teachers.Image);
            
            _appDb.Teachers.Remove(teachers);
            await _appDb.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
