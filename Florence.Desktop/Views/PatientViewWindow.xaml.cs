using Florence.Desktop.Documents;
using Florence.Desktop.Models;
using Florence.Desktop.Services;
using MahApps.Metro.Controls;
using QuestPDF.Fluent;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace Florence.Desktop.Views
{
    public partial class PatientViewWindow : MetroWindow
    {
        private readonly int _patientId;
        private PatientDto? _patient;   

        public PatientViewWindow(PatientDto summary)
        {
            InitializeComponent();
            _patientId = summary.Id;
            LoadFullPatient(_patientId);
        }

        private async void LoadFullPatient(int id)
        {
            try
            {
                var api = new ApiService();
                var full = await api.GetPatientByIdAsync(id);

                if (full == null)
                {
                    MessageBox.Show("Failed to load patient details.");
                    Close();
                    return;
                }

                _patient = new PatientDto
                {
                    Id = full.Id,
                    FullName = full.FullName,
                    DateOfBirth = full.DateOfBirth,
                    Gender = full.Gender,
                    Address = full.Address,
                    PhoneNumber = full.PhoneNumber,

                    EmergencyContact = full.EmergencyContact,
                    EmergencyRelationship = full.EmergencyRelationship,
                    EmergencyPhone = full.EmergencyPhone,

                    InsuranceProvider = full.InsuranceProvider,
                    PolicyNumber = full.PolicyNumber,

                    MedicalConditionsJson = full.MedicalConditionsJson,
                    CurrentMedications = full.CurrentMedications,
                    Surgeries = full.Surgeries,
                    Allergies = full.Allergies,
                    FamilyHistoryJson = full.FamilyHistoryJson,

                    PreferredLanguage = full.PreferredLanguage,
                    InterpreterNeeded = full.InterpreterNeeded,
                    PrimaryCarePhysician = full.PrimaryCarePhysician,
                    PhysicianPhone = full.PhysicianPhone,
                    Vaccinations = full.Vaccinations,
                    CurrentPain = full.CurrentPain,
                    GynecologicalHistory = full.GynecologicalHistory,
                    MentalHealthIssues = full.MentalHealthIssues,

                    CreatedBy = full.CreatedBy,
                    CreatedAt = full.CreatedAt,
                    UpdatedAt = full.UpdatedAt
                };

                DataContext = _patient;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading patient: {ex.Message}");
                Close();
            }
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_patient == null)
                {
                    MessageBox.Show("Patient data not loaded yet.");
                    return;
                }

                var desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                var pdfPath = Path.Combine(desktop, $"Patient_{_patient.Id}.pdf");

                var document = new PatientDocument(_patient);
                document.GeneratePdf(pdfPath);

                var p = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = pdfPath,
                        UseShellExecute = true
                    }
                };
                p.Start();

                MessageBox.Show($"Patient summary saved and opened!\n{pdfPath}",
                                "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to print patient: {ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
