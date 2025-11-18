using ControlzEx.Theming;
using Florence.ApiHost;
using MahApps.Metro.Theming;
using Microsoft.AspNetCore.Builder;
using QuestPDF.Infrastructure;
using System.Net.Http;
using System.Windows;

namespace Florence.Desktop
{
    public partial class App : Application
    {
        private WebApplication? _apiHost;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                _apiHost = ApiHostBuilder.BuildApi();

                await _apiHost.StartAsync();

                var testClient = new HttpClient();
                var response = await testClient.GetAsync("http://localhost:5005/api/invoices");

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"❌ API returned {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ API FAILED TO START:\n{ex}");
                Shutdown();
                return;
            }

            QuestPDF.Settings.License = LicenseType.Community;
            ThemeManager.Current.ChangeTheme(this, "Dark.Blue");
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            if (_apiHost != null)
            {
                await _apiHost.StopAsync();
                await _apiHost.DisposeAsync();
            }

            base.OnExit(e);
        }
    }
}
