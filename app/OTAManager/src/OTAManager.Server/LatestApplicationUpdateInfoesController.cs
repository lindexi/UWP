using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OTAManager.Server.Controllers;
using OTAManager.Server.Data;

namespace OTAManager.Server
{
    [Route("api/[controller]")]
    [ApiController]
    public class LatestApplicationUpdateInfoesController : ControllerBase
    {
        private readonly OTAManagerServerContext _context;

        public LatestApplicationUpdateInfoesController(OTAManagerServerContext context)
        {
            _context = context;
        }

        // GET: api/LatestApplicationUpdateInfoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUpdateInfo>>> GetLatestApplicationUpdateInfo()
        {
            return await _context.LatestApplicationUpdateInfo.ToListAsync();
        }

        // GET: api/LatestApplicationUpdateInfoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUpdateInfo>> GetLatestApplicationUpdateInfo(int id)
        {
            var latestApplicationUpdateInfo = await _context.LatestApplicationUpdateInfo.FindAsync(id);

            if (latestApplicationUpdateInfo == null)
            {
                return NotFound();
            }

            return latestApplicationUpdateInfo;
        }

        // PUT: api/LatestApplicationUpdateInfoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLatestApplicationUpdateInfo(int id, ApplicationUpdateInfo applicationUpdateInfo)
        {
            if (id != applicationUpdateInfo.Id)
            {
                return BadRequest();
            }

            _context.Entry(applicationUpdateInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LatestApplicationUpdateInfoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/LatestApplicationUpdateInfoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApplicationUpdateInfo>> PostLatestApplicationUpdateInfo(ApplicationUpdateInfo applicationUpdateInfo)
        {
            _context.LatestApplicationUpdateInfo.Add(applicationUpdateInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLatestApplicationUpdateInfo", new { id = applicationUpdateInfo.Id }, applicationUpdateInfo);
        }

        // DELETE: api/LatestApplicationUpdateInfoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLatestApplicationUpdateInfo(int id)
        {
            var latestApplicationUpdateInfo = await _context.LatestApplicationUpdateInfo.FindAsync(id);
            if (latestApplicationUpdateInfo == null)
            {
                return NotFound();
            }

            _context.LatestApplicationUpdateInfo.Remove(latestApplicationUpdateInfo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LatestApplicationUpdateInfoExists(int id)
        {
            return _context.LatestApplicationUpdateInfo.Any(e => e.Id == id);
        }
    }
}
