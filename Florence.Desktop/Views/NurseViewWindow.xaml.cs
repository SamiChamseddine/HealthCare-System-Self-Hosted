using System;
using System.Windows;
using Florence.Desktop.Models;
using Florence.Desktop.Services;
using MahApps.Metro.Controls;

namespace Florence.Desktop.Views
{
    public partial class NurseViewWindow : MetroWindow
    {
        public NurseViewWindow(NurseDto summary)
        {
            InitializeComponent();
            LoadFullNurse(summary.Id);
        }

        private async void LoadFullNurse(int id)
        {
            try
            {
                var api = new ApiService();
                var detailed = await api.GetNurseByIdAsync(id);
                if (detailed == null)
                {
                    MessageBox.Show("Failed to load nurse details.");
                    Close();
                    return;
                }

                DataContext = detailed;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading nurse: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
