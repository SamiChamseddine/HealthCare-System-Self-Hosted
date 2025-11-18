using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Florence.Models
{
    public enum Gender
    {
        Male,
        Female,
        Other
    }

    public class Patient
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public DateOnly DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public string EmergencyContact { get; set; } = string.Empty;
        public string EmergencyRelationship { get; set; } = string.Empty;
        public string EmergencyPhone { get; set; } = string.Empty;

        public string InsuranceProvider { get; set; } = string.Empty;
        public string PolicyNumber { get; set; } = string.Empty;

        public string MedicalConditionsJson { get; set; } = "[]"; 
        public string CurrentMedications { get; set; } = string.Empty;
        public string Surgeries { get; set; } = string.Empty;
        public string Allergies { get; set; } = string.Empty;
        public string FamilyHistoryJson { get; set; } = "[]"; 

        public string PreferredLanguage { get; set; } = string.Empty;
        public bool InterpreterNeeded { get; set; }
        public string PrimaryCarePhysician { get; set; } = string.Empty;
        public string PhysicianPhone { get; set; } = string.Empty;
        public string Vaccinations { get; set; } = string.Empty;
        public string CurrentPain { get; set; } = string.Empty;
        public string GynecologicalHistory { get; set; } = string.Empty;
        public string MentalHealthIssues { get; set; } = string.Empty;

        public string CreatedBy { get; set; } = string.Empty; 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public List<ExpenseReport> ExpenseReports { get; set; } = new();

        public List<string> MedicalConditions
        {
            get => System.Text.Json.JsonSerializer.Deserialize<List<string>>(MedicalConditionsJson) ?? new();
            set => MedicalConditionsJson = System.Text.Json.JsonSerializer.Serialize(value);
        }

        public List<string> FamilyHistory
        {
            get => System.Text.Json.JsonSerializer.Deserialize<List<string>>(FamilyHistoryJson) ?? new();
            set => FamilyHistoryJson = System.Text.Json.JsonSerializer.Serialize(value);
        }

        public override string ToString() => FullName;
    }
}