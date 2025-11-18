using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Florence.Models
{
    public enum NursePosition
    {
        RN,  // Registered Nurse
        MW,  // Midwife
        PN,  // Practical Nurse
        GD,  // Guard
        ON,  // Operation Nurse
        DN   // Director Nurse
    }

    public enum VacancySource
    {
        Publicity,
        Recommendation,
        Other
    }

    public class Nurse
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string University { get; set; } = string.Empty;

        [Range(1900, 2100)]
        public int BirthYear { get; set; }

        public string MaritalStatus { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = "-";

        public NursePosition Position { get; set; }
        public string CoursePlus { get; set; } = string.Empty;
        public string LastWorkplace { get; set; } = string.Empty;
        public string LastSalary { get; set; } = string.Empty;
        public string Languages { get; set; } = string.Empty;
        public string HospitalExperience { get; set; } = string.Empty;

        public VacancySource VacancySource { get; set; }
        public string VacancyOther { get; set; } = string.Empty;

        public string Preferences { get; set; } = string.Empty;
        public string Impression { get; set; } = string.Empty;
        public string Look { get; set; } = string.Empty;
        public string Character { get; set; } = string.Empty;

        public bool ExperienceElderly { get; set; }
        public bool ExperienceNewborns { get; set; }
        public bool ExperienceSnc { get; set; }

        public bool SkillChemo { get; set; }
        public bool SkillTracheotomy { get; set; }
        public bool SkillBasicCare { get; set; }
        public bool SkillPhysiotherapy { get; set; }
        public bool SkillColostomyBag { get; set; }
        public bool SkillIm { get; set; }
        public bool SkillIv { get; set; }
        public bool SkillFolly { get; set; }
        public bool SkillSuction { get; set; }
        public bool SkillFeedingSng { get; set; }
        public bool SkillIntraDermique { get; set; }
        public bool SkillDrugs { get; set; }
        public bool SkillPulseOxim { get; set; }
        public bool SkillBedRest { get; set; }
        public bool SkillIntubation { get; set; }
        public bool SkillGastroTube { get; set; }
        public bool SkillBedToilet { get; set; }
        public bool SkillVaccination { get; set; }
        public bool SkillDressing { get; set; }
        public bool SkillBadSore { get; set; }
        public bool SkillHemoGlucoTest { get; set; }
        public bool SkillSmoke { get; set; }
        public bool SkillPulsePressure { get; set; }
        public bool SkillTransportation { get; set; }
        public bool SkillFirstAideCourse { get; set; }

        public bool AvailabilityDailyShift { get; set; }
        public bool AvailabilityNightShift { get; set; }
        public bool Availability24Hours { get; set; }
        public bool AvailabilityWorkInHospital { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string Interviewer { get; set; } = string.Empty;
        public DateOnly? InterviewDate { get; set; }

        [JsonIgnore]
        public List<ExpenseItem> ExpenseItems { get; set; } = new();

        public override string ToString() => $"{FullName} ({Position})";
    }
}