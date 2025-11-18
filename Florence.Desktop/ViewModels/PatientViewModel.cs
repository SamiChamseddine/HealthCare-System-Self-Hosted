using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using Florence.Desktop.Models;
using Florence.Desktop.Services;
using Florence.Desktop.Utils;

namespace Florence.Desktop.ViewModels
{
    public class PatientViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _api;
        private CreatePatientDto _patient = new();
        private string _error = "";
        private readonly bool _isEditMode;
        private readonly int _existingId;

        public ObservableCollection<string> MedicalConditions { get; } = new();
        public ObservableCollection<string> FamilyHistory { get; } = new();

        public PatientViewModel(ApiService api)
        {
            _api = api;
            _isEditMode = false;
            InitCommands();
            Clear();
        }

        public PatientViewModel(ApiService api, PatientDtoFull existingPatient)
        {
            _api = api;
            _isEditMode = true;
            _existingId = existingPatient.Id;
            InitCommands();

            Patient = new CreatePatientDto
            {
                FullName = existingPatient.FullName,
                DateOfBirth = existingPatient.DateOfBirth,
                Gender = existingPatient.Gender,
                Address = existingPatient.Address,
                PhoneNumber = existingPatient.PhoneNumber,

                EmergencyContact = existingPatient.EmergencyContact,
                EmergencyRelationship = existingPatient.EmergencyRelationship,
                EmergencyPhone = existingPatient.EmergencyPhone,

                InsuranceProvider = existingPatient.InsuranceProvider,
                PolicyNumber = existingPatient.PolicyNumber,

                MedicalConditionsJson = existingPatient.MedicalConditionsJson,
                CurrentMedications = existingPatient.CurrentMedications,
                Surgeries = existingPatient.Surgeries,
                Allergies = existingPatient.Allergies,
                FamilyHistoryJson = existingPatient.FamilyHistoryJson,

                PreferredLanguage = existingPatient.PreferredLanguage,
                InterpreterNeeded = existingPatient.InterpreterNeeded,

                PrimaryCarePhysician = existingPatient.PrimaryCarePhysician,
                PhysicianPhone = existingPatient.PhysicianPhone,

                Vaccinations = existingPatient.Vaccinations,
                CurrentPain = existingPatient.CurrentPain,
                GynecologicalHistory = existingPatient.GynecologicalHistory,
                MentalHealthIssues = existingPatient.MentalHealthIssues,

                CreatedBy = existingPatient.CreatedBy
            };
            OnPropertyChanged(nameof(HeaderText));

            MedicalConditions.Clear();
            FamilyHistory.Clear();

            try
            {
                var cond = JsonSerializer.Deserialize<List<string>>(existingPatient.MedicalConditionsJson);
                if (cond != null)
                    foreach (var c in cond)
                        MedicalConditions.Add(c);

                var fam = JsonSerializer.Deserialize<List<string>>(existingPatient.FamilyHistoryJson);
                if (fam != null)
                    foreach (var f in fam)
                        FamilyHistory.Add(f);
            }
            catch
            {
            }
        }

        #region Command Setup
        private void InitCommands()
        {
            SaveCommand = new RelayCommand(async () => await SaveAsync(), CanSave);
            ClearCommand = new RelayCommand(Clear);
            AddMedicalConditionCommand = new RelayCommand(AddMedicalCondition, () => !string.IsNullOrWhiteSpace(NewMedicalCondition));
            RemoveMedicalConditionCommand = new RelayCommand<string>(RemoveMedicalCondition);
            AddFamilyHistoryCommand = new RelayCommand(AddFamilyHistoryEntry, () => !string.IsNullOrWhiteSpace(NewFamilyHistoryEntry));
            RemoveFamilyHistoryCommand = new RelayCommand<string>(RemoveFamilyHistoryEntry);
        }
        #endregion

        #region Properties
        public CreatePatientDto Patient
        {
            get => _patient;
            set { _patient = value; OnPropertyChanged(); }
        }

        public string NewMedicalCondition { get; set; } = "";
        public string NewFamilyHistoryEntry { get; set; } = "";
        public Array Genders => Enum.GetValues(typeof(Gender));
        #endregion

        #region Commands
        public ICommand SaveCommand { get; private set; } = null!;
        public ICommand ClearCommand { get; private set; } = null!;
        public ICommand AddMedicalConditionCommand { get; private set; } = null!;
        public ICommand RemoveMedicalConditionCommand { get; private set; } = null!;
        public ICommand AddFamilyHistoryCommand { get; private set; } = null!;
        public ICommand RemoveFamilyHistoryCommand { get; private set; } = null!;
        #endregion

        #region Errors
        public string Error
        {
            get => _error;
            private set { _error = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasErrors)); }
        }

        public bool HasErrors => !string.IsNullOrWhiteSpace(Error);
        #endregion

        #region Logic
        private bool CanSave() =>
            !string.IsNullOrWhiteSpace(Patient.FullName) &&
            !string.IsNullOrWhiteSpace(Patient.PhoneNumber);

        private async System.Threading.Tasks.Task SaveAsync()
        {
            try
            {
                Error = "";

                // serialize list data
                Patient.MedicalConditionsJson = JsonSerializer.Serialize(MedicalConditions);
                Patient.FamilyHistoryJson = JsonSerializer.Serialize(FamilyHistory);

                if (_isEditMode)
                {
                    await _api.UpdatePatientAsync(_existingId, Patient);
                    MessageBox.Show("✅ Patient updated successfully!", "Updated",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    await _api.CreatePatientAsync(Patient);
                    MessageBox.Show("✅ Patient created successfully!", "Created",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    Clear();
                }
            }
            catch (Exception ex)
            {
                Error = $"Failed to save patient: {ex.Message}";
            }
        }

        private void Clear()
        {
            if (_isEditMode) return;

            Patient = new CreatePatientDto
            {
                FullName = "",
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-30)),
                Gender = Gender.Other,
                PhoneNumber = "",
                MedicalConditionsJson = "[]",
                FamilyHistoryJson = "[]",
                CreatedBy = Environment.UserName
            };

            MedicalConditions.Clear();
            FamilyHistory.Clear();
            NewMedicalCondition = "";
            NewFamilyHistoryEntry = "";
            Error = "";
        }

        private void AddMedicalCondition()
        {
            if (!string.IsNullOrWhiteSpace(NewMedicalCondition))
            {
                MedicalConditions.Add(NewMedicalCondition.Trim());
                NewMedicalCondition = "";
                OnPropertyChanged(nameof(NewMedicalCondition));
            }
        }

        private void RemoveMedicalCondition(string condition)
        {
            if (!string.IsNullOrWhiteSpace(condition))
                MedicalConditions.Remove(condition);
        }

        private void AddFamilyHistoryEntry()
        {
            if (!string.IsNullOrWhiteSpace(NewFamilyHistoryEntry))
            {
                FamilyHistory.Add(NewFamilyHistoryEntry.Trim());
                NewFamilyHistoryEntry = "";
                OnPropertyChanged(nameof(NewFamilyHistoryEntry));
            }
        }

        private void RemoveFamilyHistoryEntry(string entry)
        {
            if (!string.IsNullOrWhiteSpace(entry))
                FamilyHistory.Remove(entry);
        }
        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        public string HeaderText => _isEditMode ? "Edit Patient" : "Create Patient";

    }
}
