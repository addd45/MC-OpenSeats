using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MCSeatScheduler.Controllers
{
    [Route("[controller]")]
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

        // GET: Home/Details/5
        [Route("{date}")]
        public ActionResult OpenSeats(DateTime date)
        {
            var api = new OpenSeatsController(_dbContext);
            //var dt = DateTime.Parse(date);
            var ret = api.GetOpenSeats(date.Date) as OkObjectResult;
            
            
            if (ret == null)
            {
                return View();
            }
            return View(ret.Value);
        }

        [ActionName("Delete")]
        [HttpDelete("{user}")]
        public ActionResult Delete(string user)
        {
            return NoContent(); 
        }

        // POST: Home
        [HttpPost]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                var date = collection["txtDate"];
                return RedirectToAction("OpenSeats", new { date = date.ToString()  });
            }
            catch
            {
                return View();
            }
        }
    }
}