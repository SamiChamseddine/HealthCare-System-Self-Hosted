using Florence.Desktop.Models;
using Florence.Desktop.Services;
using System.Windows;
using System.Windows.Input;

namespace Florence.Desktop.Views
{
    public partial class CustomerViewWindow : Window
    {
        private readonly ApiService _apiService = new();

        public CustomerViewWindow(CustomerDto customer)
        {
            InitializeComponent();
            DataContext = customer;
            Loaded += async (_, _) =>
            {
                try
                {
                    var invoices = await _apiService.GetCustomerInvoicesAsync(customer.Id);
                    InvoicesGrid.ItemsSource = invoices;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to load invoices: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };
        }
        private void InvoicesGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (InvoicesGrid.SelectedItem is not InvoiceDto invoice)
                return;

            var viewWindow = new InvoiceViewWindow(invoice)
            {
                Owner = this 
            };

            viewWindow.ShowDialog();
        }


        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
