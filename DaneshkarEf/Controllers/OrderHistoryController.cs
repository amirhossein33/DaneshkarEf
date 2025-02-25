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
        public class OrderHistoryController : ControllerBase
        {
            private readonly ApplicationDbContext _context;

            public OrderHistoryController(ApplicationDbContext context)
            {
                _context = context;
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<OrderHistory>>> GetOrderHistories()
            {
                return await _context.OrderHistories
                    .Include(oh => oh.Order)
                    .ToListAsync();
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<OrderHistory>> GetOrderHistory(int id)
            {
                var orderHistory = await _context.OrderHistories
                    .Include(oh => oh.Order)
                    .FirstOrDefaultAsync(oh => oh.Id == id);

                if (orderHistory == null)
                {
                    return NotFound();
                }

                return orderHistory;
            }

            [HttpPost]
            public async Task<ActionResult<OrderHistory>> PostOrderHistory(OrderHistory orderHistory)
            {
                _context.OrderHistories.Add(orderHistory);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetOrderHistory", new { id = orderHistory.Id }, orderHistory);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> PutOrderHistory(int id, OrderHistory orderHistory)
            {
                if (id != orderHistory.Id)
                {
                    return BadRequest();
                }

                _context.Entry(orderHistory).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderHistoryExists(id))
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
            public async Task<IActionResult> DeleteOrderHistory(int id)
            {
                var orderHistory = await _context.OrderHistories.FindAsync(id);
                if (orderHistory == null)
                {
                    return NotFound();
                }

                _context.OrderHistories.Remove(orderHistory);
                await _context.SaveChangesAsync();

                return NoContent();
            }

            private bool OrderHistoryExists(int id)
            {
                return _context.OrderHistories.Any(oh => oh.Id == id);
            }
        }
    }