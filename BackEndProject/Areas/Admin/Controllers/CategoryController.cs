using BackEndProject.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEndProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin,Moderator")]
    public class CategoryController : Controller
    {
        private readonly AppDb _appDb;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CategoryController(AppDb appDb, IWebHostEnvironment webHostEnvironment)
        {
            _appDb = appDb;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View(_appDb.Categories);
        }

        public async Task<IActionResult> Read(int? id)
        {
            if (id == null) { return NotFound(); }

            Category category = await _appDb.Categories.FindAsync(id);
            if (category == null) { return NotFound(); }
            return View(category);
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) { return NotFound(); }

            Category category = await _appDb.Categories.FindAsync(id);
            if (category == null) { return NotFound(); }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Category editedCategory)
        {
            if (id == null) { return NotFound(); }
            Category category = await _appDb.Categories.FindAsync(id);
            if (category == null) { return NotFound(); }

            category.Name = editedCategory.Name;

            await _appDb.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            Category newCategory = new Category
            {
                Name = category.Name
            };

            _appDb.Categories.Add(newCategory);
            await _appDb.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) { return NotFound(); }
            Category category = await _appDb.Categories.FindAsync(id);
            if (category == null) { return NotFound(); }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null) { return NotFound(); }
            Category category = await _appDb.Categories.FindAsync(id);
            if (category == null) { return NotFound(); }

            _appDb.Categories.Remove(category);
            await _appDb.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
