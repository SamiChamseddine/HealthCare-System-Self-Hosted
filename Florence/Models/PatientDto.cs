namespace Florence.Models
{
    public class PatientDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public int ExpenseReportCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
