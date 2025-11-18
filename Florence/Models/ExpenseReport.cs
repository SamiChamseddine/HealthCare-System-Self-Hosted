namespace Florence.Models
{
    public class ExpenseReport
    {
        public int Id { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal TotalHours { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<ExpenseItem> Items { get; set; } = new();

        public override string ToString() => $"Report #{Id} - {Patient.FullName}";
    }
}
