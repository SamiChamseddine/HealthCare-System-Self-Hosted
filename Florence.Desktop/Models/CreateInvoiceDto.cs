using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Florence.Desktop.Models
{
    public class CreateInvoiceDto
    {
        public int Id { get; set; }
        public DateOnly DueDate { get; set; }
        public int CustomerId { get; set; }
        public decimal Subtotal { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal VatRate { get; set; } = 0.11m;
        public decimal VatAmount { get; set; }
        public decimal Total { get; set; }
        public string Currency { get; set; } = "USD";
        public string PaymentMethod { get; set; } = "Cash";
        public string Notes { get; set; } = string.Empty;
        public DateOnly DateCreated { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public ObservableCollection<CreateInvoiceItemDto> Items { get; set; } = new();
    }

    public class CreateInvoiceItemDto : INotifyPropertyChanged
    {
        private string _description = string.Empty;
        private decimal _quantity = 1;
        private decimal _unitPrice;
        private bool _isNew = true;
        private bool _isConfirmed = false;

        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); OnPropertyChanged(nameof(Total)); }
        }

        public decimal Quantity
        {
            get => _quantity;
            set { _quantity = value; OnPropertyChanged(); OnPropertyChanged(nameof(Total)); }
        }

        public decimal UnitPrice
        {
            get => _unitPrice;
            set { _unitPrice = value; OnPropertyChanged(); OnPropertyChanged(nameof(Total)); }
        }

        [JsonIgnore]
        public bool IsNew
        {
            get => _isNew;
            set { _isNew = value; OnPropertyChanged(); }
        }

        [JsonIgnore]
        public bool IsConfirmed
        {
            get => _isConfirmed;
            set { _isConfirmed = value; OnPropertyChanged(); }
        }

        public decimal Total => Quantity * UnitPrice;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
