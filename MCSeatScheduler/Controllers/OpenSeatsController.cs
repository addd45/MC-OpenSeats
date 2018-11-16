using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MCSeatScheduler;
using MCSeatScheduler.Model;

namespace MCSeatScheduler.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OpenSeatsController : ControllerBase
    {
        private readonly MCDBContext _dbContext;

        public OpenSeatsController(MCDBContext context)
        {
            _dbContext = context;
        }

        // GET: return everything in DB (use for debugging only)
        [HttpGet("api")]
        public IEnumerable<Model.OpenSeats> GetOpenSeats()
        {
            return _dbContext.OpenSeats;
        }

        // GET: api/OpenSeats/5
        [HttpGet("api/{id}")]
        public IActionResult GetOpenSeats([FromRoute] DateTime id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var OpenSeats = _dbContext.OpenSeats.Where(c => c.Date.Date == id.Date);

            if (OpenSeats == null)
            {
                return NotFound();
            }

            return Ok(OpenSeats);
        }

        // PUT: api/OpenSeats/5
        [HttpPut("api")]
        public async Task<IActionResult> PutOpenSeats([FromBody] Model.OpenSeats OpenSeats)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Check if this record exists already
            //if it does, return
            if (OpenSeatsExists(OpenSeats))
            {
                return NoContent();
            }
            //if it doesnt, add it
            else
            {
                _dbContext.OpenSeats.Add(OpenSeats);
            }

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception) { throw; }

            return NoContent();
        }

        //Dont use this one...
        // POST: api/OpenSeats
        [HttpPost("api")]
        public async Task<IActionResult> PostOpenSeats([FromBody] Model.OpenSeats OpenSeats)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dbContext.OpenSeats.Add(OpenSeats);
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (OpenSeatsExists(OpenSeats))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetOpenSeats", new { id = OpenSeats.Date }, OpenSeats);
        }

        // DELETE: api/OpenSeats/5
        [HttpDelete("api/{date}/{eid}")]
        public async Task<IActionResult> DeleteOpenSeats([FromRoute] DateTime date, [FromRoute] string eid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var OpenSeats = await _dbContext.OpenSeats.FindAsync(date, eid);
            if (OpenSeats == null)
            {
                return NotFound();
            }

            _dbContext.OpenSeats.Remove(OpenSeats);
            await _dbContext.SaveChangesAsync();

            return Ok(OpenSeats);
        }

        private bool OpenSeatsExists(Model.OpenSeats seats)
        {
            return _dbContext.OpenSeats.Any(c => (c.Date.Date == seats.Date.Date && c.EmployeeId.ToUpper() == seats.EmployeeId.ToUpper()));
        }
    }
}