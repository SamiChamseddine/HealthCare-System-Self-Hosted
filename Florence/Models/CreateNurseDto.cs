namespace Florence.Models
{
    public class CreateNurseDto
    {
        public string FullName { get; set; } = string.Empty;
        public NursePosition Position { get; set; }
        public string Address { get; set; } = string.Empty;
        public string University { get; set; } = string.Empty;
        public int BirthYear { get; set; }
        public string MaritalStatus { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = "-";
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
        public string Interviewer { get; set; } = string.Empty;
        public DateOnly? InterviewDate { get; set; }
    }

}
