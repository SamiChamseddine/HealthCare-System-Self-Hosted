using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Florence.Desktop.Models;
using Florence.Desktop.Services;
using Florence.Desktop.Utils;

namespace Florence.Desktop.ViewModels;

public class CustomerViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    private bool _isEditMode = false;
    private int _existingId;

    private CreateCustomerDto _customer = new();
    private string _error = "";

    public CreateCustomerDto Customer
    {
        get => _customer;
        set { _customer = value; OnPropertyChanged(); }
    }

    public string Error
    {
        get => _error;
        private set { _error = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasErrors)); }
    }

    public bool HasErrors => !string.IsNullOrWhiteSpace(Error);

    public ICommand SaveCommand { get; }
    public ICommand ClearCommand { get; }

    public CustomerViewModel(ApiService apiService)
    {
        _apiService = apiService;
        SaveCommand = new RelayCommand(async () => await SaveAsync(), CanSave);
        ClearCommand = new RelayCommand(Clear);
    }

    public CustomerViewModel(ApiService apiService, CustomerDto existing)
    {
        _apiService = apiService;
        _isEditMode = true;
        _existingId = existing.Id;

        Customer = new CreateCustomerDto
        {
            Name = existing.Name,
            Phone = existing.Phone,
            Address = existing.Address,
            Email = existing.Email
        };

        SaveCommand = new RelayCommand(async () => await SaveAsync(), CanSave);
        ClearCommand = new RelayCommand(Clear);

        OnPropertyChanged(nameof(HeaderText)); 
    }

    private bool CanSave() => !string.IsNullOrWhiteSpace(Customer.Name);

    private async Task SaveAsync()
    {
        try
        {
            Error = "";

            if (!string.IsNullOrEmpty(Customer.Phone) &&
                !Customer.Phone.All(char.IsDigit))
            {
                Error = "Phone number must contain digits only.";
                return;
            }

            if (_isEditMode)
            {
                await _apiService.UpdateCustomerAsync(_existingId, Customer);
                System.Windows.MessageBox.Show("Customer updated successfully!");
            }
            else
            {
                await _apiService.CreateCustomerAsync(Customer);
                System.Windows.MessageBox.Show("Customer created successfully!");
            }

            Clear();
        }
        catch (Exception ex)
        {
            Error = $"Failed to save customer: {ex.Message}";
        }
    }

    private void Clear()
    {
        if (_isEditMode)
            return; 

        Customer = new CreateCustomerDto();
        Error = "";
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public string HeaderText => _isEditMode ? "Update Customer" : "Create Customer";

}
