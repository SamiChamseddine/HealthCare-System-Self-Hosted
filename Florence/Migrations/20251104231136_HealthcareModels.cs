using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Florence.Migrations
{
    /// <inheritdoc />
    public partial class HealthcareModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Nurses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    University = table.Column<string>(type: "text", nullable: false),
                    BirthYear = table.Column<int>(type: "integer", nullable: false),
                    MaritalStatus = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false),
                    CoursePlus = table.Column<string>(type: "text", nullable: false),
                    LastWorkplace = table.Column<string>(type: "text", nullable: false),
                    LastSalary = table.Column<string>(type: "text", nullable: false),
                    Languages = table.Column<string>(type: "text", nullable: false),
                    HospitalExperience = table.Column<string>(type: "text", nullable: false),
                    VacancySource = table.Column<int>(type: "integer", nullable: false),
                    VacancyOther = table.Column<string>(type: "text", nullable: false),
                    Preferences = table.Column<string>(type: "text", nullable: false),
                    Impression = table.Column<string>(type: "text", nullable: false),
                    Look = table.Column<string>(type: "text", nullable: false),
                    Character = table.Column<string>(type: "text", nullable: false),
                    ExperienceElderly = table.Column<bool>(type: "boolean", nullable: false),
                    ExperienceNewborns = table.Column<bool>(type: "boolean", nullable: false),
                    ExperienceSnc = table.Column<bool>(type: "boolean", nullable: false),
                    SkillChemo = table.Column<bool>(type: "boolean", nullable: false),
                    SkillTracheotomy = table.Column<bool>(type: "boolean", nullable: false),
                    SkillBasicCare = table.Column<bool>(type: "boolean", nullable: false),
                    SkillPhysiotherapy = table.Column<bool>(type: "boolean", nullable: false),
                    SkillColostomyBag = table.Column<bool>(type: "boolean", nullable: false),
                    SkillIm = table.Column<bool>(type: "boolean", nullable: false),
                    SkillIv = table.Column<bool>(type: "boolean", nullable: false),
                    SkillFolly = table.Column<bool>(type: "boolean", nullable: false),
                    SkillSuction = table.Column<bool>(type: "boolean", nullable: false),
                    SkillFeedingSng = table.Column<bool>(type: "boolean", nullable: false),
                    SkillIntraDermique = table.Column<bool>(type: "boolean", nullable: false),
                    SkillDrugs = table.Column<bool>(type: "boolean", nullable: false),
                    SkillPulseOxim = table.Column<bool>(type: "boolean", nullable: false),
                    SkillBedRest = table.Column<bool>(type: "boolean", nullable: false),
                    SkillIntubation = table.Column<bool>(type: "boolean", nullable: false),
                    SkillGastroTube = table.Column<bool>(type: "boolean", nullable: false),
                    SkillBedToilet = table.Column<bool>(type: "boolean", nullable: false),
                    SkillVaccination = table.Column<bool>(type: "boolean", nullable: false),
                    SkillDressing = table.Column<bool>(type: "boolean", nullable: false),
                    SkillBadSore = table.Column<bool>(type: "boolean", nullable: false),
                    SkillHemoGlucoTest = table.Column<bool>(type: "boolean", nullable: false),
                    SkillSmoke = table.Column<bool>(type: "boolean", nullable: false),
                    SkillPulsePressure = table.Column<bool>(type: "boolean", nullable: false),
                    SkillTransportation = table.Column<bool>(type: "boolean", nullable: false),
                    SkillFirstAideCourse = table.Column<bool>(type: "boolean", nullable: false),
                    AvailabilityDailyShift = table.Column<bool>(type: "boolean", nullable: false),
                    AvailabilityNightShift = table.Column<bool>(type: "boolean", nullable: false),
                    Availability24Hours = table.Column<bool>(type: "boolean", nullable: false),
                    AvailabilityWorkInHospital = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Interviewer = table.Column<string>(type: "text", nullable: false),
                    InterviewDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nurses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    EmergencyContact = table.Column<string>(type: "text", nullable: false),
                    EmergencyRelationship = table.Column<string>(type: "text", nullable: false),
                    EmergencyPhone = table.Column<string>(type: "text", nullable: false),
                    InsuranceProvider = table.Column<string>(type: "text", nullable: false),
                    PolicyNumber = table.Column<string>(type: "text", nullable: false),
                    MedicalConditionsJson = table.Column<string>(type: "text", nullable: false),
                    CurrentMedications = table.Column<string>(type: "text", nullable: false),
                    Surgeries = table.Column<string>(type: "text", nullable: false),
                    Allergies = table.Column<string>(type: "text", nullable: false),
                    FamilyHistoryJson = table.Column<string>(type: "text", nullable: false),
                    PreferredLanguage = table.Column<string>(type: "text", nullable: false),
                    InterpreterNeeded = table.Column<bool>(type: "boolean", nullable: false),
                    PrimaryCarePhysician = table.Column<string>(type: "text", nullable: false),
                    PhysicianPhone = table.Column<string>(type: "text", nullable: false),
                    Vaccinations = table.Column<string>(type: "text", nullable: false),
                    CurrentPain = table.Column<string>(type: "text", nullable: false),
                    GynecologicalHistory = table.Column<string>(type: "text", nullable: false),
                    MentalHealthIssues = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MedicalConditions = table.Column<List<string>>(type: "text[]", nullable: false),
                    FamilyHistory = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalHours = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpenseReports_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpenseReportId = table.Column<int>(type: "integer", nullable: false),
                    NurseId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Hours = table.Column<decimal>(type: "numeric", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpenseItems_ExpenseReports_ExpenseReportId",
                        column: x => x.ExpenseReportId,
                        principalTable: "ExpenseReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExpenseItems_Nurses_NurseId",
                        column: x => x.NurseId,
                        principalTable: "Nurses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseItems_ExpenseReportId",
                table: "ExpenseItems",
                column: "ExpenseReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseItems_NurseId",
                table: "ExpenseItems",
                column: "NurseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseReports_PatientId",
                table: "ExpenseReports",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpenseItems");

            migrationBuilder.DropTable(
                name: "ExpenseReports");

            migrationBuilder.DropTable(
                name: "Nurses");

            migrationBuilder.DropTable(
                name: "Patients");
        }
    }
}
