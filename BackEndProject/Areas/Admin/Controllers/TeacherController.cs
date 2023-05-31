using BackEndProject.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BackEndProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class TeacherController : Controller
    {
        private readonly AppDb _appDb;

        public TeacherController(AppDb appDb)
        {
            _appDb = appDb;
        }

        public async Task<IActionResult> Index()
        {
            List<Teacher> teachers = await _appDb.Teachers.Include(t => t.Skills).Include(t => t.SocialMedias).ToListAsync();
            return View(teachers);
        }

        public IActionResult Create()
        {
            ViewBag.Skills = _appDb.Skills.AsEnumerable();
            ViewBag.SocialMedias = _appDb.SocialMedias.AsEnumerable();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeacherVM teacherVM)
        {
            ViewBag.Skills = _appDb.Skills.AsEnumerable();
            ViewBag.SocialMedias = _appDb.SocialMedias.AsEnumerable();

            if (!ModelState.IsValid)
                return View();

            if (!_appDb.Skills.Any(s => s.Id == teacherVM.SkillId))
                return BadRequest();

            if (!_appDb.SocialMedias.Any(s => s.Id == teacherVM.SocialMediaId))
                return BadRequest();

            Teacher teacher = new()
            {
                Name = teacherVM.Name,
                Image = teacherVM.Image,
                Profession = teacherVM.Profession,
                Description = teacherVM.Description,
                Degree = teacherVM.Degree,
                Experience = teacherVM.Experience,
                Hobbies = teacherVM.Hobbies,
                Faculty = teacherVM.Faculty,
                Mail = teacherVM.Mail,
                Number = teacherVM.Number,
                Skills = teacherVM.Skills,
                SocialMedias = teacherVM.SocialMedias
            };

            await _appDb.Teachers.AddAsync(teacher);
            await _appDb.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Read(int id)
        {
            Teacher? teacher = await _appDb.Teachers.AsNoTracking().Include(t => t.Skills).Include(t => t.SocialMedias).FirstOrDefaultAsync(t => t.Id == id);
            if (teacher is null) { return NotFound(); }

            return View(teacher);
        }

        public async Task<IActionResult> Update(int id)
        {
            Teacher? teacher = await _appDb.Teachers.FirstOrDefaultAsync(t => t.Id == id);
            if (teacher is null) { return NotFound(); }

            TeacherVM teacherVM = new()
            {
                Name = teacher.Name,
                Image = teacher.Image,
                Profession = teacher.Profession,
                Description = teacher.Description,
                Degree = teacher.Degree,
                Experience = teacher.Experience,
                Hobbies = teacher.Hobbies,
                Faculty = teacher.Faculty,
                Mail = teacher.Mail,
                Number = teacher.Number,
                Skills = teacher.Skills,
                SocialMedias = teacher.SocialMedias
            };

            return View(teacherVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int id, TeacherVM teacherVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (!_appDb.Skills.Any(s => s.Id == teacherVM.SkillId))
                return BadRequest();

            if (!_appDb.SocialMedias.Any(s => s.Id == teacherVM.SocialMediaId))
                return BadRequest();

            Teacher? teacher = await _appDb.Teachers.FirstOrDefaultAsync(t => t.Id == id);
            if (teacher is null) { return NotFound(); }

            teacher.Name = teacherVM.Name;
            teacher.Image = teacherVM.Image;
            teacher.Profession = teacherVM.Profession;
            teacher.Description = teacherVM.Description;
            teacher.Degree = teacherVM.Degree;
            teacher.Experience = teacherVM.Experience;
            teacher.Hobbies = teacherVM.Hobbies;
            teacher.Faculty = teacherVM.Faculty;
            teacher.Mail = teacherVM.Mail;
            teacher.Number = teacherVM.Number;
            teacher.Skills = teacherVM.Skills;
            teacher.SocialMedias = teacherVM.SocialMedias;

            await _appDb.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int id)
        {
            var teachers = await _appDb.Teachers.Include(t => t.Skills).Include(t => t.SocialMedias).FirstOrDefaultAsync(t => t.Id == id);
            if (teachers is null) { return NotFound(); }

            return View(teachers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName(nameof(Delete))]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var teachers = await _appDb.Teachers.FirstOrDefaultAsync(t => t.Id == id);
            if (teachers is null) { return NotFound(); }

            await _appDb.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
