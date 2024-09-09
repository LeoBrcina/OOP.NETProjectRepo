using DAO.Models;
using DAO.Repos.Implementations;
using DAO.Repos.Interfaces;
using DAO.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace FootieForms
{
    public partial class MainForm : Form
    {
        private string _selectedWorldCup;
        private string _selectedLanguage;
        private string _selectedTeamFifaCode;
        private readonly API _apiService;
        private readonly RepositoryFactory _repositoryFactory;
        private readonly TeamRepository _teamRepository;
        private readonly FileRepository _fileRepository;
        private readonly PlayerRepository _playerRepository;

        private Point _startPoint;
        private bool _isDragging;
        private System.Windows.Forms.Timer _dragTimer;

        // konstruktor koji prima sve potrebne informacije za postavljanje glavne forme uz pomoć postavki iz settings forme
        public MainForm(string selectedWorldCup, string selectedLanguage, string selectedTeamFifaCode, API apiService)
        {
            _selectedWorldCup = selectedWorldCup;
            _selectedLanguage = selectedLanguage;
            _selectedTeamFifaCode = selectedTeamFifaCode;
            _apiService = apiService;

            _repositoryFactory = new RepositoryFactory(_apiService);
            _teamRepository = _repositoryFactory.GetTeamRepository();
            _fileRepository = _repositoryFactory.GetFileRepository();
            _playerRepository = _repositoryFactory.GetPlayerRepository();

            InitializeComponent();
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            btnRankLists.Enabled = false; // gumb za rang liste je zaključan dok god se ne odabere najdraži tim u cb kontroli
            await LoadTeamsComboBox();
            LoadFavoritePlayers();
        }

        // metoda koja primjenjuje kulturu prilikom promjene jezika te loada sve potrebne resurse prema novoodabranom jeziku
        private void ApplyCulture(string culture)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(culture);

            foreach (Control control in this.Controls)
            {
                var resources = new ComponentResourceManager(typeof(MainForm));
                resources.ApplyResources(control, control.Name);
            }

            var res = new ComponentResourceManager(typeof(MainForm));
            res.ApplyResources(this, "$this");
        }

        // metoda koja učitava najdraže igrače iz filea uz rutinske provjere
        private void LoadFavoritePlayers()
        {
            try
            {
                var favoritePlayers = _fileRepository.LoadFavoritePlayers();

                if (favoritePlayers == null || favoritePlayers.Count == 0)
                    return;

                foreach (var player in favoritePlayers)
                {
                    var playerLabel = new Label
                    {
                        Text = player.ToString(),
                        AutoSize = true,
                        Tag = player,
                        Cursor = Cursors.Hand
                    };

                    playerLabel.MouseDown += PlayerLabel_MouseDown;
                    playerLabel.MouseMove += PlayerLabel_MouseMove;
                    playerLabel.Click += PlayerLabel_Click;

                    pnlFavouritePlayers.Controls.Add(playerLabel);
                }

                PositionPlayerControls(pnlFavouritePlayers);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading favorite players: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // metoda koja sprema najdraže igrače u file u json formatu
        private void SaveFavoritePlayers()
        {
            var favoritePlayers = pnlFavouritePlayers.Controls.OfType<Label>()
        .Select(label => (Player)label.Tag)
        .ToList();

            _fileRepository.SaveFavoritePlayers(favoritePlayers);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SaveFavoritePlayers();
            base.OnFormClosing(e);
        }

        // metoda koja odrađuje sve potrebne akcije prilikom klika na gumb za postavke
        private void btnSettings_Click(object sender, EventArgs e)
        {
            SaveFavoritePlayers();  

            pnlPlayerData.Controls.Clear();

            SettingsForm settingsForm = new SettingsForm(_fileRepository, _apiService);
            settingsForm.SettingsChanged += async () =>
            {
                SaveFavoritePlayers();  

                var settings = _fileRepository.GetSettings();
                _selectedWorldCup = settings[0];
                _selectedLanguage = settings[1];
                _selectedTeamFifaCode = settings.Length >= 3 ? settings[2] : "";

                _apiService.ConfigureUrls(_selectedWorldCup);

                var culture = _selectedLanguage == "Croatian" ? "hr" : "en";
                ApplyCulture(culture);

                pnlFavouritePlayers.Controls.Clear();
                pnlAllPlayers.Controls.Clear();

                await LoadTeamsComboBox(); 
                LoadFavoritePlayers();      
            };
            settingsForm.ShowDialog();
        }

        // metoda za učitavanje timova odabranog prvenstva u combobox kontrolu uz sve rutinske provjere i minimiziranje bugova
        private async Task LoadTeamsComboBox()
        {
            try
            {
                _apiService.ConfigureUrls(_selectedWorldCup);
                var teams = await _teamRepository.GetAllTeamsAsync();

                if (teams != null && teams.Count > 0)
                {
                    cbTeams.Items.Clear();

                    foreach (var team in teams)
                    {
                        cbTeams.Items.Add(team);
                    }

                    if (!string.IsNullOrEmpty(_selectedTeamFifaCode))
                    {
                        var selectedTeam = teams.FirstOrDefault(t => t.FifaCode == _selectedTeamFifaCode);
                        if (selectedTeam != null)
                        {
                            cbTeams.SelectedItem = selectedTeam;
                        }
                    }

                    btnRankLists.Enabled = cbTeams.SelectedItem != null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load teams: {ex.Message}");
            }
        }

        // metoda koja sprema fifa kod najdražeg tima u file prilikom mijenjanja tima u cb kontroli
        private async void cbTeams_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlFavouritePlayers.Controls.Clear();

            var selectedTeam = (Team)cbTeams.SelectedItem;
            _fileRepository.SaveSettings(_selectedWorldCup, _selectedLanguage, selectedTeam.FifaCode);

            pnlFavouritePlayers.Refresh();
            await LoadPlayersForFavoriteTeam();

            btnRankLists.Enabled = cbTeams.SelectedItem != null;
        }

        // metoda koja dohvaća starting eleven + substitutes za odabrani tim uz pomoć repozitorija i api servisa
        private async Task LoadPlayersForFavoriteTeam()
        {
            try
            {
                pnlAllPlayers.Controls.Clear();

                var settings = _fileRepository.GetSettings();
                string worldCupSelection = settings[0];
                string selectedTeamFifaCode = cbTeams.SelectedItem != null ? ((Team)cbTeams.SelectedItem).FifaCode : settings[2];

                _apiService.ConfigureUrls(worldCupSelection);

                var players = await _playerRepository.GetPlayersByFifaCodeAsync(selectedTeamFifaCode, worldCupSelection);

                foreach (var player in players)
                {
                    var playerLabel = new Label
                    {
                        Text = player.ToString(),
                        AutoSize = true,
                        Tag = player,
                        Cursor = Cursors.Hand
                    };

                    playerLabel.MouseDown += PlayerLabel_MouseDown;
                    playerLabel.MouseMove += PlayerLabel_MouseMove;
                    playerLabel.Click += PlayerLabel_Click;

                    pnlAllPlayers.Controls.Add(playerLabel);
                }

                PositionPlayerControls(pnlAllPlayers);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load players: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // pomoćna metoda za razlikovanje klika i dragndrop funkcije na labelima igrača u panel kontroli
        private void PlayerLabel_MouseDown(object sender, MouseEventArgs e)
        {
            var playerLabel = sender as Label;
            if (playerLabel != null)
            {
                if (Control.ModifierKeys == Keys.Control)
                {
                    playerLabel.BackColor = playerLabel.BackColor == SystemColors.Highlight ? SystemColors.Control : SystemColors.Highlight;
                }
                else
                {
                    ClearSelection(); 
                    playerLabel.BackColor = SystemColors.Highlight;
                }

                _startPoint = e.Location;
                _isDragging = false;

                _dragTimer = new System.Windows.Forms.Timer();
                _dragTimer.Interval = 200; 
                _dragTimer.Tick += (s, args) =>
                {
                    _isDragging = true;
                    _dragTimer.Stop(); 
                };
                _dragTimer.Start();
            }
        }

        // pomoćna metoda za čišćenje forme
        private void ClearSelection()
        {
            foreach (Control control in pnlAllPlayers.Controls)
            {
                if (control is Label)
                {
                    control.BackColor = SystemColors.Control;
                }
            }

            foreach (Control control in pnlFavouritePlayers.Controls)
            {
                if (control is Label)
                {
                    control.BackColor = SystemColors.Control;
                }
            }
        }

        // metoda za dohvaćanje selectanih labela u panelu
        private List<Label> GetSelectedLabels(Panel panel)
        {
            var selectedLabels = new List<Label>();
            foreach (Control control in panel.Controls)
            {
                if (control is Label label && label.BackColor == SystemColors.Highlight)
                {
                    selectedLabels.Add(label);
                }
            }
            return selectedLabels;
        }

        // pomoćna metoda za dragndrop funkcije
        private void PlayerLabel_MouseMove(object sender, MouseEventArgs e)
        {
            var playerLabel = sender as Label;
            if (playerLabel != null && e.Button == MouseButtons.Left && _isDragging)
            {
                var parentPanel = playerLabel.Parent as Panel;
                if (parentPanel != null)
                {
                    var selectedLabels = GetSelectedLabels(parentPanel);
                    if (selectedLabels.Count > 0)
                    {
                        DoDragDrop(new DataObject(typeof(List<Label>).FullName, selectedLabels), DragDropEffects.Move);
                    }
                }
            }
        }

        // metoda koja učitava karticu igrača prilikom klika na odgovarajući label
        private void PlayerLabel_Click(object sender, EventArgs e)
        {
            if (!_isDragging) 
            {
                if (pnlPlayerData.Controls.Count > 0)
                {
                    var oldControl = pnlPlayerData.Controls[0];
                    oldControl.Dispose();
                }

                pnlPlayerData.Controls.Clear();

                var selectedPlayer = (Player)((Label)sender).Tag;

                var isFavorite = pnlFavouritePlayers.Controls.Contains((Label)sender);

                var playerDataControl = new PlayerData(selectedPlayer, isFavorite, _fileRepository)
                {
                    Dock = DockStyle.Fill 
                };

                pnlPlayerData.Controls.Add(playerDataControl);
            }
        }

        // pomoćna dragndrop metoda
        private void pnlAllPlayers_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(List<Label>).FullName))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        // pomoćna dragndrop metoda
        private void pnlAllPlayers_DragDrop(object sender, DragEventArgs e)
        {
            var selectedLabels = e.Data.GetData(typeof(List<Label>).FullName) as List<Label>;
            if (selectedLabels != null)
            {
                foreach (var label in selectedLabels)
                {
                    pnlFavouritePlayers.Controls.Remove(label);
                    pnlAllPlayers.Controls.Add(label);
                    label.BackColor = SystemColors.Control; 
                }
                PositionPlayerControls(pnlAllPlayers);
                PositionPlayerControls(pnlFavouritePlayers);
            }
        }

        // pomoćna dragndrop metoda
        private void pnlFavouritePlayers_DragDrop(object sender, DragEventArgs e)
        {
            var selectedLabels = e.Data.GetData(typeof(List<Label>).FullName) as List<Label>;
            if (selectedLabels != null)
            {
                if (pnlFavouritePlayers.Controls.Count + selectedLabels.Count > 3)
                {
                    MessageBox.Show("You can only select up to 3 favorite players.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                foreach (var label in selectedLabels)
                {
                    pnlAllPlayers.Controls.Remove(label);
                    pnlFavouritePlayers.Controls.Add(label);
                    label.BackColor = SystemColors.Control;
                }
                PositionPlayerControls(pnlAllPlayers);
                PositionPlayerControls(pnlFavouritePlayers);
            }
        }

        // pomoćna metoda za pozicioniranje kontrola unutar panela
        private void PositionPlayerControls(Panel panel)
        {
            int yOffset = 0;
            foreach (Control control in panel.Controls)
            {
                control.Location = new System.Drawing.Point(0, yOffset);
                yOffset += control.Height + 5; 
            }
        }

        // metoda za otvaranje rank lists forme za odabrani najdraži team
        private void btnRankLists_Click(object sender, EventArgs e)
        {
            var selectedTeam = (Team)cbTeams.SelectedItem;
            var rankListsForm = new RankListsForm(
                _playerRepository,
                _fileRepository,
                _apiService,
                selectedTeam.FifaCode, 
                _selectedWorldCup
            );
            rankListsForm.ShowDialog();
        }
    }
}