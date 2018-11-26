using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MCSeatScheduler.Controllers
{
    [Authorize]
	[Route("[controller]")]
	public class HomeController : Controller
	{
		private readonly OpenSeatsController _apiController;

		public HomeController(OpenSeatsController openSeatsController)
		{
            _apiController = openSeatsController;
		}

        // GET: Home
        
        public IActionResult Index()
		{
			//Page where they pick a date n stuff
			return View();
		}

		[HttpGet("Reserve/{date}")]
		public async Task<IActionResult> Reserve([FromRoute]DateTime date)
		{
			string eid = HttpContext.User.Identity.Name;

			if (string.IsNullOrEmpty(eid)){
				return RedirectToAction("Index", "Login");
			}
			return await ReserveNew(date, eid);
		}

		[Route("ReserveNow")]
		public async Task<IActionResult> ReserveNew(DateTime date, string eid)
		{		
			//Cant reserve in the past	 
			if (date.Date < DateTime.Now.Date){
				return BadRequest("Cant reserve a date in the past");
			}
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
					 await _apiController.ReserveSeat(date, eid);
					 return RedirectToAction("ViewOpenSeats", "Home", new {date = date.ToString("MM-dd-yy")});
				}
			}
			else{
				throw new Exception("Error getting available seats");
			}

		}

		[HttpGet("Delete")]
		public async Task<IActionResult> Delete(DateTime date, string eid)
		{			
			//var dt = DateTime.Parse(date);
			var ret = await _apiController.DeleteOpenSeats(date.Date, eid);
			return RedirectToAction("OpenSeats", "Home", new
			{
				date = date.ToString()
			});
		}

		[HttpGet("{date}")]
		public IActionResult ViewOpenSeats([FromRoute] DateTime date)
		{
			//Set the date here for the other views
			TempData["SelectedDate"] = date;

			//var dt = DateTime.Parse(date);
			var ret = _apiController.GetOpenSeats(date.Date) as OkObjectResult;

			if (ret == null)
		    {
				return View();
			}
			return View("OpenSeats", ret.Value);
		}
	
		[HttpPost]
		public IActionResult OpenSeats(DateTime date)
		{
			return RedirectToAction("ViewOpenSeats", "Home", new {date = date.ToString("MM-dd-yy")});
		}
	}
}