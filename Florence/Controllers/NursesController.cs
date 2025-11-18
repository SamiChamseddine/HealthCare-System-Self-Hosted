using Florence.Data;
using Florence.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Florence.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NursesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NursesController(ApplicationDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<List<NurseDto>>> GetNurses()
        {
            var nurses = await _context.Nurses.ToListAsync();
            return Ok(nurses.Select(MapToDto).ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NurseDtoFull>> GetNurse(int id)
        {
            var nurse = await _context.Nurses.FindAsync(id);
            if (nurse == null) return NotFound();

            return Ok(new NurseDtoFull
            {
                Id = nurse.Id,
                FullName = nurse.FullName,
                Position = nurse.Position,
                Address = nurse.Address,
                University = nurse.University,
                BirthYear = nurse.BirthYear,
                MaritalStatus = nurse.MaritalStatus,
                PhoneNumber = nurse.PhoneNumber,
                CoursePlus = nurse.CoursePlus,
                LastWorkplace = nurse.LastWorkplace,
                LastSalary = nurse.LastSalary,
                Languages = nurse.Languages,
                HospitalExperience = nurse.HospitalExperience,
                VacancySource = nurse.VacancySource,
                VacancyOther = nurse.VacancyOther,
                Preferences = nurse.Preferences,
                Impression = nurse.Impression,
                Look = nurse.Look,
                Character = nurse.Character,
                ExperienceElderly = nurse.ExperienceElderly,
                ExperienceNewborns = nurse.ExperienceNewborns,
                ExperienceSnc = nurse.ExperienceSnc,
                SkillChemo = nurse.SkillChemo,
                SkillTracheotomy = nurse.SkillTracheotomy,
                SkillBasicCare = nurse.SkillBasicCare,
                SkillPhysiotherapy = nurse.SkillPhysiotherapy,
                SkillColostomyBag = nurse.SkillColostomyBag,
                SkillIm = nurse.SkillIm,
                SkillIv = nurse.SkillIv,
                SkillFolly = nurse.SkillFolly,
                SkillSuction = nurse.SkillSuction,
                SkillFeedingSng = nurse.SkillFeedingSng,
                SkillIntraDermique = nurse.SkillIntraDermique,
                SkillDrugs = nurse.SkillDrugs,
                SkillPulseOxim = nurse.SkillPulseOxim,
                SkillBedRest = nurse.SkillBedRest,
                SkillIntubation = nurse.SkillIntubation,
                SkillGastroTube = nurse.SkillGastroTube,
                SkillBedToilet = nurse.SkillBedToilet,
                SkillVaccination = nurse.SkillVaccination,
                SkillDressing = nurse.SkillDressing,
                SkillBadSore = nurse.SkillBadSore,
                SkillHemoGlucoTest = nurse.SkillHemoGlucoTest,
                SkillSmoke = nurse.SkillSmoke,
                SkillPulsePressure = nurse.SkillPulsePressure,
                SkillTransportation = nurse.SkillTransportation,
                SkillFirstAideCourse = nurse.SkillFirstAideCourse,
                AvailabilityDailyShift = nurse.AvailabilityDailyShift,
                AvailabilityNightShift = nurse.AvailabilityNightShift,
                Availability24Hours = nurse.Availability24Hours,
                AvailabilityWorkInHospital = nurse.AvailabilityWorkInHospital,
                Interviewer = nurse.Interviewer,
                InterviewDate = nurse.InterviewDate,
                CreatedAt = nurse.CreatedAt,
                UpdatedAt = nurse.UpdatedAt
            });
        }


        [HttpPost]
        public async Task<ActionResult<NurseDto>> CreateNurse(CreateNurseDto createDto)
        {
            var nurse = new Nurse
            {
                FullName = createDto.FullName,
                Position = createDto.Position,
                Address = createDto.Address,
                University = createDto.University,
                BirthYear = createDto.BirthYear,
                MaritalStatus = createDto.MaritalStatus,
                PhoneNumber = createDto.PhoneNumber,
                CoursePlus = createDto.CoursePlus,
                LastWorkplace = createDto.LastWorkplace,
                LastSalary = createDto.LastSalary,
                Languages = createDto.Languages,
                HospitalExperience = createDto.HospitalExperience,
                VacancySource = createDto.VacancySource,
                VacancyOther = createDto.VacancyOther,
                Preferences = createDto.Preferences,
                Impression = createDto.Impression,
                Look = createDto.Look,
                Character = createDto.Character,
                ExperienceElderly = createDto.ExperienceElderly,
                ExperienceNewborns = createDto.ExperienceNewborns,
                ExperienceSnc = createDto.ExperienceSnc,
                SkillChemo = createDto.SkillChemo,
                SkillTracheotomy = createDto.SkillTracheotomy,
                SkillBasicCare = createDto.SkillBasicCare,
                SkillPhysiotherapy = createDto.SkillPhysiotherapy,
                SkillColostomyBag = createDto.SkillColostomyBag,
                SkillIm = createDto.SkillIm,
                SkillIv = createDto.SkillIv,
                SkillFolly = createDto.SkillFolly,
                SkillSuction = createDto.SkillSuction,
                SkillFeedingSng = createDto.SkillFeedingSng,
                SkillIntraDermique = createDto.SkillIntraDermique,
                SkillDrugs = createDto.SkillDrugs,
                SkillPulseOxim = createDto.SkillPulseOxim,
                SkillBedRest = createDto.SkillBedRest,
                SkillIntubation = createDto.SkillIntubation,
                SkillGastroTube = createDto.SkillGastroTube,
                SkillBedToilet = createDto.SkillBedToilet,
                SkillVaccination = createDto.SkillVaccination,
                SkillDressing = createDto.SkillDressing,
                SkillBadSore = createDto.SkillBadSore,
                SkillHemoGlucoTest = createDto.SkillHemoGlucoTest,
                SkillSmoke = createDto.SkillSmoke,
                SkillPulsePressure = createDto.SkillPulsePressure,
                SkillTransportation = createDto.SkillTransportation,
                SkillFirstAideCourse = createDto.SkillFirstAideCourse,
                AvailabilityDailyShift = createDto.AvailabilityDailyShift,
                AvailabilityNightShift = createDto.AvailabilityNightShift,
                Availability24Hours = createDto.Availability24Hours,
                AvailabilityWorkInHospital = createDto.AvailabilityWorkInHospital,
                Interviewer = createDto.Interviewer,
                InterviewDate = createDto.InterviewDate
            };

            _context.Nurses.Add(nurse);
            await _context.SaveChangesAsync();
            return Ok(nurse);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<NurseDto>> UpdateNurse(int id, CreateNurseDto updateDto)
        {
            var nurse = await _context.Nurses.FindAsync(id);
            if (nurse == null) return NotFound();

            nurse.FullName = updateDto.FullName;
            nurse.Position = updateDto.Position;
            nurse.Address = updateDto.Address;
            nurse.University = updateDto.University;
            nurse.BirthYear = updateDto.BirthYear;
            nurse.MaritalStatus = updateDto.MaritalStatus;
            nurse.PhoneNumber = updateDto.PhoneNumber;
            nurse.CoursePlus = updateDto.CoursePlus;
            nurse.LastWorkplace = updateDto.LastWorkplace;
            nurse.LastSalary = updateDto.LastSalary;
            nurse.Languages = updateDto.Languages;
            nurse.HospitalExperience = updateDto.HospitalExperience;
            nurse.VacancySource = updateDto.VacancySource;
            nurse.VacancyOther = updateDto.VacancyOther;
            nurse.Preferences = updateDto.Preferences;
            nurse.Impression = updateDto.Impression;
            nurse.Look = updateDto.Look;
            nurse.Character = updateDto.Character;
            nurse.ExperienceElderly = updateDto.ExperienceElderly;
            nurse.ExperienceNewborns = updateDto.ExperienceNewborns;
            nurse.ExperienceSnc = updateDto.ExperienceSnc;
            nurse.SkillChemo = updateDto.SkillChemo;
            nurse.SkillTracheotomy = updateDto.SkillTracheotomy;
            nurse.SkillBasicCare = updateDto.SkillBasicCare;
            nurse.SkillPhysiotherapy = updateDto.SkillPhysiotherapy;
            nurse.SkillColostomyBag = updateDto.SkillColostomyBag;
            nurse.SkillIm = updateDto.SkillIm;
            nurse.SkillIv = updateDto.SkillIv;
            nurse.SkillFolly = updateDto.SkillFolly;
            nurse.SkillSuction = updateDto.SkillSuction;
            nurse.SkillFeedingSng = updateDto.SkillFeedingSng;
            nurse.SkillIntraDermique = updateDto.SkillIntraDermique;
            nurse.SkillDrugs = updateDto.SkillDrugs;
            nurse.SkillPulseOxim = updateDto.SkillPulseOxim;
            nurse.SkillBedRest = updateDto.SkillBedRest;
            nurse.SkillIntubation = updateDto.SkillIntubation;
            nurse.SkillGastroTube = updateDto.SkillGastroTube;
            nurse.SkillBedToilet = updateDto.SkillBedToilet;
            nurse.SkillVaccination = updateDto.SkillVaccination;
            nurse.SkillDressing = updateDto.SkillDressing;
            nurse.SkillBadSore = updateDto.SkillBadSore;
            nurse.SkillHemoGlucoTest = updateDto.SkillHemoGlucoTest;
            nurse.SkillSmoke = updateDto.SkillSmoke;
            nurse.SkillPulsePressure = updateDto.SkillPulsePressure;
            nurse.SkillTransportation = updateDto.SkillTransportation;
            nurse.SkillFirstAideCourse = updateDto.SkillFirstAideCourse;
            nurse.AvailabilityDailyShift = updateDto.AvailabilityDailyShift;
            nurse.AvailabilityNightShift = updateDto.AvailabilityNightShift;
            nurse.Availability24Hours = updateDto.Availability24Hours;
            nurse.AvailabilityWorkInHospital = updateDto.AvailabilityWorkInHospital;
            nurse.Interviewer = updateDto.Interviewer;
            nurse.InterviewDate = updateDto.InterviewDate;

            nurse.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return Ok(nurse);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNurse(int id)
        {
            var nurse = await _context.Nurses.FindAsync(id);
            if (nurse == null) return NotFound();

            var hasExpenseItems = await _context.ExpenseItems.AnyAsync(ei => ei.NurseId == id);
            if (hasExpenseItems)
                return BadRequest("Cannot delete nurse with existing expense items");

            _context.Nurses.Remove(nurse);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}/expense-items")]
        public async Task<ActionResult<List<ExpenseItemDto>>> GetNurseExpenseItems(int id)
        {
            var nurse = await _context.Nurses.FindAsync(id);
            if (nurse == null) return NotFound();

            var items = await _context.ExpenseItems
                .Include(ei => ei.Report)
                .Include(ei => ei.Nurse)
                .Where(ei => ei.NurseId == id)
                .ToListAsync();

            return Ok(items.Select(ExpenseItemsController.MapToDto).ToList());
        }

        private NurseDto MapToDto(Nurse nurse)
        {
            return new NurseDto
            {
                Id = nurse.Id,
                FullName = nurse.FullName,
                Position = nurse.Position,
                PhoneNumber = nurse.PhoneNumber,
                ExpenseItemCount = nurse.ExpenseItems.Count,
                CreatedAt = nurse.CreatedAt
            };
        }
    }
}