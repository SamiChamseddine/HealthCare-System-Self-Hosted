using Florence.Data;
using Florence.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Florence.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PatientsController(ApplicationDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<List<PatientDto>>> GetPatients()
        {
            var patients = await _context.Patients.ToListAsync();
            return Ok(patients.Select(MapToDto).ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDtoFull>> GetPatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return NotFound();

            return Ok(new PatientDtoFull
            {
                Id = patient.Id,
                FullName = patient.FullName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                Address = patient.Address,
                PhoneNumber = patient.PhoneNumber,
                EmergencyContact = patient.EmergencyContact,
                EmergencyRelationship = patient.EmergencyRelationship,
                EmergencyPhone = patient.EmergencyPhone,
                InsuranceProvider = patient.InsuranceProvider,
                PolicyNumber = patient.PolicyNumber,
                MedicalConditionsJson = patient.MedicalConditionsJson,
                CurrentMedications = patient.CurrentMedications,
                Surgeries = patient.Surgeries,
                Allergies = patient.Allergies,
                FamilyHistoryJson = patient.FamilyHistoryJson,
                PreferredLanguage = patient.PreferredLanguage,
                InterpreterNeeded = patient.InterpreterNeeded,
                PrimaryCarePhysician = patient.PrimaryCarePhysician,
                PhysicianPhone = patient.PhysicianPhone,
                Vaccinations = patient.Vaccinations,
                CurrentPain = patient.CurrentPain,
                GynecologicalHistory = patient.GynecologicalHistory,
                MentalHealthIssues = patient.MentalHealthIssues,
                CreatedBy = patient.CreatedBy,
                CreatedAt = patient.CreatedAt,
                UpdatedAt = patient.UpdatedAt
            });
        }



        [HttpPost]
        public async Task<ActionResult<PatientDto>> CreatePatient(CreatePatientDto createDto)
        {
            var patient = new Patient
            {
                FullName = createDto.FullName,
                DateOfBirth = createDto.DateOfBirth,
                Gender = createDto.Gender,
                Address = createDto.Address,
                PhoneNumber = createDto.PhoneNumber,
                EmergencyContact = createDto.EmergencyContact,
                EmergencyRelationship = createDto.EmergencyRelationship,
                EmergencyPhone = createDto.EmergencyPhone,
                InsuranceProvider = createDto.InsuranceProvider,
                PolicyNumber = createDto.PolicyNumber,
                MedicalConditionsJson = createDto.MedicalConditionsJson,
                CurrentMedications = createDto.CurrentMedications,
                Surgeries = createDto.Surgeries,
                Allergies = createDto.Allergies,
                FamilyHistoryJson = createDto.FamilyHistoryJson,
                PreferredLanguage = createDto.PreferredLanguage,
                InterpreterNeeded = createDto.InterpreterNeeded,
                PrimaryCarePhysician = createDto.PrimaryCarePhysician,
                PhysicianPhone = createDto.PhysicianPhone,
                Vaccinations = createDto.Vaccinations,
                CurrentPain = createDto.CurrentPain,
                GynecologicalHistory = createDto.GynecologicalHistory,
                MentalHealthIssues = createDto.MentalHealthIssues,
                CreatedBy = createDto.CreatedBy
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return Ok(patient);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PatientDto>> UpdatePatient(int id, CreatePatientDto updateDto)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return NotFound();

            patient.FullName = updateDto.FullName;
            patient.DateOfBirth = updateDto.DateOfBirth;
            patient.Gender = updateDto.Gender;
            patient.Address = updateDto.Address;
            patient.PhoneNumber = updateDto.PhoneNumber;
            patient.EmergencyContact = updateDto.EmergencyContact;
            patient.EmergencyRelationship = updateDto.EmergencyRelationship;
            patient.EmergencyPhone = updateDto.EmergencyPhone;
            patient.InsuranceProvider = updateDto.InsuranceProvider;
            patient.PolicyNumber = updateDto.PolicyNumber;
            patient.MedicalConditionsJson = updateDto.MedicalConditionsJson;
            patient.CurrentMedications = updateDto.CurrentMedications;
            patient.Surgeries = updateDto.Surgeries;
            patient.Allergies = updateDto.Allergies;
            patient.FamilyHistoryJson = updateDto.FamilyHistoryJson;
            patient.PreferredLanguage = updateDto.PreferredLanguage;
            patient.InterpreterNeeded = updateDto.InterpreterNeeded;
            patient.PrimaryCarePhysician = updateDto.PrimaryCarePhysician;
            patient.PhysicianPhone = updateDto.PhysicianPhone;
            patient.Vaccinations = updateDto.Vaccinations;
            patient.CurrentPain = updateDto.CurrentPain;
            patient.GynecologicalHistory = updateDto.GynecologicalHistory;
            patient.MentalHealthIssues = updateDto.MentalHealthIssues;

            patient.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return Ok(patient);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return NotFound();

            var hasReports = await _context.ExpenseReports.AnyAsync(er => er.PatientId == id);
            if (hasReports)
                return BadRequest("Cannot delete patient with existing expense reports");

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}/expense-reports")]
        public async Task<ActionResult<List<ExpenseReportDto>>> GetPatientExpenseReports(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return NotFound();

            var reports = await _context.ExpenseReports
                .Include(er => er.Patient)
                .Include(er => er.Items)
                    .ThenInclude(i => i.Nurse)
                .Where(er => er.PatientId == id)
                .ToListAsync();

            return Ok(reports.Select(ExpenseReportsController.MapToDto).ToList());
        }

        private PatientDto MapToDto(Patient patient)
        {
            return new PatientDto
            {
                Id = patient.Id,
                FullName = patient.FullName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                PhoneNumber = patient.PhoneNumber,
                ExpenseReportCount = patient.ExpenseReports.Count,
                CreatedAt = patient.CreatedAt
            };
        }
    }
}