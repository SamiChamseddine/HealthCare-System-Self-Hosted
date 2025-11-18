namespace Florence.Models
{
    public class ExpenseReportDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalHours { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ExpenseItemDto> Items { get; set; } = new();
    }
}
