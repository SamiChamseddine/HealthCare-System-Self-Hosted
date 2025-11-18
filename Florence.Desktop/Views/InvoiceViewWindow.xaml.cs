using Florence.Desktop.Documents;
using Florence.Desktop.Models;
using Florence.Desktop.Services;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Diagnostics;
using System.IO;
using System.Printing;
using System.Windows;
using System.Windows.Input;

namespace Florence.Desktop.Views
{
    public partial class InvoiceViewWindow : Window
    {
        private readonly InvoiceDto _invoice;

        public InvoiceViewWindow(InvoiceDto invoice)
        {
            InitializeComponent();
            _invoice = invoice;
            DataContext = invoice;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void Print_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                var pdfPath = Path.Combine(desktopPath, $"Invoice_{_invoice.InvoiceNumber:D4}.pdf");

                var api = new ApiService();
                var customer = await api.GetCustomerByIdAsync(_invoice.CustomerId);

                if (customer == null)
                {
                    MessageBox.Show("Customer not found. Cannot generate invoice.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var document = new InvoiceDocument(_invoice, customer);
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

                MessageBox.Show($"Invoice saved and opened!\n{pdfPath}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Printing failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
                {
                    RoutedEvent = UIElement.MouseWheelEvent,
                    Source = sender
                };
                MainScrollViewer.RaiseEvent(eventArg);
            }
        }
    }
}