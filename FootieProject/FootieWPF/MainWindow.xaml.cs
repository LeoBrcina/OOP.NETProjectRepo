using DAO.Models;
using DAO.Repos.Implementations;
using DAO.Services;
using FootieWPF.ViewModels;
using FootieWPF.Views;
using System.Windows;

namespace FootieWPF
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        // konstruktor za main window zajedno sa dohvaćanjem i postavljanjem potrebnih postavki
        public MainWindow()
        {
            InitializeComponent();

            var apiService = API.Instance;
            var fileRepository = new FileRepository();

            var settings = fileRepository.GetSettings();
            string worldCupSelection = settings.Length >= 1 ? settings[0] : "Men's World Cup 2018";
            string selectedLanguage = settings.Length >= 2 ? settings[1] : "English";
            string selectedTeamFifaCode = settings.Length >= 3 ? settings[2] : "";

            apiService.ConfigureUrls(worldCupSelection);

            _viewModel = new MainViewModel(worldCupSelection, selectedLanguage, selectedTeamFifaCode, apiService);

            DataContext = _viewModel;

            ApplyWindowResolution(fileRepository.GetResolution());
        }

        // pomoćna metoda za postavljanje rezolucije main windowa
        private void ApplyWindowResolution(string resolution)
        {
            if (resolution == "Fullscreen")
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                var dimensions = resolution?.Split('x');
                if (dimensions != null && dimensions.Length == 2 && int.TryParse(dimensions[0], out int width) && int.TryParse(dimensions[1], out int height))
                {
                    Width = width;
                    Height = height;
                }
            }
        }
    }
}
