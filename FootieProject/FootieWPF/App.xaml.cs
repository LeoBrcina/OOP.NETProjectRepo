using DAO.Repos.Implementations;
using DAO.Services;
using FootieWPF.ViewModels;
using FootieWPF.Views;
using System.Windows;

namespace FootieWPF
{
    public partial class App : Application
    {
        // metoda kojom provjeravamo i odabiremo koji ćemo prozor pokrenuti ovisno o tome postoji li file zadužen za postavke
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            FileRepository fileRepo = new FileRepository();
            API apiService = API.Instance;

            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            if (!fileRepo.SettingsExist())
            {
                OpenSettingsWindow(fileRepo, apiService);
            }
            else
            {
                OpenMainWindow(fileRepo, apiService);
            }
        }

        // metoda za otvaranje settings windowa sa svim potrebnim podacima te rutinskim provjerama čime također određujemo hoćemo li otvoriti glavni window
        private void OpenSettingsWindow(FileRepository fileRepo, API apiService)
        {
            SettingsViewModel settingsViewModel = new SettingsViewModel(fileRepo, apiService);
            var settingsWindow = new SettingsWindow { DataContext = settingsViewModel };

            bool? result = settingsWindow.ShowDialog();

            if (settingsViewModel.SettingsApplied)
            {
                OpenMainWindow(fileRepo, apiService);
            }
            else
            {
                if (result == false || result == null) 
                {
                    MessageBox.Show("No settings applied. Shutting down the application.", "Shutdown", MessageBoxButton.OK, MessageBoxImage.Information);
                    Shutdown(); 
                }
            }
        }

        // metoda za otvaranje main windowa sa svim potrebnim podacima i provjerama
        private void OpenMainWindow(FileRepository fileRepo, API apiService)
        {
            try
            {
                var settings = fileRepo.GetSettings();
                string selectedWorldCup = settings.Length >= 1 ? settings[0] : "Men's World Cup 2018";
                string selectedLanguage = settings.Length >= 2 ? settings[1] : "English";
                string selectedTeamFifaCode = settings.Length >= 3 ? settings[2] : "";
                string selectedResolution = fileRepo.GetResolution() ?? "Fullscreen";

                apiService.ConfigureUrls(selectedWorldCup);

                var mainWindowViewModel = new MainViewModel(selectedWorldCup, selectedLanguage, selectedTeamFifaCode, apiService);
                var mainWindow = new MainWindow { DataContext = mainWindowViewModel };

                ApplyWindowResolution(mainWindow, selectedResolution);

                mainWindow.Closed += (s, args) =>
                {
                    Application.Current.Shutdown();
                };

                mainWindow.Show();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Failed to open Main Window: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();  
            }
        }

        // pomoćna metoda za primjenjivanje odabrane rezolucije na main window
        private void ApplyWindowResolution(MainWindow mainWindow, string selectedResolution)
        {
            if (string.IsNullOrEmpty(selectedResolution))
            {
                mainWindow.WindowState = WindowState.Maximized;
                return;
            }

            if (selectedResolution == "Fullscreen")
            {
                mainWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                var dimensions = selectedResolution.Split('x');
                if (dimensions.Length == 2 && int.TryParse(dimensions[0], out int width) && int.TryParse(dimensions[1], out int height))
                {
                    mainWindow.WindowState = WindowState.Normal;
                    mainWindow.Width = width;
                    mainWindow.Height = height;
                    mainWindow.Left = (SystemParameters.PrimaryScreenWidth - mainWindow.Width) / 2;
                    mainWindow.Top = (SystemParameters.PrimaryScreenHeight - mainWindow.Height) / 2;
                }
            }
        }

    }
}
