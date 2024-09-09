using DAO.Repos.Interfaces;
using DAO.Services;
using FootieWPF.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FootieWPF.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private readonly IFileRepository _fileRepo;
        private readonly API _apiService;

        private string _selectedWorldCup;
        private string _selectedLanguage;
        private string _selectedResolution;
        private string _selectedTeamFifaCode;

        public bool SettingsApplied { get; private set; }
        public ObservableCollection<string> WorldCupOptions { get; set; }
        public ObservableCollection<string> LanguageOptions { get; set; }
        public ObservableCollection<string> ResolutionOptions { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        // konstruktor za settings view model koji prima potrebni file repositoy kao i api servis za postavljanje aplikacije u odabrano radno okruženje
        public SettingsViewModel(IFileRepository fileRepo, API apiService)
        {
            _fileRepo = fileRepo;
            _apiService = apiService;

            WorldCupOptions = new ObservableCollection<string>
            {
                "Men's World Cup 2018",
                "Women's World Cup 2019"
            };

            LanguageOptions = new ObservableCollection<string>
            {
                "English",
                "Croatian"
            };

            ResolutionOptions = new ObservableCollection<string>
            {
                "Fullscreen",
                "1000x600",
                "1200x700",
                "1400x800"
            };

            LoadSettings();
        }

        public string SelectedWorldCup
        {
            get => _selectedWorldCup;
            set { _selectedWorldCup = value; OnPropertyChanged(); }
        }

        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set { _selectedLanguage = value; OnPropertyChanged(); }
        }

        public string SelectedResolution
        {
            get => _selectedResolution;
            set { _selectedResolution = value; OnPropertyChanged(); }
        }

        public string SelectedTeamFifaCode
        {
            get => _selectedTeamFifaCode;
            set { _selectedTeamFifaCode = value; OnPropertyChanged(); }
        }

        // komadne za primjenu postavki i gašenje aplikacije
        public ICommand ApplyCommand => new RelayCommand<object>(async param => await ConfirmApplySettings());
        public ICommand CancelCommand => new RelayCommand<object>(param => ConfirmExit());

        // metoda za učitavanje postavki iz file-a u comboboxeve za lakše korištenje
        private void LoadSettings()
        {
            if (_fileRepo.SettingsExist())
            {
                var settings = _fileRepo.GetSettings();
                SelectedWorldCup = settings.Length >= 1 ? settings[0] : WorldCupOptions[0];
                SelectedLanguage = settings.Length >= 2 ? settings[1] : LanguageOptions[0]; 
                SelectedTeamFifaCode = settings.Length >= 3 ? settings[2] : null;
                SelectedResolution = _fileRepo.GetResolution() ?? ResolutionOptions[0]; 
            }
        }

        // metoda koja primjenjuje odabrane postavke uz korisničku potvrdu
        private async Task ConfirmApplySettings()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to apply the settings?", "Confirm Apply", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                await SaveSettingsAsync();
            }
        }

        // metoda koja sprema podatke u file po potrebi
        private async Task SaveSettingsAsync()
        {
            if (IsSettingsValid()) 
            {
                _fileRepo.SaveSettings(SelectedWorldCup, SelectedLanguage, SelectedTeamFifaCode);
                _fileRepo.AppendResolution(SelectedResolution);

                SettingsApplied = true;

                Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is SettingsWindow)?.Close();
            }
            else
            {
                MessageBox.Show("Please select all options before applying settings.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // metoda za gašenje aplikacije uz korisničku potvrdu
        private void ConfirmExit()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to exit?", "Confirm Exit", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();  
            }
        }

        // metoda koja se poziva prilikom okidanja property changed eventa
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // pomoćna metoda za provjeru jesu li odabrane sve potrebne postavke u settings windowu
        private bool IsSettingsValid()
        {
            return !string.IsNullOrEmpty(SelectedWorldCup) && !string.IsNullOrEmpty(SelectedLanguage) && !string.IsNullOrEmpty(SelectedResolution);
        }
    }
}
