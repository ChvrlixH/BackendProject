using BackEndProject.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEndProject.Controllers
{
    public class EventController : Controller
    {
        private readonly AppDb _appDb;

        public EventController(AppDb appDb)
        {
            _appDb = appDb;
        }

        public IActionResult Index()
        {
            List<Event> events = _appDb.Events.Include(e => e.Speakers).ToList();

            EventVM eventVM = new()
            {
                Events = events
            };
            return View(eventVM);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) { return NotFound(); }

            Event _event = await _appDb.Events.FirstOrDefaultAsync(e => e.Id == id);
            if (_event == null) { return NotFound(); }

            List<EventSpeaker> eventSpeakers = _appDb.EventSpeakers.Include(es=>es.Speaker).Where(es=>es.EventId == _event.Id).ToList();
            EventVM eventVM = new()
            { 
                Event = _event,
                EventSpeakers= eventSpeakers
            };

            return View(eventVM);

        }
    }
}
