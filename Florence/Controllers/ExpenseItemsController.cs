using Florence.Data;
using Florence.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Florence.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ExpenseItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<ExpenseItemDto>>> GetExpenseItems()
        {
            var items = await _context.ExpenseItems
                .Include(i => i.Report)
                .Include(i => i.Nurse)
                .ToListAsync();

            return Ok(items.Select(MapToDto).ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseItemDto>> GetExpenseItem(int id)
        {
            var item = await _context.ExpenseItems
                .Include(i => i.Report)
                .Include(i => i.Nurse)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (item == null) return NotFound();
            return Ok(MapToDto(item));
        }

        [HttpPost]
        public async Task<ActionResult<ExpenseItemDto>> CreateExpenseItem(CreateExpenseItemDto createDto)
        {
            var report = await _context.ExpenseReports.FindAsync(createDto.ExpenseReportId);
            if (report == null) return BadRequest("Expense report not found");

            var nurse = await _context.Nurses.FindAsync(createDto.NurseId);
            if (nurse == null) return BadRequest("Nurse not found");

            var item = new ExpenseItem
            {
                ExpenseReportId = createDto.ExpenseReportId,
                NurseId = createDto.NurseId,
                Date = createDto.Date,
                Description = createDto.Description,
                Hours = createDto.Hours,
                Amount = createDto.Amount
            };

            _context.ExpenseItems.Add(item);
            await _context.SaveChangesAsync();

            return Ok(MapToDto(item));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ExpenseItemDto>> UpdateExpenseItem(int id, CreateExpenseItemDto updateDto)
        {
            var item = await _context.ExpenseItems.FindAsync(id);
            if (item == null) return NotFound();

            item.NurseId = updateDto.NurseId;
            item.Date = updateDto.Date;
            item.Description = updateDto.Description;
            item.Hours = updateDto.Hours;
            item.Amount = updateDto.Amount;

            await _context.SaveChangesAsync();
            return Ok(MapToDto(item));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteExpenseItem(int id)
        {
            var item = await _context.ExpenseItems.FindAsync(id);
            if (item == null) return NotFound();

            _context.ExpenseItems.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        public static ExpenseItemDto MapToDto(ExpenseItem item)
        {
            return new ExpenseItemDto
            {
                Id = item.Id,
                ExpenseReportId = item.ExpenseReportId,
                ReportPeriod = $"{item.Report.StartDate} to {item.Report.EndDate}",
                NurseId = item.NurseId,
                NurseName = item.Nurse.FullName,
                Date = item.Date,
                Description = item.Description,
                Hours = item.Hours,
                Amount = item.Amount
            };
        }
    }
}