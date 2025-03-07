namespace DaneshkarEf.Controllers
{
    using DaneshkarEf.Dto;
    using DaneshkarEf.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using X.PagedList;
    using X.PagedList.EF;
    using Microsoft.AspNetCore.JsonPatch;

    namespace BookStore.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class BooksController : ControllerBase
        {
            private readonly ApplicationDbContext _context;

            public BooksController(ApplicationDbContext context)
            {
                _context = context;
            }


            [HttpGet]
            public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooks()
            {
                var books = await _context.Books
                    .Include(b => b.Authors)
                    .ThenInclude(ba => ba.Author)  // لود کردن اطلاعات نویسندگان
                    .Select(b => new BookDTO
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Authors = b.Authors.Select(ba => new BookAuthorDTO
                        {
                            AuthorId = ba.AuthorId,
                            AuthorName = ba.Author.Name
                        }).ToList(),
                        Description = b.Description
                    })
                    .ToListAsync();

                return Ok(books);
            }


            [HttpGet("{id}")]
            public async Task<ActionResult<BookDTO>> GetBook(int id)
            {
                var book = await _context.Books
                    .Include(b => b.Authors)
                    .ThenInclude(ba => ba.Author)
                    .Where(b => b.Id == id)
                    .Select(b => new BookDTO
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Authors = b.Authors.Select(ba => new BookAuthorDTO
                        {
                            AuthorId = ba.AuthorId,
                            AuthorName = ba.Author.Name
                        }).ToList(),
                        Description = b.Description
                    })
                    .FirstOrDefaultAsync();

                if (book == null)
                {
                    return NotFound();
                }

                return Ok(book);
            }





            [HttpPatch("{id}")]
            public async Task<IActionResult> PatchBook(int id, [FromBody] JsonPatchDocument<Book> patchDoc)
            {
                if (patchDoc == null)
                {
                    return BadRequest("Invalid patch document.");
                }

                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    return NotFound();
                }

              
                patchDoc.ApplyTo(book);

             
                if (!TryValidateModel(book))
                {
                    return BadRequest(ModelState);
                }

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(id))
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


            [HttpPost]
            public async Task<ActionResult<Book>> PostBook(Book book)
            {
                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
            }


            [HttpPut("{id}")]
            public async Task<IActionResult> PutBook(int id, Book book)
            {
                if (id != book.Id)
                {
                    return BadRequest();
                }

                _context.Entry(book).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(id))
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


            [HttpGet("titles")]
            public async Task<IEnumerable<string>> GetBookTitles()
            {
                return await _context.Books
                    .Select(b => b.Title)
                    .ToListAsync();
            }


            [HttpGet("paged")]
            public async Task<ActionResult<IPagedList<BookDTO>>> GetPagedBooks(int pageNumber = 1, int pageSize = 10)
            {
                if (pageNumber < 1 || pageSize < 1)
                {
                    return BadRequest("Page number and page size must be greater than 0.");
                }

                var books = await _context.Books
                    .Include(b => b.Authors)
                    .ThenInclude(ba => ba.Author)
                    .OrderBy(b => b.Id)
                    .Select(b => new BookDTO
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Authors = b.Authors.Select(ba => new BookAuthorDTO
                        {
                            AuthorId = ba.AuthorId,
                            AuthorName = ba.Author.Name
                        }).ToList(),
                        Description = b.Description
                    })
                    .ToPagedListAsync(pageNumber, pageSize);

                return Ok(books);
            }


            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteBook(int id)
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    return NotFound();
                }

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();

                return NoContent();
            }


            private bool BookExists(int id)
            {
                return _context.Books.Any(e => e.Id == id);
            }
        }
    }
}
