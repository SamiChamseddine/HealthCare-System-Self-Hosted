using Florence.Desktop.Models;
using Florence.Desktop.Services;
using Florence.Desktop.Utils;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;

namespace Florence.Desktop.ViewModels
{
    public class NurseViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _api;
        private CreateNurseDto _nurse = new();
        private string _error = "";
        private readonly bool _isEditMode;
        private readonly int? _existingId;

        public NurseViewModel(ApiService api)
        {
            _api = api;
            _isEditMode = false;
            InitCommands();
            Clear();
        }

        public NurseViewModel(ApiService api, NurseDtoFull existing)
        {
            _api = api;
            _isEditMode = true;
            _existingId = existing.Id;
            InitCommands();

            Nurse = new CreateNurseDto
            {
                FullName = existing.FullName,
                Position = existing.Position,
                Address = existing.Address,
                University = existing.University,
                BirthYear = existing.BirthYear,
                MaritalStatus = existing.MaritalStatus,
                PhoneNumber = existing.PhoneNumber,

                CoursePlus = existing.CoursePlus,
                LastWorkplace = existing.LastWorkplace,
                LastSalary = existing.LastSalary,
                Languages = existing.Languages,
                HospitalExperience = existing.HospitalExperience,

                VacancySource = existing.VacancySource,
                VacancyOther = existing.VacancyOther,

                Preferences = existing.Preferences,
                Impression = existing.Impression,
                Look = existing.Look,
                Character = existing.Character,

                ExperienceElderly = existing.ExperienceElderly,
                ExperienceNewborns = existing.ExperienceNewborns,
                ExperienceSnc = existing.ExperienceSnc,

                SkillChemo = existing.SkillChemo,
                SkillTracheotomy = existing.SkillTracheotomy,
                SkillBasicCare = existing.SkillBasicCare,
                SkillPhysiotherapy = existing.SkillPhysiotherapy,
                SkillColostomyBag = existing.SkillColostomyBag,
                SkillIm = existing.SkillIm,
                SkillIv = existing.SkillIv,
                SkillFolly = existing.SkillFolly,
                SkillSuction = existing.SkillSuction,
                SkillFeedingSng = existing.SkillFeedingSng,
                SkillIntraDermique = existing.SkillIntraDermique,
                SkillDrugs = existing.SkillDrugs,
                SkillPulseOxim = existing.SkillPulseOxim,
                SkillBedRest = existing.SkillBedRest,
                SkillIntubation = existing.SkillIntubation,
                SkillGastroTube = existing.SkillGastroTube,
                SkillBedToilet = existing.SkillBedToilet,
                SkillVaccination = existing.SkillVaccination,
                SkillDressing = existing.SkillDressing,
                SkillBadSore = existing.SkillBadSore,
                SkillHemoGlucoTest = existing.SkillHemoGlucoTest,
                SkillSmoke = existing.SkillSmoke,
                SkillPulsePressure = existing.SkillPulsePressure,
                SkillTransportation = existing.SkillTransportation,
                SkillFirstAideCourse = existing.SkillFirstAideCourse,

                AvailabilityDailyShift = existing.AvailabilityDailyShift,
                AvailabilityNightShift = existing.AvailabilityNightShift,
                Availability24Hours = existing.Availability24Hours,
                AvailabilityWorkInHospital = existing.AvailabilityWorkInHospital,

                Interviewer = existing.Interviewer,
                InterviewDate = existing.InterviewDate
            };
            OnPropertyChanged(nameof(HeaderText));
        }


        #region Commands
        private void InitCommands()
        {
            SaveCommand = new RelayCommand(async () => await SaveAsync(), CanSave);
            ClearCommand = new RelayCommand(Clear);
        }

        public ICommand SaveCommand { get; private set; } = null!;
        public ICommand ClearCommand { get; private set; } = null!;
        #endregion

        #region Data
        public CreateNurseDto Nurse
        {
            get => _nurse;
            set { _nurse = value; OnPropertyChanged(); }
        }

        public Array Positions => Enum.GetValues(typeof(NursePosition));
        #endregion

        #region Error
        public string Error
        {
            get => _error;
            private set { _error = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasErrors)); }
        }

        public bool HasErrors => !string.IsNullOrWhiteSpace(Error);
        #endregion

        #region Logic
        private bool CanSave() =>
            !string.IsNullOrWhiteSpace(Nurse.FullName) &&
            !string.IsNullOrWhiteSpace(Nurse.PhoneNumber);

        private async System.Threading.Tasks.Task SaveAsync()
        {
            try
            {
                Error = "";

                if (!CanSave())
                {
                    Error = "Full Name and Phone Number are required.";
                    return;
                }

                if (_isEditMode && _existingId.HasValue)
                {
                    await _api.UpdateNurseAsync(_existingId.Value, Nurse);
                    MessageBox.Show("✅ Nurse updated successfully!", "Updated",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    await _api.CreateNurseAsync(Nurse);
                    MessageBox.Show("✅ Nurse created successfully!", "Created",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    Clear();
                }
            }
            catch (Exception ex)
            {
                Error = $"Failed to save nurse: {ex.Message}";
            }
        }

        private void Clear()
        {
            if (_isEditMode) return;

            Nurse = new CreateNurseDto
            {
                FullName = "",
                Position = NursePosition.RN,
                PhoneNumber = ""
            };

            Error = "";
        }
        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
        public string HeaderText => _isEditMode ? "Update Nurse" : "Create Nurse";
    }
}
