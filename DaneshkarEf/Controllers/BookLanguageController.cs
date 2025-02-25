using Microsoft.AspNetCore.Mvc;

namespace DaneshkarEf.Controllers
{
    using DaneshkarEf.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

 
        [Route("api/[controller]")]
        [ApiController]
        public class BookLanguageController : ControllerBase
        {
            private readonly ApplicationDbContext _context;

            public BookLanguageController(ApplicationDbContext context)
            {
                _context = context;
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<BookLanguage>>> GetBookLanguages()
            {
                return await _context.BookLanguages.ToListAsync();
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<BookLanguage>> GetBookLanguage(int id)
            {
                var bookLanguage = await _context.BookLanguages.FindAsync(id);

                if (bookLanguage == null)
                {
                    return NotFound();
                }

                return bookLanguage;
            }

            [HttpPost]
            public async Task<ActionResult<BookLanguage>> PostBookLanguage(BookLanguage bookLanguage)
            {
                _context.BookLanguages.Add(bookLanguage);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetBookLanguage", new { id = bookLanguage.Id }, bookLanguage);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> PutBookLanguage(int id, BookLanguage bookLanguage)
            {
                if (id != bookLanguage.Id)
                {
                    return BadRequest();
                }

                _context.Entry(bookLanguage).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookLanguageExists(id))
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

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteBookLanguage(int id)
            {
                var bookLanguage = await _context.BookLanguages.FindAsync(id);
                if (bookLanguage == null)
                {
                    return NotFound();
                }

                _context.BookLanguages.Remove(bookLanguage);
                await _context.SaveChangesAsync();

                return NoContent();
            }

            private bool BookLanguageExists(int id)
            {
                return _context.BookLanguages.Any(bl => bl.Id == id);
            }
        }
    }