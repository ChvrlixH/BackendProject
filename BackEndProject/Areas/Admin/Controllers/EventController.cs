using BackEndProject.Areas.Admin.ViewModels;
using BackEndProject.Models;
using BackEndProject.Utils;
using BackEndProject.Utils.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BackEndProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin,Moderator")]
    public class EventController : Controller
    {
        private readonly AppDb _appDb;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EventController(AppDb appDb, IWebHostEnvironment webHostEnvironment)
        {
            _appDb = appDb;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View(_appDb.Events);
        }

        public async Task<IActionResult> Read(int? id) 
        {
            if(id == null) { return NotFound(); }
            Event _event = await _appDb.Events.FirstOrDefaultAsync(e => e.Id == id);
            IList<EventSpeaker> eventSpeakers = _appDb.EventSpeakers.Include(e=>e.Speaker).Where(e=>e.EventId == _event.Id).ToList(); 
            if (_event == null) { return NotFound();}

            return View(_event);
        }

        public async Task<IActionResult> Update(int? id)
        {
            if(id == null) { return NotFound();}
            Event _event = await _appDb.Events.Include(e=>e.Speakers).FirstOrDefaultAsync(e => e.Id == id);
            IList<EventSpeaker> eventSpeakers = _appDb.EventSpeakers.Include(e => e.Speaker).Where(e => e.EventId == _event.Id).ToList();
            if (_event == null) { return NotFound();}

            EventVM model = new EventVM
            {
                Event = _event,
                Speakers = _appDb.Speakers.ToList(),
                EventSpeakers = eventSpeakers
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, EventVM editedEvent)
        {
            Event _event = await _appDb.Events.Include(e => e.Speakers).FirstOrDefaultAsync(e => e.Id == id);
            IList<EventSpeaker> eventSpeakers = _appDb.EventSpeakers.Include(e => e.Event).Include(e => e.Speaker).Where(e => e.EventId == _event.Id).ToList();
            EventVM model = new EventVM
            {
                Event = _event,
                Speakers = _appDb.Speakers.ToList(),
                EventSpeakers = eventSpeakers
            };

            if (!editedEvent.Photo.CheckFileType(ContentType.image.ToString()))
            {
                ModelState.AddModelError("Image", "Faylin tipi image olmalidir.");
                return View(model);
            }
            if (!editedEvent.Photo.CheckFileSize(1000))
            {
                ModelState.AddModelError("Image", "Faylin hecmi 1 mb-dan kicik olmalidir.");
                return View(model);
            }

            List<EventSpeaker> eventSpeakers1 = new List<EventSpeaker>();
            string spikers = Request.Form["states"];
            string[] arr = spikers.Split(",");
            List<int> ids = new List<int>();
            foreach (string spikerId in arr)
            {
                ids.Add(Int32.Parse(spikerId));
            }
            foreach (int sId in ids)
            {
                eventSpeakers1.Add(new EventSpeaker
                {
                    EventId = _event.Id,
                    SpeakerId = sId
                });
            }

            Utils.Enums.FileCourse.DeleteFile(_webHostEnvironment.WebRootPath, "admin", "images", "faces", _event.Image);
            _event.Image = await editedEvent.Photo.SaveImg(_webHostEnvironment.WebRootPath, "admin", "images", "faces");

            _event.Title = editedEvent.Title;
            _event.Description = editedEvent.Description;
            _event.Day = editedEvent.Day;
            _event.Time = editedEvent.Time;
            _event.Venue = editedEvent.Venue;
            _event.Speakers = eventSpeakers1;

            await _appDb.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Create()
        {
            EventVM model = new EventVM
            {
                Speakers = _appDb.Speakers.ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventVM _event)
        {
            EventVM model = new EventVM
            {
                Speakers = _appDb.Speakers.ToList()
            };

            if (!_event.Photo.CheckFileType(ContentType.image.ToString()))
            {
                ModelState.AddModelError("Image", "Faylin tipi image olmalidir.");
                return View(model);
            }
            if (!_event.Photo.CheckFileSize(1000))
            {
                ModelState.AddModelError("Image", "Faylin hecmi 1 mb-dan kicik olmalidir.");
                return View(model);
            }

            Event newEvent = new Event
            {
                Title = _event.Title,
                Description = _event.Description,
                Day = _event.Day,
                Time = _event.Time,
                Venue = _event.Venue
            };

            newEvent.Image = await _event.Photo.SaveImg(_webHostEnvironment.WebRootPath, "admin", "images", "faces");

            List<EventSpeaker> eventSpeakers = new List<EventSpeaker>();
            string spikers = Request.Form["states"];
            string[] arr = spikers.Split(",");
            List<int> ids = new List<int>();
            foreach (string spikerId in arr)
            {

                ids.Add(Int32.Parse(spikerId));
            }

            foreach (int id in ids)
            {
                eventSpeakers.Add(new EventSpeaker
                {
                    EventId = newEvent.Id,
                    SpeakerId = id
                });
            }
            newEvent.Speakers = eventSpeakers;
            await _appDb.Events.AddAsync(newEvent);
            await _appDb.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) { return NotFound(); }
            Event _event = await _appDb.Events.FirstOrDefaultAsync(e => e.Id == id);
            IList<EventSpeaker> eventSpeakers = _appDb.EventSpeakers.Include(e => e.Speaker).Where(e => e.EventId == _event.Id).ToList();
            if (_event == null) { return NotFound(); }

            return View(_event);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id is null) { return NotFound(); }
            Event _event = await _appDb.Events.FirstOrDefaultAsync(e => e.Id == id);
            IList<EventSpeaker> eventSpeakers = _appDb.EventSpeakers.Include(e => e.Speaker).Where(e => e.EventId == _event.Id).ToList();
            if (_event == null) { return NotFound(); }

            Utils.Enums.FileCourse.DeleteFile(_webHostEnvironment.WebRootPath, "admin", "images", "faces", _event.Image);
            
            _appDb.Events.Remove(_event);
            await _appDb.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

    }
}
