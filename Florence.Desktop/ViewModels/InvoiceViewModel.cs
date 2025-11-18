using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Florence.Desktop.Models;
using Florence.Desktop.Services;
using Florence.Desktop.Utils;

namespace Florence.Desktop.ViewModels
{
    public class InvoiceViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private CreateInvoiceDto _invoice = new();
        private string _error = "";
        private List<CustomerDto> _customers = new();

        private string _vatRateText = "11.00";
        private string _discountPercentText = "0.00";

        public bool IsEditing { get; set; }
        public string HeaderText { get; set; } = "Create New Invoice";

        public InvoiceViewModel(ApiService apiService)
        {
            _apiService = apiService;

            SaveCommand = new RelayCommand(async () => await SaveAsync(), CanSave);
            ClearCommand = new RelayCommand(Clear);
            AddItemCommand = new RelayCommand(AddItem);
            RemoveItemCommand = new RelayCommand<CreateInvoiceItemDto>(RemoveItemExecute);
            ConfirmItemCommand = new RelayCommand<CreateInvoiceItemDto>(ConfirmItemExecute);

            InitializeAsync();
            Clear();
            AttachItemCollectionEvents();
        }

        public InvoiceViewModel(ApiService apiService, InvoiceDto existingInvoice)
            : this(apiService)
        {
            IsEditing = true;
            HeaderText = $"Updating Invoice ID {existingInvoice.Id}";

            Invoice = new CreateInvoiceDto
            {
                Id = existingInvoice.Id,
                CustomerId = existingInvoice.CustomerId,
                DueDate = existingInvoice.DueDate,
                VatRate = existingInvoice.VatRate,
                DiscountPercent = existingInvoice.DiscountPercent,
                Currency = existingInvoice.Currency,
                PaymentMethod = existingInvoice.PaymentMethod,
                Notes = existingInvoice.Notes,
                Items = new ObservableCollection<CreateInvoiceItemDto>(
                    existingInvoice.Items.Select(i => new CreateInvoiceItemDto
                    {
                        Description = i.Description,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice
                    }))
            };

            _vatRateText = Invoice.VatRate.ToString("F2", CultureInfo.CurrentCulture);
            _discountPercentText = Invoice.DiscountPercent.ToString("F2", CultureInfo.CurrentCulture);

            AttachItemCollectionEvents();
            RecalculateTotals();
            OnPropertyChanged(nameof(Invoice));
        }

        public CreateInvoiceDto Invoice
        {
            get => _invoice;
            set
            {
                _invoice = value;
                OnPropertyChanged();
                RecalculateTotals();
            }
        }

        public List<CustomerDto> Customers
        {
            get => _customers;
            private set
            {
                _customers = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<CreateInvoiceItemDto> Items => _invoice.Items;
        private void AttachItemCollectionEvents()
        {
            // attach to existing
            foreach (var item in Items)
                item.PropertyChanged += Item_PropertyChanged;

            Items.CollectionChanged += (s, e) =>
            {
                if (e.NewItems != null)
                    foreach (CreateInvoiceItemDto i in e.NewItems)
                        i.PropertyChanged += Item_PropertyChanged;

                if (e.OldItems != null)
                    foreach (CreateInvoiceItemDto i in e.OldItems)
                        i.PropertyChanged -= Item_PropertyChanged;

                RecalculateTotals();
            };
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is nameof(CreateInvoiceItemDto.Quantity)
                or nameof(CreateInvoiceItemDto.UnitPrice)
                or nameof(CreateInvoiceItemDto.Description)
                or nameof(CreateInvoiceItemDto.Total))
            {
                RecalculateTotals();
            }
        }
        public string VatRateText
        {
            get => _vatRateText;
            set
            {
                if (_vatRateText == value) return;
                _vatRateText = value;

                if (decimal.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out var parsed))
                {
                    parsed = Math.Clamp(parsed, 0, 100);
                    _invoice.VatRate = parsed;
                    RecalculateTotals();
                }

                OnPropertyChanged();
            }
        }

        public string DiscountPercentText
        {
            get => _discountPercentText;
            set
            {
                if (_discountPercentText == value) return;
                _discountPercentText = value;

                if (decimal.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out var parsed))
                {
                    parsed = Math.Clamp(parsed, 0, 100);
                    _invoice.DiscountPercent = parsed;
                    RecalculateTotals();
                }

                OnPropertyChanged();
            }
        }
        public decimal Subtotal => Items.Sum(i => i.Quantity * i.UnitPrice);
        public decimal DiscountAmount => Subtotal * (_invoice.DiscountPercent / 100m);
        public decimal AmountAfterDiscount => Subtotal - DiscountAmount;
        public decimal VatAmount => AmountAfterDiscount * (_invoice.VatRate / 100m);
        public decimal Total => AmountAfterDiscount + VatAmount;

        private void RecalculateTotals()
        {
            OnPropertyChanged(nameof(Subtotal));
            OnPropertyChanged(nameof(DiscountAmount));
            OnPropertyChanged(nameof(AmountAfterDiscount));
            OnPropertyChanged(nameof(VatAmount));
            OnPropertyChanged(nameof(Total));
        }
        public ICommand SaveCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand AddItemCommand { get; }
        public ICommand RemoveItemCommand { get; }
        public ICommand ConfirmItemCommand { get; }

        private bool CanSave() =>
            _invoice.CustomerId > 0 &&
            Items.Any(i => !string.IsNullOrWhiteSpace(i.Description) && i.UnitPrice > 0);

        private async Task SaveAsync()
        {
            try
            {
                Error = "";

                if (_invoice.CustomerId <= 0)
                {
                    Error = "Please select a customer.";
                    return;
                }

                if (Subtotal <= 0)
                {
                    Error = "Invoice must have at least one valid item.";
                    return;
                }

                _invoice.Subtotal = Subtotal;
                _invoice.DiscountAmount = DiscountAmount;
                _invoice.VatAmount = VatAmount;
                _invoice.Total = Total;

                if (IsEditing)
                {
                    await _apiService.UpdateInvoiceAsync(_invoice.Id, _invoice);
                    System.Windows.MessageBox.Show("Invoice updated successfully!");
                }
                else
                {
                    await _apiService.CreateInvoiceAsync(_invoice);
                    System.Windows.MessageBox.Show("Invoice created successfully!");
                }

                ((MainWindow)System.Windows.Application.Current.MainWindow)
                    .ContentFrame.Navigate(new Views.HomePage());
            }
            catch (Exception ex)
            {
                Error = $"Failed to save invoice: {ex.Message}";
            }
        }

        private void Clear()
        {
            Invoice = new CreateInvoiceDto
            {
                DueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(15)),
                VatRate = 11m,
                DiscountPercent = 0m,
                Currency = "USD",
                PaymentMethod = "Cash",
                Items = new ObservableCollection<CreateInvoiceItemDto> { new() }
            };

            _vatRateText = "11.00";
            _discountPercentText = "0.00";
            Error = "";

            AttachItemCollectionEvents();
            RecalculateTotals();
        }

        private void AddItem() =>
            Items.Add(new CreateInvoiceItemDto { Quantity = 1, IsNew = true });

        private void ConfirmItemExecute(CreateInvoiceItemDto item)
        {
            if (item == null) return;
            item.IsNew = false;
            item.IsConfirmed = true;
            RecalculateTotals();
        }

        private void RemoveItemExecute(object parameter)
        {
            if (parameter is CreateInvoiceItemDto item && Items.Count > 1)
            {
                Items.Remove(item);
            }

            RecalculateTotals();
        }

        public string Error
        {
            get => _error;
            private set
            {
                _error = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasErrors));
            }
        }

        public bool HasErrors => !string.IsNullOrWhiteSpace(Error);

        private async void InitializeAsync()
        {
            try
            {
                Customers = await _apiService.GetCustomersAsync();
                if (!_invoice.Items.Any())
                    _invoice.Items = new ObservableCollection<CreateInvoiceItemDto> { new() };

                RecalculateTotals();
            }
            catch (Exception ex)
            {
                Error = $"Failed to load customers: {ex.Message}";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
