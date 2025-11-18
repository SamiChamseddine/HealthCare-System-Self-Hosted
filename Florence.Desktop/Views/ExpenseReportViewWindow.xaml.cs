using Florence.Desktop.Documents;
using Florence.Desktop.Models;
using Florence.Desktop.Services;
using QuestPDF.Fluent;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Florence.Desktop.Views
{
    public partial class ExpenseReportViewWindow : Window
    {
        private readonly ExpenseReportDto _report;

        public ExpenseReportViewWindow(ExpenseReportDto report)
        {
            InitializeComponent();
            _report = report;
            DataContext = report;
        }

        private void Close_Click(object sender, RoutedEventArgs e) => Close();

        private async void Print_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                var pdfPath = Path.Combine(desktopPath, $"ExpenseReport_{_report.Id:D4}.pdf");

                var api = new ApiService();
                var patient = await api.GetPatientByIdAsync(_report.PatientId);

                if (patient == null)
                {
                    MessageBox.Show("Patient not found. Cannot generate expense report.",
                                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var document = new ExpenseReportDocument(_report, patient);
                document.GeneratePdf(pdfPath);

                var openProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = pdfPath,
                        UseShellExecute = true
                    }
                };
                openProcess.Start();

                MessageBox.Show($"Expense report saved and opened:\n{pdfPath}",
                                "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Printing failed: {ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

                var parent = VisualTreeHelper.GetParent((DependencyObject)sender);

                while (parent != null && parent is not UIElement)
                    parent = VisualTreeHelper.GetParent(parent);

                (parent as UIElement)?.RaiseEvent(arg);
            }
        }

    }
}
