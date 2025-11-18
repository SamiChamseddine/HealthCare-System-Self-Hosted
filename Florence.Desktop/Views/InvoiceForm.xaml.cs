using Florence.Desktop.Models;
using Florence.Desktop.Services;
using Florence.Desktop.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Florence.Desktop.Views
{
    public partial class InvoiceForm : Page
    {
        private readonly InvoiceViewModel _viewModel;
        private ICollectionView _customersView;

        public InvoiceForm()
        {
            InitializeComponent();
            var apiService = new ApiService();

            _viewModel = new InvoiceViewModel(apiService)
            {
                HeaderText = "Create New Invoice",
                IsEditing = false
            };

            DataContext = _viewModel;
            Loaded += Page_Loaded;
        }

        public InvoiceForm(InvoiceDto invoice)
        {
            InitializeComponent();
            var apiService = new ApiService();

            _viewModel = new InvoiceViewModel(apiService, invoice)
            {
                HeaderText = $"Updating Invoice ID {invoice.Id}",
                IsEditing = true
            };

            DataContext = _viewModel;
            Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Customers != null && _viewModel.Customers.Any())
            {
                _customersView = CollectionViewSource.GetDefaultView(_viewModel.Customers);
                _customersView.Filter = CustomerFilter;
                CustomerComboBox.ItemsSource = _customersView;
            }
        }

        private bool CustomerFilter(object obj)
        {
            if (obj is not CustomerDto c) return false;

            var query = CustomerComboBox.Text?.Trim().ToLower() ?? string.Empty;
            if (string.IsNullOrEmpty(query)) return true;

            return (c.Name?.ToLower().Contains(query) ?? false)
                || (c.Email?.ToLower().Contains(query) ?? false)
                || (c.Phone?.ToLower().Contains(query) ?? false);
        }

        private void CustomerComboBox_KeyUp(object sender, KeyEventArgs e)
        {
            _customersView?.Refresh();
            CustomerComboBox.IsDropDownOpen = true;
        }

        private void CustomerComboBox_DropDownOpened(object sender, EventArgs e)
        {
            _customersView?.Refresh();
        }
        private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is UIElement element)
            {
                e.Handled = true;
                var e2 = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
                {
                    RoutedEvent = UIElement.MouseWheelEvent,
                    Source = sender
                };
                element.RaiseEvent(e2);
            }
        }

        private void PercentageTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is not TextBox tb) return;

            string decimalSep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            string allowed = "0-9" + Regex.Escape(decimalSep);
            string pattern = $"^[{allowed}]$";
            string proposedText = tb.Text.Insert(tb.SelectionStart, e.Text);

            if (!Regex.IsMatch(e.Text, pattern)
                || proposedText.Count(c => c.ToString() == decimalSep) > 1)
            {
                e.Handled = true;
                return;
            }

            if (decimal.TryParse(proposedText, NumberStyles.Number, CultureInfo.CurrentCulture, out var value))
            {
                e.Handled = value < 0 || value > 100;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void PercentageTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is not TextBox tb) return;

            if (decimal.TryParse(tb.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out var value))
            {
                value = Math.Clamp(value, 0, 100);
                tb.Text = value.ToString("F2", CultureInfo.CurrentCulture);
            }
            else
            {
                tb.Text = "0.00";
            }

            tb.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
        }
    }
}