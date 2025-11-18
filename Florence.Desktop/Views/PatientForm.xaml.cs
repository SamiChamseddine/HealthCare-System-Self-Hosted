using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Florence.Desktop.Models;
using Florence.Desktop.Services;
using Florence.Desktop.ViewModels;

namespace Florence.Desktop.Views
{
    public partial class PatientForm : Page
    {
        private readonly PatientViewModel _vm;

        public PatientForm()
        {
            InitializeComponent();
            _vm = new PatientViewModel(new ApiService());
            DataContext = _vm;
        }

        public PatientForm(PatientDtoFull existingPatient)
        {
            InitializeComponent();
            DataContext = new PatientViewModel(new ApiService(), existingPatient);
        }


        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
        }
    }
}
