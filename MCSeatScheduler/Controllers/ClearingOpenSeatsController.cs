﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MCSeatScheduler;

namespace MCSeatScheduler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClearingOpenSeatsController : ControllerBase
    {
        private readonly MCDBContext _dbContext;

        public ClearingOpenSeatsController(MCDBContext context)
        {
            _dbContext = context;
        }

        // GET: return everything in DB (use for debugging only)
        [HttpGet]
        public IEnumerable<ClearingOpenSeats> GetClearingOpenSeats()
        {
            return _dbContext.ClearingOpenSeats;
        }

        // GET: api/ClearingOpenSeats/5
        [HttpGet("{id}")]
        public IActionResult GetClearingOpenSeats([FromRoute] DateTime id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var clearingOpenSeats = _dbContext.ClearingOpenSeats.Where(c => c.Date.Date == id.Date);

            if (clearingOpenSeats == null)
            {
                return NotFound();
            }

            return Ok(clearingOpenSeats);
        }

        // PUT: api/ClearingOpenSeats/5
        [HttpPut]
        public async Task<IActionResult> PutClearingOpenSeats([FromBody] ClearingOpenSeats clearingOpenSeats)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dbContext.ClearingOpenSeats.Add(clearingOpenSeats);            

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception) { throw; }

            return NoContent();
        }

        // POST: api/ClearingOpenSeats
        [HttpPost]
        public async Task<IActionResult> PostClearingOpenSeats([FromBody] ClearingOpenSeats clearingOpenSeats)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dbContext.ClearingOpenSeats.Add(clearingOpenSeats);
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ClearingOpenSeatsExists(clearingOpenSeats.Date))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetClearingOpenSeats", new { id = clearingOpenSeats.Date }, clearingOpenSeats);
        }

        // DELETE: api/ClearingOpenSeats/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClearingOpenSeats([FromRoute] DateTime id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var clearingOpenSeats = await _dbContext.ClearingOpenSeats.FindAsync(id);
            if (clearingOpenSeats == null)
            {
                return NotFound();
            }

            _dbContext.ClearingOpenSeats.Remove(clearingOpenSeats);
            await _dbContext.SaveChangesAsync();

            return Ok(clearingOpenSeats);
        }

        private bool ClearingOpenSeatsExists(DateTime id)
        {
            return _dbContext.ClearingOpenSeats.Any(e => e.Date == id);
        }
    }
}