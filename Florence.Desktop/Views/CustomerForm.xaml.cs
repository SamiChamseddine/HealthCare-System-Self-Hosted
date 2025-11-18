using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Florence.Desktop.Services;
using Florence.Desktop.ViewModels;
using Florence.Desktop.Models;

namespace Florence.Desktop.Views
{
    public partial class CustomerForm : Page
    {
        public CustomerForm()
        {
            InitializeComponent();
            DataContext = new CustomerViewModel(new ApiService());
        }

        public CustomerForm(CustomerDto existingCustomer)
        {
            InitializeComponent();
            DataContext = new CustomerViewModel(new ApiService(), existingCustomer);
        }

        private void PhoneTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");
        }

        private void PhoneTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = (string)e.DataObject.GetData(typeof(string));
                if (!Regex.IsMatch(text, "^[0-9]+$"))
                    e.CancelCommand();
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void BackToDashboardButton_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
