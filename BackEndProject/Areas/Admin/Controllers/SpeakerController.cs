using BackEndProject.Areas.Admin.ViewModels;
using BackEndProject.Utils;
using BackEndProject.Utils.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEndProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class SpeakerController : Controller
    {
        private readonly AppDb _appDb;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SpeakerController(AppDb appDb, IWebHostEnvironment webHostEnvironment)
        {
            _appDb = appDb;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View(_appDb.Speakers);
        }

        public async Task<IActionResult> Read(int? id)
        {
            if (id == null) { return NotFound(); }
            
            Speaker speaker = await _appDb.Speakers.FindAsync(id);
            if (speaker == null) { return NotFound();}
            return View(speaker);
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) { return NotFound();}

            Speaker speaker = await _appDb.Speakers.FindAsync(id);
            if (speaker == null) { return NotFound(); }
            return View(speaker);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Speaker editedSpeaker)
        {
            if (id == null) { return NotFound();}
            Speaker speaker = await _appDb.Speakers.FindAsync(id);
            if (speaker == null) { return NotFound();}
           
            if (!editedSpeaker.Photo.CheckFileType(ContentType.image.ToString()))
            {
                ModelState.AddModelError("Image", "Faylin tipi image olmalidir.");
                return View(speaker);
            }
            if (!editedSpeaker.Photo.CheckFileSize(1000))
            {
                ModelState.AddModelError("Image", "Faylin hecmi 1 mb-dan kicik olmalidir.");
                return View(speaker);
            }

            Utils.Enums.FileCourse.DeleteFile(_webHostEnvironment.WebRootPath, "admin", "images", "faces", speaker.Image);
            speaker.Image = await editedSpeaker.Photo.SaveImg(_webHostEnvironment.WebRootPath, "admin", "images", "faces");
            speaker.Fullname=editedSpeaker.Fullname;
            speaker.Profession= editedSpeaker.Profession;
            await _appDb.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Speaker speaker)
        {
            if(speaker.Photo == null) { return View();}

            if (!speaker.Photo.CheckFileType(ContentType.image.ToString()))
            {
                ModelState.AddModelError("Image", "Faylin tipi image olmalidir.");
                return View();
            }
            if (!speaker.Photo.CheckFileSize(1000))
            {
                ModelState.AddModelError("Image", "Faylin hecmi 1 mb-dan kicik olmalidir.");
                return View();
            }

            Speaker newSpeaker = new Speaker
            {
                Fullname = speaker.Fullname,
                Profession = speaker.Profession
            };

            newSpeaker.Image = await speaker.Photo.SaveImg(_webHostEnvironment.WebRootPath, "admin", "images", "faces");
            _appDb.Speakers.Add(newSpeaker);
            await _appDb.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete (int? id)
        {
            if (id == null) return NotFound();
            Speaker speaker = await _appDb.Speakers.FindAsync(id);
            if (speaker == null) return NotFound();
            return View(speaker);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null) return NotFound();
            Speaker speaker = await _appDb.Speakers.FindAsync(id);
            if (speaker == null) return NotFound();

            Utils.Enums.FileCourse.DeleteFile(_webHostEnvironment.WebRootPath, "admin", "images", "faces", speaker.Image);

            _appDb.Speakers.Remove(speaker);
            await _appDb.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

