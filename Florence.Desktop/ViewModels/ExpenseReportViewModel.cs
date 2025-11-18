using Florence.Desktop.Models;
using Florence.Desktop.Services;
using Florence.Desktop.Utils;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Florence.Desktop.ViewModels
{
    public class ExpenseReportViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService = new();

        public event PropertyChangedEventHandler? PropertyChanged;
        private void Notify(string prop)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public int Id { get; set; }

        private int _patientId;
        public int PatientId
        {
            get => _patientId;
            set { _patientId = value; Notify(nameof(PatientId)); }
        }

        private DateOnly _startDate = DateOnly.FromDateTime(DateTime.Today);
        public DateOnly StartDate
        {
            get => _startDate;
            set { _startDate = value; Notify(nameof(StartDate)); }
        }

        private DateOnly _endDate = DateOnly.FromDateTime(DateTime.Today);
        public DateOnly EndDate
        {
            get => _endDate;
            set { _endDate = value; Notify(nameof(EndDate)); }
        }

        private decimal _totalAmount;
        public decimal TotalAmount
        {
            get => _totalAmount;
            set { _totalAmount = value; Notify(nameof(TotalAmount)); }
        }

        private decimal _totalHours;
        public decimal TotalHours
        {
            get => _totalHours;
            set { _totalHours = value; Notify(nameof(TotalHours)); }
        }


        public ObservableCollection<PatientDto> Patients { get; } = new();
        public ObservableCollection<NurseDto> Nurses { get; } = new();

        private ObservableCollection<ExpenseItemViewModel> _items = new();
        public ObservableCollection<ExpenseItemViewModel> Items
        {
            get => _items;
            set
            {
                if (_items != null)
                {
                    _items.CollectionChanged -= Items_CollectionChanged;

                    foreach (var item in _items)
                        item.PropertyChanged -= Item_PropertyChanged;
                }

                _items = value;
                Notify(nameof(Items));

                if (_items != null)
                {
                    _items.CollectionChanged += Items_CollectionChanged;

                    foreach (var item in _items)
                        item.PropertyChanged += Item_PropertyChanged;
                }

                RecalculateTotals();
            }
        }

        private ExpenseItemViewModel? _selectedItem;
        public ExpenseItemViewModel? SelectedItem
        {
            get => _selectedItem;
            set { _selectedItem = value; Notify(nameof(SelectedItem)); }
        }

        public ICommand AddItemCommand { get; }
        public ICommand RemoveItemCommand { get; }

        public ExpenseReportViewModel()
        {
            AddItemCommand = new RelayCommand(AddItem);
            RemoveItemCommand = new RelayCommand<ExpenseItemViewModel>(RemoveItem);

            Items.CollectionChanged += Items_CollectionChanged;
        }

        public async Task LoadLookupDataAsync()
        {
            var patients = await _apiService.GetPatientsAsync();
            var nurses = await _apiService.GetNursesAsync();

            Patients.Clear();
            foreach (var p in patients)
                Patients.Add(p);

            Nurses.Clear();
            foreach (var n in nurses)
                Nurses.Add(n);

            Notify(nameof(Patients));
            Notify(nameof(Nurses));
        }

        public bool IsEditMode { get; set; } = false;

        public string HeaderText { get; set; } = "Create New Expense Report";
        public string SaveButtonText => IsEditMode ? "Update" : "Create";

        public void LoadFromDto(ExpenseReportDto dto)
        {
            IsEditMode = true;
            Id = dto.Id;

            HeaderText = $"Updating Expense Report #{dto.Id}";
            Notify(nameof(HeaderText));
            Notify(nameof(SaveButtonText));

            PatientId = dto.PatientId;
            StartDate = dto.StartDate;
            EndDate = dto.EndDate;

            Items = new ObservableCollection<ExpenseItemViewModel>(
                dto.Items.Select(i =>
                {
                    var vm = new ExpenseItemViewModel
                    {
                        NurseId = i.NurseId,
                        Date = i.Date,
                        Description = i.Description,
                        Hours = i.Hours,
                        Amount = i.Amount
                    };
                    return vm;
                })
            );

        }

        private void Items_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (ExpenseItemViewModel item in e.NewItems)
                    item.PropertyChanged += Item_PropertyChanged;

            if (e.OldItems != null)
                foreach (ExpenseItemViewModel item in e.OldItems)
                    item.PropertyChanged -= Item_PropertyChanged;

            RecalculateTotals();
        }

        private void Item_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ExpenseItemViewModel.Hours) ||
                e.PropertyName == nameof(ExpenseItemViewModel.Amount))
            {
                RecalculateTotals();
            }
        }

        public void AddItem()
        {
            Items.Add(new ExpenseItemViewModel
            {
                Date = DateOnly.FromDateTime(DateTime.Today)
            });
        }

        public void RemoveItem(ExpenseItemViewModel vm)
        {
            if (vm != null)
                Items.Remove(vm);
        }

        public void RecalculateTotals()
        {
            TotalAmount = Items.Sum(i => i.Amount);
            TotalHours = Items.Sum(i => i.Hours);
        }


        public CreateExpenseReportDto ToCreateDto()
        {
            return new CreateExpenseReportDto
            {
                PatientId = PatientId,
                StartDate = StartDate,
                EndDate = EndDate,
                TotalAmount = TotalAmount,
                TotalHours = TotalHours,
                Items = Items.Select(i => new CreateExpenseItemDto
                {
                    NurseId = i.NurseId,
                    Date = i.Date,
                    Description = i.Description,
                    Hours = i.Hours,
                    Amount = i.Amount
                }).ToList()
            };
        }
    }


    public class ExpenseItemViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void Notify(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private int _nurseId;
        public int NurseId
        {
            get => _nurseId;
            set { _nurseId = value; Notify(nameof(NurseId)); }
        }

        private DateOnly _date;
        public DateOnly Date
        {
            get => _date;
            set { _date = value; Notify(nameof(Date)); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set { _description = value; Notify(nameof(Description)); }
        }

        private decimal _hours;
        public decimal Hours
        {
            get => _hours;
            set { _hours = value; Notify(nameof(Hours)); }
        }

        private decimal _amount;
        public decimal Amount
        {
            get => _amount;
            set { _amount = value; Notify(nameof(Amount)); }
        }
    }
}
