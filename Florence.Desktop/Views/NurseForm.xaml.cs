using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Florence.Desktop.Services;
using Florence.Desktop.ViewModels;
using Florence.Desktop.Models;


namespace Florence.Desktop.Views
{
    public partial class NurseForm : Page
    {
        private readonly NurseViewModel _vm;

        public NurseForm()
        {
            InitializeComponent();
            _vm = new NurseViewModel(new ApiService());
            DataContext = _vm;
        }
        public NurseForm(NurseDtoFull nurse)
        {
            InitializeComponent();
            DataContext = new NurseViewModel(new ApiService(), nurse);
        }



        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
        }
        private void ComboBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
            {
                RoutedEvent = UIElement.MouseWheelEvent,
                Source = sender
            };
            var parent = ((Control)sender).Parent as UIElement;
            parent?.RaiseEvent(eventArg);
        }

    }
}
