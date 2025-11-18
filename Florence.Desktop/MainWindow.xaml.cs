using System.Collections.ObjectModel;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.IconPacks;
using Florence.Desktop.Views;

namespace Florence.Desktop
{
    public partial class MainWindow : MetroWindow
    {
        public ObservableCollection<NavItem> MenuItems { get; set; } = new();

        public MainWindow()
        {
            InitializeComponent();

            MenuItems.Add(new NavItem("Home", PackIconMaterialKind.HomeOutline));
            MenuItems.Add(new NavItem("Create Invoice", PackIconMaterialKind.FilePlusOutline));
            MenuItems.Add(new NavItem("Create Expense Report", PackIconMaterialKind.FileDocumentEditOutline));
            MenuItems.Add(new NavItem("Create Customer", PackIconMaterialKind.AccountPlusOutline));
            MenuItems.Add(new NavItem("Create Patient", PackIconMaterialKind.AccountPlus));
            MenuItems.Add(new NavItem("Create Nurse", PackIconMaterialKind.MedicalBag));


            DataContext = this;

            Loaded += (_, _) => ContentFrame.Navigate(new HomePage());
        }

        private void RootNav_ItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs e)
        {
            if (e.InvokedItem is not NavItem item)
                return;

            switch (item.Label)
            {
                case "Home":
                    ContentFrame.Navigate(new HomePage());
                    break;
                case "Create Invoice":
                    ContentFrame.Navigate(new InvoiceForm());
                    break;
                case "Create Expense Report":
                    ContentFrame.Navigate(new ExpenseReportForm());
                    break;
                case "Create Customer":
                    ContentFrame.Navigate(new CustomerForm());
                    break;
                case "Create Patient":
                    ContentFrame.Navigate(new PatientForm());
                    break;
                case "Create Nurse":
                    ContentFrame.Navigate(new NurseForm());
                    break;
            }
        }
    }

    public class NavItem
    {
        public string Label { get; set; }
        public PackIconMaterialKind Icon { get; set; }

        public NavItem(string label, PackIconMaterialKind icon)
        {
            Label = label;
            Icon = icon;
        }
    }
}
