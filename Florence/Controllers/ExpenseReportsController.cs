using Florence.Data;
using Florence.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Florence.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseReportsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ExpenseReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<ExpenseReportDto>>> GetExpenseReports()
        {
            var reports = await _context.ExpenseReports
                .Include(r => r.Patient)
                .Include(r => r.Items)
                    .ThenInclude(i => i.Nurse)
                .ToListAsync();

            return Ok(reports.Select(MapToDto).ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseReportDto>> GetExpenseReport(int id)
        {
            var report = await _context.ExpenseReports
                .Include(r => r.Patient)
                .Include(r => r.Items)
                    .ThenInclude(i => i.Nurse)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (report == null) return NotFound();
            return Ok(MapToDto(report));
        }

        [HttpPost]
        public async Task<ActionResult<ExpenseReportDto>> CreateExpenseReport(CreateExpenseReportDto createDto)
        {
            var patient = await _context.Patients.FindAsync(createDto.PatientId);
            if (patient == null) return BadRequest("Patient not found");

            var report = new ExpenseReport
            {
                PatientId = createDto.PatientId,
                StartDate = createDto.StartDate,
                EndDate = createDto.EndDate,
                TotalAmount = createDto.TotalAmount,
                TotalHours = createDto.TotalHours,
                Items = createDto.Items.Select(item => new ExpenseItem
                {
                    NurseId = item.NurseId,
                    Date = item.Date,
                    Description = item.Description,
                    Hours = item.Hours,
                    Amount = item.Amount
                }).ToList()
            };

            _context.ExpenseReports.Add(report);
            await _context.SaveChangesAsync();
            var savedReport = await _context.ExpenseReports
                .Include(r => r.Patient)
                .Include(r => r.Items)
                    .ThenInclude(i => i.Nurse)
                .FirstOrDefaultAsync(r => r.Id == report.Id);
            return Ok(MapToDto(savedReport!));

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ExpenseReportDto>> UpdateExpenseReport(int id, CreateExpenseReportDto updateDto)
        {
            var report = await _context.ExpenseReports
                .Include(r => r.Patient)
                .Include(r => r.Items)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (report == null)
                return NotFound();

            report.PatientId = updateDto.PatientId;
            report.StartDate = updateDto.StartDate;
            report.EndDate = updateDto.EndDate;
            report.TotalAmount = updateDto.TotalAmount;
            report.TotalHours = updateDto.TotalHours;

            report.Items.Clear();
            report.Items = updateDto.Items.Select(i => new ExpenseItem
            {
                NurseId = i.NurseId,
                Date = i.Date,
                Description = i.Description,
                Hours = i.Hours,
                Amount = i.Amount
            }).ToList();

            await _context.SaveChangesAsync();

            var saved = await _context.ExpenseReports
                .Include(r => r.Patient)
                .Include(r => r.Items)
                    .ThenInclude(i => i.Nurse)
                .FirstOrDefaultAsync(r => r.Id == id);

            return Ok(ExpenseReportsController.MapToDto(saved!));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpenseReport(int id)
        {
            var report = await _context.ExpenseReports.FindAsync(id);
            if (report == null)
                return NotFound();

            _context.ExpenseReports.Remove(report);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        public static ExpenseReportDto MapToDto(ExpenseReport report)
        {
            return new ExpenseReportDto
            {
                Id = report.Id,
                PatientId = report.PatientId,
                PatientName = report.Patient.FullName,
                StartDate = report.StartDate,
                EndDate = report.EndDate,
                TotalAmount = report.TotalAmount,
                TotalHours = report.TotalHours,
                CreatedAt = report.CreatedAt,
                Items = report.Items.Select(ExpenseItemsController.MapToDto).ToList()
            };
        }
    }
}