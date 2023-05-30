using BackEndProject.Areas.Admin.ViewModels;
using BackEndProject.Utils;
using BackEndProject.Utils.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BackEndProject.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin,Moderator")]
	public class BlogsController : Controller
	{
		private readonly AppDb _appDb;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public BlogsController(AppDb appDb, IWebHostEnvironment webHostEnvironment)
		{
			_appDb = appDb;
			_webHostEnvironment = webHostEnvironment;
		}

		public IActionResult Index()
		{
			List<Blog> blogs = _appDb.Blogs.ToList();
			return View(blogs);
		}

		[Authorize(Roles = "Admin,Moderator")]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin,Moderator")]
		public async Task<IActionResult> Create(BlogsVM blogsVM)
		{
			if (!ModelState.IsValid)
				return View();

			if (blogsVM.Image == null)
			{
				ModelState.AddModelError("Image", "Image bos olmamalidir.");
				return View();
			}

			if (!blogsVM.Image.CheckFileSize(500))
			{
				ModelState.AddModelError("Image", "Faylin hecmi 100 kb-dan kicik olmalidir.");
				return View();
			}
			if (!blogsVM.Image.CheckFileType(ContentType.image.ToString()))
			{
				ModelState.AddModelError("Image", "Faylin tipi image olmalidir.");
				return View();
			}

			List<Blog> blogs = _appDb.Blogs.ToList();
			foreach (var blogsItem in blogs)
			{
				if (blogsItem.Title == blogsVM.Title)
				{
					ModelState.AddModelError("Title", "This title is available");
					return View();
				}
			}

			string fileName = $"{Guid.NewGuid()}-{blogsVM.Image.FileName}";
			string path = Path.Combine(_webHostEnvironment.WebRootPath, "admin", "images", "faces", fileName);
			using (FileStream stream = new(path, FileMode.Create))
			{
				await blogsVM.Image.CopyToAsync(stream);
			}

			Blog blog = new()
			{
				Name = blogsVM.Name,
				Date= blogsVM.Date,
				Comment= blogsVM.Comment,
				Title = blogsVM.Title,
				Description = blogsVM.Description,
				Image = fileName
			};

			await _appDb.Blogs.AddAsync(blog);
			await _appDb.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}

		[Authorize(Roles = "Admin,Moderator")]
		public IActionResult Read(int id)
		{
			Blog? blog = _appDb.Blogs.AsNoTracking().FirstOrDefault(c => c.Id == id);
			if (blog is null) { return NotFound(); }

			return View(blog);
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int id)
		{
			Blog? blog = await _appDb.Blogs.FirstOrDefaultAsync(c => c.Id == id);

			if (blog is null) { return NotFound(); }

			return View(blog);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ActionName("Delete")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteCourses(int id)
		{
			Blog? blog = await _appDb.Blogs.FirstOrDefaultAsync(c => c.Id == id);

			if (blog is null) { return NotFound(); }

			FileCourse.DeleteFile(_webHostEnvironment.WebRootPath, "admin", "images", "faces", blog.Image);

			_appDb.Blogs.Remove(blog);
			await _appDb.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}

		[Authorize(Roles = "Admin,Moderator")]
		public IActionResult Update(int id)
		{
			Blog? blog = _appDb.Blogs.FirstOrDefault(c => c.Id == id);

			if (blog is null) { return NotFound(); }

			return View(blog);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin,Moderator")]
		public IActionResult Update(Blog blog, int id)
		{
			Blog? dBblog = _appDb.Blogs.AsNoTracking().FirstOrDefault(c => c.Id == id);

			if (blog is null) { return NotFound(); }


			_appDb.Blogs.Update(blog);
			_appDb.SaveChanges();

			return RedirectToAction(nameof(Index));
		}
	}
}
