using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNet6APIEFCoreSQLite.Models;

namespace DotNet6APIEFCoreSQLite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KaijusController : ControllerBase
    {
        private readonly KaijuDBContext _context;

        public KaijusController(KaijuDBContext context)
        {
            _context = context;
        }

        // GET: api/Kaijus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kaiju>>> GetKaijus()
        {
            return await _context.Kaijus.ToListAsync();
        }

        // GET: api/Kaijus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Kaiju>> GetKaiju(int id)
        {
            var kaiju = await _context.Kaijus.FindAsync(id);

            if (kaiju == null)
            {
                return NotFound();
            }

            return kaiju;
        }

        // PUT: api/Kaijus/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKaiju(int id, Kaiju kaiju)
        {
            if (id != kaiju.Id)
            {
                return BadRequest();
            }

            _context.Entry(kaiju).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KaijuExists(id))
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

        // POST: api/Kaijus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Kaiju>> PostKaiju(Kaiju kaiju)
        {
            _context.Kaijus.Add(kaiju);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKaiju", new { id = kaiju.Id }, kaiju);
        }

        // DELETE: api/Kaijus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKaiju(int id)
        {
            var kaiju = await _context.Kaijus.FindAsync(id);
            if (kaiju == null)
            {
                return NotFound();
            }

            _context.Kaijus.Remove(kaiju);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KaijuExists(int id)
        {
            return _context.Kaijus.Any(e => e.Id == id);
        }
    }
}
