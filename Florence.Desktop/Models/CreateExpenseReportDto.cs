
namespace Florence.Desktop.Models;

    public class CreateExpenseReportDto
    {
        public int PatientId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalHours { get; set; }
        public List<CreateExpenseItemDto> Items { get; set; } = new();
    }

