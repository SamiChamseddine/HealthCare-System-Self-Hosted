
namespace Florence.Desktop.Models;
    public class ExpenseItemDto
    {
        public int Id { get; set; }
        public int ExpenseReportId { get; set; }
        public string ReportPeriod { get; set; } = string.Empty;
        public int NurseId { get; set; }
        public string NurseName { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Hours { get; set; }
        public decimal Amount { get; set; }
    }

