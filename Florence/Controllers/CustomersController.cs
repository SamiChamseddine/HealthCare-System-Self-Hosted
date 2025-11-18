using Florence.Data;
using Florence.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Florence.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<List<CustomerDto>>> GetCustomers()
        {
            var customers = await _context.Customers.ToListAsync();
            return Ok(customers.Select(MapToDto).ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound();
            return Ok(MapToDto(customer));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> CreateCustomer(CreateCustomerDto createDto)
        {
            var customer = new Customer
            {
                Name = createDto.Name,
                Phone = createDto.Phone,
                Address = createDto.Address,
                Email = createDto.Email
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return Ok(MapToDto(customer));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerDto>> UpdateCustomer(int id, CreateCustomerDto updateDto)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound();

            customer.Name = updateDto.Name;
            customer.Phone = updateDto.Phone;
            customer.Address = updateDto.Address;
            customer.Email = updateDto.Email;

            await _context.SaveChangesAsync();
            return Ok(MapToDto(customer));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound();

            var hasInvoices = await _context.Invoices.AnyAsync(i => i.CustomerId == id);
            if (hasInvoices)
                return BadRequest("Cannot delete customer with existing invoices");

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}/invoices")]
        public async Task<ActionResult<List<InvoiceDto>>> GetCustomerInvoices(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound();

            var invoices = await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Items)
                .Where(i => i.CustomerId == id)
                .ToListAsync();

            return Ok(invoices.Select(InvoicesController.MapToDto).ToList());
        }

        private CustomerDto MapToDto(Customer customer)
        {
            return new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone,
                Address = customer.Address,
                Email = customer.Email,
                InvoiceCount = customer.Invoices.Count
            };
        }
    }
}