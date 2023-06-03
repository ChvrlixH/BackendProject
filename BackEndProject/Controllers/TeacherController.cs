using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEndProject.Controllers
{
    public class TeacherController : Controller
    {
        private readonly AppDb _appDb;

        public TeacherController(AppDb appDb)
        {
            _appDb = appDb;
        }

        public IActionResult Index()
        {
            List<Teacher> teachers = _appDb.Teachers.Include(t=>t.SocialMedias).Include(t=>t.Skills).ToList();
            return View(teachers);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return NotFound();
            Teacher teachers = await _appDb.Teachers.Include(t => t.SocialMedias).Include(t => t.Skills).FirstOrDefaultAsync(t=>t.Id==id);
            if (teachers is null) { return NotFound();}
            return View(teachers);
        }
    }
}
