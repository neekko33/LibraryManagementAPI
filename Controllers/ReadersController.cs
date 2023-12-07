using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadersController : ControllerBase
    {
        private readonly libraryContext _context;

        public ReadersController(libraryContext context)
        {
            _context = context;
        }

        // GET: api/Readers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reader>>> GetReaders()
        {
            return await _context.Readers.ToListAsync();
        }

        // GET: api/Readers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reader>> GetReader(int id)
        {
            var reader = await _context.Readers.FindAsync(id);

            if (reader == null)
            {
                return NotFound();
            }

            return reader;
        }

        // PUT: api/Readers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReader(int id, Reader reader)
        {
            if (id != reader.ReaderID)
            {
                return BadRequest();
            }

            _context.Entry(reader).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReaderExists(id))
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

        // POST: api/Readers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Reader>> PostReader(Reader reader)
        {
            _context.Readers.Add(reader);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReader", new { id = reader.ReaderID }, reader);
        }

        // DELETE: api/Readers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReader(int id)
        {
            var reader = await _context.Readers.FindAsync(id);
            if (reader == null)
            {
                return NotFound();
            }

            _context.Readers.Remove(reader);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReaderExists(int id)
        {
            return _context.Readers.Any(e => e.ReaderID == id);
        }
    }
}
