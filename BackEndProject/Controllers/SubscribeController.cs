using Microsoft.AspNetCore.Mvc;

namespace BackEndProject.Controllers
{
	public class SubscribeController : Controller
	{
		private readonly AppDb _appDb;

		public SubscribeController(AppDb appDb)
		{
			_appDb = appDb;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Subscribe(string Email)
		{

			return View();
		}
	}
}
