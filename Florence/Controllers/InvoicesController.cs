using Florence.Data;
using Florence.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Florence.Models.Invoice;

namespace Florence.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InvoicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        private async Task<int> GetNextInvoiceNumber()
        {
            var lastNumber = await _context.Invoices
                .MaxAsync(i => (int?)i.InvoiceNumber) ?? 0;
            return lastNumber + 1;
        }

        [HttpPost]
        public async Task<ActionResult<InvoiceDto>> CreateInvoice(CreateInvoiceDto createDto)
        {
            var customer = await _context.Customers.FindAsync(createDto.CustomerId);
            if (customer == null)
                return BadRequest("Customer not found");

            var invoice = new Invoice
            {
                InvoiceNumber = await GetNextInvoiceNumber(),
                CustomerId = createDto.CustomerId,
                DueDate = createDto.DueDate,
                DateCreated = DateTime.SpecifyKind(
    createDto.DateCreated.ToDateTime(TimeOnly.MinValue),
    DateTimeKind.Utc
),
                Subtotal = createDto.Subtotal,
                DiscountPercent = createDto.DiscountPercent,
                DiscountAmount = createDto.DiscountAmount,
                VatRate = createDto.VatRate,
                VatAmount = createDto.VatAmount,
                Total = createDto.Total,
                Currency = createDto.Currency,
                PaymentMethod = createDto.PaymentMethod,
                Notes = createDto.Notes,
                Items = createDto.Items.Select(item => new InvoiceItem
                {
                    Description = item.Description,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            };

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return Ok(MapToDto(invoice));
        }

        [HttpGet]
        public async Task<ActionResult<List<InvoiceDto>>> GetInvoices()
        {
            var invoices = await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Items)
                .ToListAsync();

            return Ok(invoices.Select(MapToDto).ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InvoiceDto>> GetInvoice(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
                return NotFound();

            return Ok(MapToDto(invoice));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<InvoiceDto>> UpdateInvoice(int id, CreateInvoiceDto updateDto)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
                return NotFound();

            invoice.CustomerId = updateDto.CustomerId;
            invoice.DueDate = updateDto.DueDate;
            invoice.DateCreated = DateTime.SpecifyKind(
    updateDto.DateCreated.ToDateTime(TimeOnly.MinValue),
    DateTimeKind.Utc
);
            invoice.Subtotal = updateDto.Subtotal;
            invoice.DiscountPercent = updateDto.DiscountPercent;
            invoice.DiscountAmount = updateDto.DiscountAmount;
            invoice.VatRate = updateDto.VatRate;
            invoice.VatAmount = updateDto.VatAmount;
            invoice.Total = updateDto.Total;
            invoice.Currency = updateDto.Currency;
            invoice.PaymentMethod = updateDto.PaymentMethod;
            invoice.Notes = updateDto.Notes;

            invoice.Items.Clear();
            invoice.Items = updateDto.Items.Select(item => new InvoiceItem
            {
                Description = item.Description,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            }).ToList();

            invoice.LastModified = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(MapToDto(invoice));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInvoice(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null) return NotFound();

            invoice.Delete();
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}/pay")]
        public async Task<ActionResult<InvoiceDto>> MarkAsPaid(int id, string paymentMethod = "Cash")
        {
            var invoice = await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
                return NotFound();

            invoice.AddPayment(invoice.Total, paymentMethod);
            await _context.SaveChangesAsync();

            return Ok(MapToDto(invoice));
        }


        [HttpGet("overdue")]
        public async Task<ActionResult<List<InvoiceDto>>> GetOverdueInvoices()
        {
            var overdueInvoices = await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Items)
                .Where(i => i.Status == InvoiceStatus.Late ||
                           (i.DueDate < DateOnly.FromDateTime(DateTime.UtcNow) && i.Status == InvoiceStatus.Pending))
                .ToListAsync();

            return Ok(overdueInvoices.Select(MapToDto).ToList());
        }

        public static InvoiceDto MapToDto(Invoice invoice)
        {
            if (invoice == null)
                throw new ArgumentNullException(nameof(invoice));

            try
            {
                if (invoice.Customer == null)
                    throw new InvalidOperationException("Invoice.Customer is null");

                if (invoice.Items == null)
                    throw new InvalidOperationException("Invoice.Items is null");

                return new InvoiceDto
                {
                    Id = invoice.Id,
                    InvoiceNumber = invoice.InvoiceNumber,
                    DateCreated = invoice.DateCreated,
                    DueDate = invoice.DueDate,
                    CustomerId = invoice.CustomerId,
                    CustomerName = invoice.Customer?.Name ?? "Unknown Customer",
                    Subtotal = invoice.Subtotal,
                    DiscountPercent = invoice.DiscountPercent,
                    DiscountAmount = invoice.DiscountAmount,
                    VatRate = invoice.VatRate,
                    VatAmount = invoice.VatAmount,
                    Total = invoice.Total,
                    Status = invoice.Status,
                    PaymentDate = invoice.PaymentDate,
                    Currency = invoice.Currency,
                    PaymentMethod = invoice.PaymentMethod,
                    Notes = invoice.Notes,
                    Items = invoice.Items?.Select(item => new InvoiceItemDto
                    {
                        Id = item.Id,
                        Description = item.Description ?? "",
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    }).ToList() ?? new List<InvoiceItemDto>()
                };
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error mapping invoice ID {invoice?.Id} to DTO: {ex.Message}", ex);
            }
        }

        [HttpPatch("check-late")]
        public async Task<ActionResult<int>> CheckAndMarkLateInvoices()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var overdueInvoices = await _context.Invoices
                .Where(i => i.Status == InvoiceStatus.Pending && i.DueDate < today)
                .ToListAsync();

            foreach (var invoice in overdueInvoices)
                invoice.Status = InvoiceStatus.Late;

            await _context.SaveChangesAsync();
            return Ok(new { updated = overdueInvoices.Count });
        }

    }
}
