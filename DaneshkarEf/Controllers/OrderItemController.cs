﻿namespace DaneshkarEf.Controllers
{
    using DaneshkarEf.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;


        [Route("api/[controller]")]
        [ApiController]
        public class OrderItemController : ControllerBase
        {
            private readonly ApplicationDbContext _context;

            public OrderItemController(ApplicationDbContext context)
            {
                _context = context;
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItems()
            {
                return await _context.OrderItems
                    .Include(oi => oi.Order)
                    .Include(oi => oi.Book)
                    .ToListAsync();
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<OrderItem>> GetOrderItem(int id)
            {
                var orderItem = await _context.OrderItems
                    .Include(oi => oi.Order)
                    .Include(oi => oi.Book)
                    .FirstOrDefaultAsync(oi => oi.Id == id);

                if (orderItem == null)
                {
                    return NotFound();
                }

                return orderItem;
            }

            [HttpPost]
            public async Task<ActionResult<OrderItem>> PostOrderItem(OrderItem orderItem)
            {
                _context.OrderItems.Add(orderItem);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetOrderItem", new { id = orderItem.Id }, orderItem);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> PutOrderItem(int id, OrderItem orderItem)
            {
                if (id != orderItem.Id)
                {
                    return BadRequest();
                }

                _context.Entry(orderItem).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderItemExists(id))
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
            public async Task<IActionResult> DeleteOrderItem(int id)
            {
                var orderItem = await _context.OrderItems.FindAsync(id);
                if (orderItem == null)
                {
                    return NotFound();
                }

                _context.OrderItems.Remove(orderItem);
                await _context.SaveChangesAsync();

                return NoContent();
            }

            private bool OrderItemExists(int id)
            {
                return _context.OrderItems.Any(oi => oi.Id == id);
            }
        }
    }
