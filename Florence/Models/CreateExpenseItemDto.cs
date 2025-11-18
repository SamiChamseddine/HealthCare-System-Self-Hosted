namespace Florence.Models
{
    public class CreateExpenseItemDto
    {
        public int ExpenseReportId { get; set; }
        public int NurseId { get; set; }
        public DateOnly Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Hours { get; set; }
        public decimal Amount { get; set; }
    }
}
