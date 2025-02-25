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
    public class CustomerAddressController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CustomerAddressController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerAddress>>> GetCustomerAddresses()
        {
            return await _context.CustomerAddresses
                .Include(ca => ca.Customer)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerAddress>> GetCustomerAddress(int id)
        {
            var customerAddress = await _context.CustomerAddresses
                .Include(ca => ca.Customer)
                .FirstOrDefaultAsync(ca => ca.Id == id);

            if (customerAddress == null)
            {
                return NotFound();
            }

            return customerAddress;
        }

        [HttpPost]
        public async Task<ActionResult<CustomerAddress>> PostCustomerAddress(CustomerAddress customerAddress)
        {
            _context.CustomerAddresses.Add(customerAddress);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomerAddress", new { id = customerAddress.Id }, customerAddress);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerAddress(int id, CustomerAddress customerAddress)
        {
            if (id != customerAddress.Id)
            {
                return BadRequest();
            }

            _context.Entry(customerAddress).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerAddressExists(id))
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
        public async Task<IActionResult> DeleteCustomerAddress(int id)
        {
            var customerAddress = await _context.CustomerAddresses.FindAsync(id);
            if (customerAddress == null)
            {
                return NotFound();
            }

            _context.CustomerAddresses.Remove(customerAddress);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerAddressExists(int id)
        {
            return _context.CustomerAddresses.Any(ca => ca.Id == id);
        }
    }
}