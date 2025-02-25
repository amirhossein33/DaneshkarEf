using DaneshkarEf.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace DaneshkarEf.Controllers
{



    [Route("api/[controller]")]
        [ApiController]
        public class BookAuthorController : ControllerBase
        {
            private readonly ApplicationDbContext _context;

            public BookAuthorController(ApplicationDbContext context)
            {
                _context = context;
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<BookAuthor>>> GetBookAuthors()
            {
                return await _context.BookAuthors
                    .Include(ba => ba.Book)
                    .Include(ba => ba.Author)
                    .ToListAsync();
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<BookAuthor>> GetBookAuthor(int id)
            {
                var bookAuthor = await _context.BookAuthors
                    .Include(ba => ba.Book)
                    .Include(ba => ba.Author)
                    .FirstOrDefaultAsync(ba => ba.Id == id);

                if (bookAuthor == null)
                {
                    return NotFound();
                }

                return bookAuthor;
            }

            [HttpPost]
            public async Task<ActionResult<BookAuthor>> PostBookAuthor(BookAuthor bookAuthor)
            {
                _context.BookAuthors.Add(bookAuthor);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetBookAuthor", new { id = bookAuthor.Id }, bookAuthor);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> PutBookAuthor(int id, BookAuthor bookAuthor)
            {
                if (id != bookAuthor.Id)
                {
                    return BadRequest();
                }

                _context.Entry(bookAuthor).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookAuthorExists(id))
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
            public async Task<IActionResult> DeleteBookAuthor(int id)
            {
                var bookAuthor = await _context.BookAuthors.FindAsync(id);
                if (bookAuthor == null)
                {
                    return NotFound();
                }

                _context.BookAuthors.Remove(bookAuthor);
                await _context.SaveChangesAsync();

                return NoContent();
            }

            private bool BookAuthorExists(int id)
            {
                return _context.BookAuthors.Any(ba => ba.Id == id);
            }
        }
    }
