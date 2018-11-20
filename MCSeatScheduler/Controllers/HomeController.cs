using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MCSeatScheduler.Controllers
{
	public class HomeController : Controller
	{
		private readonly OpenSeatsController _apiController;

		public HomeController(OpenSeatsController openSeatsController)
		{
            _apiController = openSeatsController;
		}

		// GET: Home
		public ActionResult Index()
		{
			//Page where they pick a date n stuff
			return View();
		}
		
		public ActionResult Reserve(DateTime date)
		{
			return View("Reserve");

		}

		public async Task<IActionResult> ReserveNew(DateTime date, string eid)
		{			 
			//make sure seats still available
			var open = _apiController.GetOpenSeats(date.Date) as OkObjectResult;

			if (open != null){
				var openSeats = open.Value as IEnumerable<Model.OpenSeats>;

				//Make sure theres still spots available
				if (openSeats.Count() > 9){
					return BadRequest("All seats reserved");
				}
				//cant reserve twice
				else if (openSeats.Any(s=> s.EmployeeId == eid)){
					return BadRequest("cant reserve yourself twice");
				}
				else{
					return await _apiController.ReserveSeat(date, eid);
				}
			}
			else{
				throw new Exception("Error getting available seats");
			}
			//var ret = api.PutOpenSeats();

		}

		public async Task Delete(DateTime date, string eid)
		{
			//var dt = DateTime.Parse(date);
			var ret = await _apiController.DeleteOpenSeats(date.Date, eid);

			//if (ret == null)
			//{
			//	return View();
			//}
			//return View(ret.Value);
		}
	

		public IActionResult OpenSeats(DateTime date)
		{
			//Set the date here for the other views
			TempData["SelectedDate"] = date;

			//var dt = DateTime.Parse(date);
			var ret = _apiController.GetOpenSeats(date.Date) as OkObjectResult;

			if (ret == null)
		    {
				return View();
			}
			return View(ret.Value);
		}

		// POST: Home
		[HttpPost]
		public IActionResult Create(IFormCollection collection)
		{
			try
			{
				var date = collection["txtDate"];
				return RedirectToAction("OpenSeats", new { date = date.ToString()
				});
			}
			catch
			{
				return View();
			}
		}
	}
}