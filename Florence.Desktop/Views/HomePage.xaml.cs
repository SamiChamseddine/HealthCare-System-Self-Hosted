using Florence.Desktop.Models;
using Florence.Desktop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Florence.Desktop.Views
{
    public partial class HomePage : Page
    {
        private readonly ApiService _apiService = new();

        private List<InvoiceDto> _allInvoices = new();
        private List<CustomerDto> _allCustomers = new();
        private List<PatientDto> _allPatients = new();
        private List<NurseDto> _allNurses = new();
        private List<ExpenseReportDto> _allExpenseReports = new();

        public HomePage()
        {
            InitializeComponent();
            Loaded += HomePage_Loaded;
        }

        private async void HomePage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await _apiService.CheckLateInvoicesAsync();

                _allInvoices = await _apiService.GetInvoicesAsync();
                _allCustomers = await _apiService.GetCustomersAsync();
                _allPatients = await _apiService.GetPatientsAsync();
                _allNurses = await _apiService.GetNursesAsync();
                _allExpenseReports = await _apiService.GetExpenseReportsAsync();

                InvoicesGrid.ItemsSource = _allInvoices;
                CustomersGrid.ItemsSource = _allCustomers;
                PatientsGrid.ItemsSource = _allPatients;
                NursesGrid.ItemsSource = _allNurses;
                ExpenseReportsGrid.ItemsSource = _allExpenseReports;

                ApplyInvoiceFilters();
                ApplyCustomerFilters();
                ApplyPatientFilters();
                ApplyNurseFilters();
                ApplyExpenseFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data:\n{ex.Message}", "Error");
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
        }
        private void InvoiceSearchBox_TextChanged(object sender, TextChangedEventArgs e)
            => ApplyInvoiceFilters();

        private void FilterInvoices(object sender, RoutedEventArgs e)
            => ApplyInvoiceFilters();

        private void ApplyInvoiceFilters()
        {
            if (_allInvoices.Count == 0) return;

            string search = InvoiceSearchBox.Text?.Trim().ToLower() ?? "";
            bool hideDeleted = HideDeletedCheckBox.IsChecked == true;

            var filtered = _allInvoices.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                filtered = filtered.Where(i =>
                    (i.CustomerName?.ToLower().Contains(search) == true) ||
                    i.Id.ToString().Contains(search) ||
                    i.Status.ToString().ToLower().Contains(search));
            }

            if (hideDeleted)
                filtered = filtered.Where(i => i.Status != InvoiceStatus.Deleted);

            InvoicesGrid.ItemsSource = filtered.ToList();
        }

        private void ExpenseSearchBox_TextChanged(object sender, TextChangedEventArgs e)
            => ApplyExpenseFilters();

        private void ApplyExpenseFilters()
        {
            if (_allExpenseReports.Count == 0) return;

            string search = ExpenseSearchBox.Text?.Trim().ToLower() ?? "";

            var filtered = _allExpenseReports.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                filtered = filtered.Where(r =>
                    r.Id.ToString().Contains(search) ||
                    (r.PatientName?.ToLower().Contains(search) == true) ||
                    ($"{r.StartDate:yyyy-MM-dd} - {r.EndDate:yyyy-MM-dd}")
                       .ToLower()
                       .Contains(search));
            }

            ExpenseReportsGrid.ItemsSource = filtered.ToList();
        }

        private void CustomerSearchBox_TextChanged(object sender, TextChangedEventArgs e)
            => ApplyCustomerFilters();

        private void ApplyCustomerFilters()
        {
            if (_allCustomers.Count == 0) return;

            string search = CustomerSearchBox.Text?.Trim().ToLower() ?? "";

            var filtered = _allCustomers.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                filtered = filtered.Where(c =>
                    c.Id.ToString().Contains(search) ||
                    (c.Name?.ToLower().Contains(search) == true) ||
                    (c.Email?.ToLower().Contains(search) == true) ||
                    (c.Phone?.ToLower().Contains(search) == true));
            }

            CustomersGrid.ItemsSource = filtered.ToList();
        }
        private void PatientSearchBox_TextChanged(object sender, TextChangedEventArgs e)
            => ApplyPatientFilters();

        private void ApplyPatientFilters()
        {
            if (_allPatients.Count == 0) return;

            string search = PatientSearchBox.Text?.Trim().ToLower() ?? "";

            var filtered = _allPatients.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                filtered = filtered.Where(p =>
                    p.Id.ToString().Contains(search) ||
                    (p.FullName?.ToLower().Contains(search) == true) ||
                    (p.PhoneNumber?.ToLower().Contains(search) == true) ||
                    p.Gender.ToString().ToLower().Contains(search));
            }

            PatientsGrid.ItemsSource = filtered.ToList();
        }
        private void NurseSearchBox_TextChanged(object sender, TextChangedEventArgs e)
            => ApplyNurseFilters();

        private void ApplyNurseFilters()
        {
            if (_allNurses.Count == 0) return;

            string search = NurseSearchBox.Text?.Trim().ToLower() ?? "";

            var filtered = _allNurses.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                filtered = filtered.Where(n =>
                    n.Id.ToString().Contains(search) ||
                    (n.FullName?.ToLower().Contains(search) == true) ||
                    n.Position.ToString().ToLower().Contains(search) ||
                    (n.PhoneNumber?.ToLower().Contains(search) == true));
            }

            NursesGrid.ItemsSource = filtered.ToList();
        }
        private async void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (MainTabs.SelectedItem is not TabItem tab) return;

            string header = tab.Header?.ToString() ?? "";

            switch (header)
            {
                case var h when h.Contains("Invoice", StringComparison.OrdinalIgnoreCase):
                    if (InvoicesGrid.SelectedItem is not InvoiceDto inv) { Warn("invoice"); return; }
                    NavigateTo(new InvoiceForm(inv));
                    break;

                case var h when h.Contains("Patient", StringComparison.OrdinalIgnoreCase):
                    if (PatientsGrid.SelectedItem is not PatientDto pat) { Warn("patient"); return; }
                    NavigateTo(new PatientForm(await _apiService.GetPatientByIdAsync(pat.Id)));
                    break;

                case var h when h.Contains("Nurse", StringComparison.OrdinalIgnoreCase):
                    if (NursesGrid.SelectedItem is not NurseDto nur) { Warn("nurse"); return; }
                    NavigateTo(new NurseForm(await _apiService.GetNurseByIdAsync(nur.Id)));
                    break;

                case var h when h.Contains("Customer", StringComparison.OrdinalIgnoreCase):
                    if (CustomersGrid.SelectedItem is not CustomerDto cust) { Warn("customer"); return; }
                    NavigateTo(new CustomerForm(cust));
                    break;

                case var h when h.Contains("Expense", StringComparison.OrdinalIgnoreCase):
                    if (ExpenseReportsGrid.SelectedItem is not ExpenseReportDto exp) { Warn("expense report"); return; }
                    NavigateTo(new ExpenseReportForm(exp));
                    break;

                default:
                    MessageBox.Show("Please select a valid tab.");
                    break;
            }
        }

        private void Warn(string name)
        {
            MessageBox.Show($"Please select a {name} first.");
        }

        private void NavigateTo(Page page)
        {
            if (Window.GetWindow(this) is not MainWindow window)
            {
                MessageBox.Show("Main window not found.");
                return;
            }

            window.ContentFrame.Navigate(page);
        }
        private void View_Click(object sender, RoutedEventArgs e)
        {
            if (MainTabs.SelectedItem is not TabItem tab) return;

            string header = tab.Header?.ToString() ?? "";

            if (header.Contains("Invoice") && InvoicesGrid.SelectedItem is InvoiceDto inv)
                Show(new InvoiceViewWindow(inv));

            else if (header.Contains("Patient") && PatientsGrid.SelectedItem is PatientDto pat)
                Show(new PatientViewWindow(pat));

            else if (header.Contains("Nurse") && NursesGrid.SelectedItem is NurseDto nur)
                Show(new NurseViewWindow(nur));

            else if (header.Contains("Customer") && CustomersGrid.SelectedItem is CustomerDto cust)
                Show(new CustomerViewWindow(cust));

            else if (header.Contains("Expense") && ExpenseReportsGrid.SelectedItem is ExpenseReportDto exp)
                Show(new ExpenseReportViewWindow(exp));

            else
                MessageBox.Show("Please select an item to view.");
        }

        private void Show(Window window)
        {
            window.Owner = Application.Current.MainWindow;
            window.ShowDialog();
        }
        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (MainTabs.SelectedItem is not TabItem tab) return;

            string header = tab.Header?.ToString() ?? "";

            object? selected = header switch
            {
                var h when h.Contains("Invoice") => InvoicesGrid.SelectedItem,
                var h when h.Contains("Customer") => CustomersGrid.SelectedItem,
                var h when h.Contains("Patient") => PatientsGrid.SelectedItem,
                var h when h.Contains("Nurse") => NursesGrid.SelectedItem,
                var h when h.Contains("Expense") => ExpenseReportsGrid.SelectedItem,
                _ => null
            };

            if (selected == null)
            {
                MessageBox.Show("Please select an item to delete.");
                return;
            }

            string name = selected switch
            {
                InvoiceDto => "invoice",
                CustomerDto => "customer",
                PatientDto => "patient",
                NurseDto => "nurse",
                ExpenseReportDto => "expense report",
                _ => "item"
            };

            if (MessageBox.Show($"Are you sure you want to delete this {name}?",
                    "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                != MessageBoxResult.Yes) return;

            try
            {
                switch (selected)
                {
                    case InvoiceDto inv:
                        await _apiService.DeleteInvoiceAsync(inv.Id);
                        _allInvoices = await _apiService.GetInvoicesAsync();
                        ApplyInvoiceFilters();
                        break;

                    case CustomerDto cust:
                        await _apiService.DeleteCustomerAsync(cust.Id);
                        _allCustomers = await _apiService.GetCustomersAsync();
                        ApplyCustomerFilters();
                        break;

                    case PatientDto pat:
                        await _apiService.DeletePatientAsync(pat.Id);
                        _allPatients = await _apiService.GetPatientsAsync();
                        ApplyPatientFilters();
                        break;

                    case NurseDto nur:
                        await _apiService.DeleteNurseAsync(nur.Id);
                        _allNurses = await _apiService.GetNursesAsync();
                        ApplyNurseFilters();
                        break;

                    case ExpenseReportDto exp:
                        await _apiService.DeleteExpenseReportAsync(exp.Id);
                        _allExpenseReports = await _apiService.GetExpenseReportsAsync();
                        ApplyExpenseFilters();
                        break;
                }

                MessageBox.Show($"{name} deleted successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to delete {name}:\n{ex.Message}", "Error");
            }
        }
        private async void MarkPaid_Click(object sender, RoutedEventArgs e)
        {
            if (InvoicesGrid.SelectedItem is not InvoiceDto invoice)
            {
                MessageBox.Show("Please select an invoice first.");
                return;
            }

            if (invoice.Status == InvoiceStatus.Deleted)
            {
                MessageBox.Show("Cannot mark deleted invoice as paid.");
                return;
            }

            if (invoice.Status == InvoiceStatus.Paid)
            {
                MessageBox.Show("Invoice is already paid.");
                return;
            }

            if (MessageBox.Show($"Mark Invoice #{invoice.Id} as PAID?",
                "Confirm", MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;

            try
            {
                await _apiService.MarkInvoicePaidAsync(invoice.Id);
                _allInvoices = await _apiService.GetInvoicesAsync();
                ApplyInvoiceFilters();

                MessageBox.Show("Invoice marked as paid.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error:\n{ex.Message}");
            }
        }
    }
}
