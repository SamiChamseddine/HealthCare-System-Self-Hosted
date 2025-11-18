using Florence.Desktop.Utils;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Florence.Desktop.Commands
{
    public static class NavigationCommands
    {
        public static ICommand BackToDashboardCommand { get; } =
            new RelayCommand(() =>
            {
                var currentWindow = Application.Current.Windows
                    .OfType<Window>()
                    .FirstOrDefault(w => w.IsActive);

                var navService = NavigationService.GetNavigationService(
                    currentWindow?.Content as DependencyObject
                );

                if (navService != null && navService.CanGoBack)
                {
                    navService.GoBack();
                    return;
                }

                var main = new MainWindow();
                main.Show();

                currentWindow?.Close();
            });
    }
}
