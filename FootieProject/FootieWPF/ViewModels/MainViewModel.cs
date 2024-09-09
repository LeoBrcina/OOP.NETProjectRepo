using DAO.Models;
using DAO.Repos.Implementations;
using DAO.Services;
using FootieWPF.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FootieWPF.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly TeamRepository _teamRepository;
        private readonly MatchRepository _matchRepository;
        private readonly ResultRepository _resultRepository;
        public readonly FileRepository _fileRepository;
        private readonly RepositoryFactory _repositoryFactory;
        private readonly API _apiService;

        private string _selectedWorldCup;
        private string _selectedLanguage;
        private string _selectedTeamFifaCode;
        private Team _selectedFavoriteTeam;
        private MatchTeam _selectedOpponentTeam;
        private string _matchScore;

        public Player HomeGK { get; set; }
        public Player HomeDF1 { get; set; }
        public Player HomeDF2 { get; set; }
        public Player HomeDF3 { get; set; }
        public Player HomeDF4 { get; set; }
        public Player HomeDF5 { get; set; }
        public Player HomeMF1 { get; set; }
        public Player HomeMF2 { get; set; }
        public Player HomeMF3 { get; set; }
        public Player HomeMF4 { get; set; }
        public Player HomeMF5 { get; set; }
        public Player HomeFW1 { get; set; }
        public Player HomeFW2 { get; set; }
        public Player HomeFW3 { get; set; }
        public Player HomeFW4 { get; set; }
        public Player HomeFW5 { get; set; }

        public Player AwayGK { get; set; }
        public Player AwayDF1 { get; set; }
        public Player AwayDF2 { get; set; }
        public Player AwayDF3 { get; set; }
        public Player AwayDF4 { get; set; }
        public Player AwayDF5 { get; set; }
        public Player AwayMF1 { get; set; }
        public Player AwayMF2 { get; set; }
        public Player AwayMF3 { get; set; }
        public Player AwayMF4 { get; set; }
        public Player AwayMF5 { get; set; }
        public Player AwayFW1 { get; set; }
        public Player AwayFW2 { get; set; }
        public Player AwayFW3 { get; set; }
        public Player AwayFW4 { get; set; }
        public Player AwayFW5 { get; set; }

        public ObservableCollection<Team> FavoriteTeams { get; set; } = new ObservableCollection<Team>();
        public ObservableCollection<MatchTeam> OpponentTeams { get; set; } = new ObservableCollection<MatchTeam>();
        public ObservableCollection<Match> OpponentMatches { get; set; } = new ObservableCollection<Match>();

        public event PropertyChangedEventHandler PropertyChanged;

        // komanda za otvaranje prozora postavki
        public ICommand OpenSettingsCommand { get; }

        // konstruktor za main view model koji će se baviti manipulacijom podataka i prosljeđivanjem istih u main xaml cs za UI prikaz
        public MainViewModel(string selectedWorldCup, string selectedLanguage, string selectedTeamFifaCode, API apiService)
        {
            _selectedWorldCup = selectedWorldCup;
            _selectedLanguage = selectedLanguage;
            _selectedTeamFifaCode = selectedTeamFifaCode;
            _apiService = apiService;

            _repositoryFactory = new RepositoryFactory(apiService);
            _teamRepository = _repositoryFactory.GetTeamRepository();
            _fileRepository = _repositoryFactory.GetFileRepository();
            _matchRepository = _repositoryFactory.GetMatchRepository();
            _resultRepository = _repositoryFactory.GetResultRepository();

            LoadFavoriteTeam();

            OpenSettingsCommand = new RelayCommand<object>(param => OpenSettingsWindow());

            ShowPlayerCardCommand = new RelayCommand<Player>(ShowPlayerCard);
        }

        // metoda za otvaranje samog settings prozora
        private void OpenSettingsWindow()
        {
            FileRepository fileRepo = new FileRepository();
            API apiService = API.Instance;

            SettingsViewModel settingsViewModel = new SettingsViewModel(fileRepo, apiService);
            var settingsWindow = new SettingsWindow { DataContext = settingsViewModel };

            bool? result = settingsWindow.ShowDialog();

            if (settingsViewModel.SettingsApplied)
            {
                ApplyNewSettings(fileRepo);
            }
        }

        // pomoćna metoda za primjenjivanje novopostavljenih postavki na glavni prozor
        private void ApplyNewSettings(FileRepository fileRepo)
        {
            var settings = fileRepo.GetSettings();
            string selectedWorldCup = settings.Length >= 1 ? settings[0] : "Men's World Cup 2018";
            string selectedLanguage = settings.Length >= 2 ? settings[1] : "English";
            string selectedTeamFifaCode = settings.Length >= 3 ? settings[2] : "";

            _selectedWorldCup = selectedWorldCup;
            _selectedLanguage = selectedLanguage;
            _selectedTeamFifaCode = selectedTeamFifaCode;

            _apiService.ConfigureUrls(_selectedWorldCup);
            LoadFavoriteTeam();

            string selectedResolution = fileRepo.GetResolution() ?? "Fullscreen";
            ApplyWindowResolution(Application.Current.MainWindow as MainWindow, selectedResolution);
        }

        // metoda za primjenjivanje rezolucije na glavni prozor
        private void ApplyWindowResolution(MainWindow mainWindow, string selectedResolution)
        {
            if (selectedResolution == "Fullscreen")
            {
                mainWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                var dimensions = selectedResolution.Split('x');
                if (dimensions.Length == 2 && int.TryParse(dimensions[0], out int width) && int.TryParse(dimensions[1], out int height))
                {
                    mainWindow.Width = width;
                    mainWindow.Height = height;
                }
            }
        }

        public string SelectedWorldCup
        {
            get => _selectedWorldCup;
            set
            {
                _selectedWorldCup = value;
                OnPropertyChanged();
                LoadFavoriteTeam();
            }
        }

        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                _selectedLanguage = value;
                OnPropertyChanged();
            }
        }

        public string SelectedTeamFifaCode
        {
            get => _selectedTeamFifaCode;
            set
            {
                _selectedTeamFifaCode = value;
                OnPropertyChanged();
            }
        }

        // metoda za učitavanje najdražeg tima i postavljanje istog u combobox kontrolu 
        public async void LoadFavoriteTeam()
        {
            var teams = await _teamRepository.GetAllTeamsAsync();

            FavoriteTeams.Clear();
            foreach (var team in teams)
            {
                FavoriteTeams.Add(team);

                if (!string.IsNullOrEmpty(_selectedTeamFifaCode) && team.FifaCode == _selectedTeamFifaCode)
                {
                    SelectedFavoriteTeam = team;
                }
            }

            if (FavoriteTeams.Count == 0)
            {
                SelectedFavoriteTeam = null;
            }
        }

        public Team SelectedFavoriteTeam
        {
            get => _selectedFavoriteTeam;
            set
            {
                _selectedFavoriteTeam = value;
                OnPropertyChanged();
                MatchScore = "";
                if (_selectedFavoriteTeam != null)
                {
                    SaveFavoriteTeamFifaCode();
                }

                LoadOpponentTeams();
            }
        }

        // metoda za učitavanje timova koji su na odabranom prvenstvu igrali utakmicu sa prije odabranim najdražim timom uz potrebne provjere
        private async void LoadOpponentTeams()
        {
            if (SelectedFavoriteTeam == null) return;

            var matches = await _matchRepository.GetMatchesByTeamFifaCodeAsync(SelectedFavoriteTeam.FifaCode, _selectedWorldCup);

            OpponentTeams.Clear();
            OpponentMatches.Clear();

            foreach (var match in matches)
            {
                OpponentMatches.Add(match);

                if (match.HomeTeam.FifaCode != SelectedFavoriteTeam.FifaCode)
                {
                    OpponentTeams.Add(match.HomeTeam);  
                }
                else if (match.AwayTeam.FifaCode != SelectedFavoriteTeam.FifaCode)
                {
                    OpponentTeams.Add(match.AwayTeam); 
                }
            }
        }

        public MatchTeam SelectedOpponentTeam
        {
            get => _selectedOpponentTeam;
            set
            {
                _selectedOpponentTeam = value;
                OnPropertyChanged();

                if (_selectedOpponentTeam != null)
                {
                    var matches = OpponentMatches
                        .Where(m => m.HomeTeam.FifaCode == _selectedOpponentTeam.FifaCode || m.AwayTeam.FifaCode == _selectedOpponentTeam.FifaCode)
                        .ToList();

                    SelectedOpponentMatch = matches.FirstOrDefault();
                }

                LoadPlayersForMatch();
                LoadMatchScore();
            }
        }
        private Match _selectedOpponentMatch;
        public Match SelectedOpponentMatch
        {
            get => _selectedOpponentMatch;
            private set
            {
                _selectedOpponentMatch = value;
                OnPropertyChanged();
            }
        }

        // metoda za učitavanje rezultata iz odabrane utakmice u label između comboboxova
        private async void LoadMatchScore()
        {
            if (SelectedFavoriteTeam == null || SelectedOpponentTeam == null) return;

            var matches = await _matchRepository.GetMatchesByTeamFifaCodeAsync(SelectedFavoriteTeam.FifaCode, _selectedWorldCup);
            var match = matches.FirstOrDefault(m =>
                (m.HomeTeam.FifaCode == SelectedFavoriteTeam.FifaCode && m.AwayTeam.FifaCode == SelectedOpponentTeam.FifaCode) ||
                (m.AwayTeam.FifaCode == SelectedFavoriteTeam.FifaCode && m.HomeTeam.FifaCode == SelectedOpponentTeam.FifaCode));

            if (match != null)
            {
                MatchScore = $"{match.HomeTeamCountry} {match.HomeTeam.Goals} : {match.AwayTeam.Goals} {match.AwayTeamCountry}";
                LoadPlayers(match);
            }
            else
            {
                MatchScore = "No match found.";
            }
        }

        // metoda za učitavanje igrača koji su igrali odabranu utakmicu
        private async void LoadPlayersForMatch()
        {
            if (SelectedFavoriteTeam == null || SelectedOpponentTeam == null) return;

            var matches = await _matchRepository.GetMatchesByTeamFifaCodeAsync(SelectedFavoriteTeam.FifaCode, _selectedWorldCup);
            var match = matches.FirstOrDefault(m =>
                (m.HomeTeam.FifaCode == SelectedFavoriteTeam.FifaCode && m.AwayTeam.FifaCode == SelectedOpponentTeam.FifaCode) ||
                (m.AwayTeam.FifaCode == SelectedFavoriteTeam.FifaCode && m.HomeTeam.FifaCode == SelectedOpponentTeam.FifaCode));

            if (match != null)
            {
                LoadPlayers(match);
            }
        }

        // metoda koja uz rutinske provjere čisti travnjak te postavlja igrače iz utakmice na njihove odgovarajuće pozicije
        private void LoadPlayers(Match match)
        {
            ClearPlayerFields();

            var homePlayers = match.HomeTeamStatistics.StartingEleven.ToList();
            AssignPlayersToPositions(homePlayers, isHomeTeam: true);

            var awayPlayers = match.AwayTeamStatistics.StartingEleven.ToList();
            AssignPlayersToPositions(awayPlayers, isHomeTeam: false);

            OnPropertyChanged(nameof(HomeGK));
            OnPropertyChanged(nameof(HomeDF1));
            OnPropertyChanged(nameof(HomeDF2));
            OnPropertyChanged(nameof(HomeDF3));
            OnPropertyChanged(nameof(HomeDF4));
            OnPropertyChanged(nameof(HomeDF5));
            OnPropertyChanged(nameof(HomeMF1));
            OnPropertyChanged(nameof(HomeMF2));
            OnPropertyChanged(nameof(HomeMF3));
            OnPropertyChanged(nameof(HomeMF4));
            OnPropertyChanged(nameof(HomeMF5));
            OnPropertyChanged(nameof(HomeFW1));
            OnPropertyChanged(nameof(HomeFW2));
            OnPropertyChanged(nameof(HomeFW3));
            OnPropertyChanged(nameof(HomeFW4));
            OnPropertyChanged(nameof(HomeFW5));

            OnPropertyChanged(nameof(AwayGK));
            OnPropertyChanged(nameof(AwayDF1));
            OnPropertyChanged(nameof(AwayDF2));
            OnPropertyChanged(nameof(AwayDF3));
            OnPropertyChanged(nameof(AwayDF4));
            OnPropertyChanged(nameof(AwayDF5));
            OnPropertyChanged(nameof(AwayMF1));
            OnPropertyChanged(nameof(AwayMF2));
            OnPropertyChanged(nameof(AwayMF3));
            OnPropertyChanged(nameof(AwayMF4));
            OnPropertyChanged(nameof(AwayMF5));
            OnPropertyChanged(nameof(AwayFW1));
            OnPropertyChanged(nameof(AwayFW2));
            OnPropertyChanged(nameof(AwayFW3));
            OnPropertyChanged(nameof(AwayFW4));
            OnPropertyChanged(nameof(AwayFW5));
        }

        // pomoćna metoda koja postavlja sve igrače na njihove odgovarajuće pozicije za određeni odabrani match
        private void AssignPlayersToPositions(List<Player> players, bool isHomeTeam)
        {
            int dfCount = 0, mfCount = 0, fwCount = 0;

            foreach (var player in players)
            {
                switch (player.Position)
                {
                    case "Goalie":
                        if (isHomeTeam) HomeGK = player;
                        else AwayGK = player;
                        break;
                    case "Defender":
                        if (isHomeTeam)
                        {
                            switch (dfCount)
                            {
                                case 0: HomeDF1 = player; break;
                                case 1: HomeDF2 = player; break;
                                case 2: HomeDF3 = player; break;
                                case 3: HomeDF4 = player; break;
                                case 4: HomeDF5 = player; break;
                            }
                        }
                        else
                        {
                            switch (dfCount)
                            {
                                case 0: AwayDF1 = player; break;
                                case 1: AwayDF2 = player; break;
                                case 2: AwayDF3 = player; break;
                                case 3: AwayDF4 = player; break;
                                case 4: AwayDF5 = player; break;
                            }
                        }
                        dfCount++;
                        break;
                    case "Midfield":
                        if (isHomeTeam)
                        {
                            switch (mfCount)
                            {
                                case 0: HomeMF1 = player; break;
                                case 1: HomeMF2 = player; break;
                                case 2: HomeMF3 = player; break;
                                case 3: HomeMF4 = player; break;
                                case 4: HomeMF5 = player; break;
                            }
                        }
                        else
                        {
                            switch (mfCount)
                            {
                                case 0: AwayMF1 = player; break;
                                case 1: AwayMF2 = player; break;
                                case 2: AwayMF3 = player; break;
                                case 3: AwayMF4 = player; break;
                                case 4: AwayMF5 = player; break;
                            }
                        }
                        mfCount++;
                        break;
                    case "Forward":
                        if (isHomeTeam)
                        {
                            switch (fwCount)
                            {
                                case 0: HomeFW1 = player; break;
                                case 1: HomeFW2 = player; break;
                                case 2: HomeFW3 = player; break;
                                case 3: HomeFW4 = player; break;
                                case 4: HomeFW5 = player; break;
                            }
                        }
                        else
                        {
                            switch (fwCount)
                            {
                                case 0: AwayFW1 = player; break;
                                case 1: AwayFW2 = player; break;
                                case 2: AwayFW3 = player; break;
                                case 3: AwayFW4 = player; break;
                                case 4: AwayFW5 = player; break;
                            }
                        }
                        fwCount++;
                        break;
                }
            }
        }

        // pomoćna metoda za čišćenje travnjaka i pozicija
        private void ClearPlayerFields()
        {
            HomeGK = HomeDF1 = HomeDF2 = HomeDF3 = HomeDF4 = HomeDF5 = HomeMF1 = HomeMF2 = HomeMF3 = HomeMF4 = HomeMF5 = HomeFW1 = HomeFW2 = HomeFW3 = HomeFW4 = HomeFW5 = null;
            AwayGK = AwayDF1 = AwayDF2 = AwayDF3 = AwayDF4 = AwayDF5 = AwayMF1 = AwayMF2 = AwayMF3 = AwayMF4 = AwayMF5 = AwayFW1 = AwayFW2 = AwayFW3 = AwayFW4 = AwayFW5 = null;
        }

        public string MatchScore
        {
            get => _matchScore;
            set
            {
                _matchScore = value;
                OnPropertyChanged();
            }
        }

        // metoda za spremanje najdražeg tima uz pomoć file repositorya po potrebi
        private void SaveFavoriteTeamFifaCode()
        {
            var settings = _fileRepository.GetSettings();
            if (settings.Length >= 3)
            {
                settings[2] = SelectedFavoriteTeam.FifaCode;
            }
            _fileRepository.SaveSettings(settings[0], settings[1], settings[2]);
        }

        // komande za otvaranje prozora sa opširnijim statistikama za oba tima na utakmici pritiskom na gumbe Stats
        public ICommand ShowFavoriteTeamDetailsCommand => new RelayCommand<object>(async param => await ShowTeamDetailsAsync(SelectedFavoriteTeam));
        public ICommand ShowOpponentTeamDetailsCommand => new RelayCommand<object>(async param => await ShowTeamDetailsAsync(SelectedOpponentTeam));

        // pomoćna metoda za prikazivanje i dohvaćanje statistika za pojedini tim u utakmici uz rutinske provjere i slanje u novi team stats window
        private async Task ShowTeamDetailsAsync(object team)
        {
            if (team == null) return;

            Result teamStats = null;
            if (team is Team selectedTeam)
            {
                var teamStatsList = await _resultRepository.GetAllResultsAsync();
                teamStats = teamStatsList.FirstOrDefault(t => t.FifaCode == selectedTeam.FifaCode);
            }
            else if (team is MatchTeam selectedMatchTeam)
            {
                var teamStatsList = await _resultRepository.GetAllResultsAsync();
                teamStats = teamStatsList.FirstOrDefault(t => t.FifaCode == selectedMatchTeam.FifaCode);
            }

            if (teamStats != null)
            {
                var teamStatsViewModel = new TeamStatsViewModel(teamStats);
                var statsWindow = new TeamStatsWindow(teamStatsViewModel);
                statsWindow.Show();
            }
        }

        // metoda koja se okida na svaki property changed event
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // komanda za prikazivanje prozora sa igračevom karticom i dodatnim statistikama za odabranu utakmicu
        public ICommand ShowPlayerCardCommand { get; }

        // metoda koja pronalazi i dohvaća potrebne statistike za odabranog igrača te ih prikazuje u novom player card windowu
        private void ShowPlayerCard(Player selectedPlayer)
        {
            if (selectedPlayer == null || SelectedOpponentTeam == null) return;

            // Retrieve the match related to the selected opponent
            var selectedMatch = OpponentMatches
                .FirstOrDefault(m => m.HomeTeam.FifaCode == SelectedOpponentTeam.FifaCode ||
                                     m.AwayTeam.FifaCode == SelectedOpponentTeam.FifaCode);

            if (selectedMatch == null) return;

            var playerCardViewModel = new PlayerCardViewModel(
                selectedPlayer.Name,
                selectedPlayer.ShirtNumber,
                selectedPlayer.Position,
                GetGoalsForPlayer(selectedPlayer.Name, selectedMatch),
                GetYellowCardsForPlayer(selectedPlayer.Name, selectedMatch),
                _fileRepository);

            var playerCardWindow = new PlayerCardWindow(playerCardViewModel);
            playerCardWindow.ShowDialog();
        }

        // pomoćna metoda za dohvaćanje statistike golova igrača za odabranu utakmicu
        public int GetGoalsForPlayer(string playerName, Match match)
        {
            return match.HomeTeamEvents.Concat(match.AwayTeamEvents)
                       .Count(e => e.Player == playerName && (e.TypeOfEvent == "goal" || e.TypeOfEvent == "goal-penalty"));
        }

        // pomoćna metoda za dohvaćanje statistike kartona igrača za odabranu utakmicu
        public int GetYellowCardsForPlayer(string playerName, Match match)
        {
            return match.HomeTeamEvents.Concat(match.AwayTeamEvents)
                       .Count(e => e.Player == playerName && (e.TypeOfEvent == "yellow-card" || e.TypeOfEvent == "yellow-card-second"));
        }
    }
}
