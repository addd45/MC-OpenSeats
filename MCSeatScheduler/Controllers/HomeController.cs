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
		private readonly MCDBContext _dbContext;

		public HomeController(MCDBContext context)
		{
			_dbContext = context;

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
			var api = new OpenSeatsController(_dbContext);
			 
			//make sure seats still available
			var open = api.GetOpenSeats(date.Date) as OkObjectResult;

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
					return await api.ReserveSeat(date, eid);
				}
			}
			else{
				throw new Exception("Error getting available seats");
			}

		}

		public void Delete(DateTime date, string eid)
		{
			var api = new OpenSeatsController(_dbContext);
			//var dt = DateTime.Parse(date);
			var ret = api.DeleteOpenSeats(date.Date, eid);

			//if (ret == null)
			//{
			//	return View();
			//}
			//return View(ret.Value);
		}
	

		public ActionResult OpenSeats(DateTime date)
		{
			//Set the date here for the other views
			TempData["SelectedDate"] = date;

			var api = new OpenSeatsController(_dbContext);
			//var dt = DateTime.Parse(date);
			var ret = api.GetOpenSeats(date.Date) as OkObjectResult;

			if (ret == null)
		    {
				return View();
			}
			return View(ret.Value);
		}

		// POST: Home
		[HttpPost]
		public ActionResult Create(IFormCollection collection)
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