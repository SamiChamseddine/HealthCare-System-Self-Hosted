using Florence.Desktop.Models;
using Florence.Desktop.Services;
using Florence.Desktop.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Florence.Desktop.Views
{
    public partial class ExpenseReportForm : Page
    {
        private readonly ApiService _apiService = new();
        public ExpenseReportViewModel ViewModel { get; private set; }

        public ExpenseReportForm()
        {
            InitializeComponent();
            ViewModel = new ExpenseReportViewModel();
            DataContext = ViewModel;

            Loaded += async (_, __) => await InitializeAsync();
        }

        public ExpenseReportForm(ExpenseReportDto existingReport)
        {
            InitializeComponent();
            ViewModel = new ExpenseReportViewModel();
            DataContext = ViewModel;

            Loaded += async (_, __) =>
            {
                await InitializeAsync();

                ViewModel.LoadFromDto(existingReport);

            };
        }


        private async Task InitializeAsync()
        {
            try
            {
                await ViewModel.LoadLookupDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error loading patient/nurse data:\n{ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
        }

        private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = true;

                var arg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
                {
                    RoutedEvent = UIElement.MouseWheelEvent,
                    Source = sender
                };

                DependencyObject parent = VisualTreeHelper.GetParent((DependencyObject)sender);

                while (parent != null && parent is not UIElement)
                    parent = VisualTreeHelper.GetParent(parent);

                (parent as UIElement)?.RaiseEvent(arg);
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            await SaveAsync();
        }

        private async Task SaveAsync()
        {
            try
            {
                var dto = ViewModel.ToCreateDto();

                if (ViewModel.IsEditMode)
                {
                    await _apiService.UpdateExpenseReportAsync(ViewModel.Id, dto);
                    MessageBox.Show("Expense Report updated successfully!", "Success");
                }
                else
                {
                    await _apiService.CreateExpenseReportAsync(dto);
                    MessageBox.Show("Expense Report created successfully!", "Success");
                }

                ((MainWindow)Application.Current.MainWindow)
                    .ContentFrame.Navigate(new HomePage());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to save expense report:\n{ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ViewModel = new ExpenseReportViewModel();
            DataContext = ViewModel;

            _ = InitializeAsync();
        }
    }
}
