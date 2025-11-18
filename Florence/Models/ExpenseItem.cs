using System.Text.Json.Serialization;

namespace Florence.Models
{
    public class ExpenseItem
    {
        public int Id { get; set; }

        public int ExpenseReportId { get; set; }
        [JsonIgnore]
        public ExpenseReport Report { get; set; } = null!;

        public int NurseId { get; set; }
        public Nurse Nurse { get; set; } = null!;

        public DateOnly Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Hours { get; set; }
        public decimal Amount { get; set; }

        public override string ToString() => $"{Date} - {Description}";
    }
}
