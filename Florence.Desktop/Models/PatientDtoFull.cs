namespace Florence.Desktop.Models
{
    public class PatientDtoFull
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
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
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
