using DAO.Models;
using DAO.Repos.Implementations;
using DAO.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace FootieForms
{
    public partial class RankListsForm : Form
    {
        private readonly PlayerRepository _playerRepository;
        private readonly MatchRepository _matchRepository;
        private readonly FileRepository _fileRepository;
        private readonly string _selectedTeamFifaCode;
        private readonly string _selectedWorldCup;

        // konstruktor za rank list formu koji prima sve potrebne informacije za pravilan prikaz rang listi odabranog teama
        public RankListsForm(PlayerRepository playerRepository, FileRepository fileRepository, API apiService, string selectedTeamFifaCode, string selectedWorldCup)
        {
            _playerRepository = playerRepository;
            _fileRepository = fileRepository;
            _matchRepository = new MatchRepository(apiService);
            _selectedTeamFifaCode = selectedTeamFifaCode;
            _selectedWorldCup = selectedWorldCup;

            InitializeComponent();
        }

        // učitavanje svih listi pri otvaranju forme
        private async void RankListsForm_Load(object sender, EventArgs e)
        {
            await LoadPlayerRankingsByGoals();
            await LoadPlayerRankingsByYellowCards();
            await LoadMatchRankingsByAttendance();
        }

        // metoda za primjenjivanje kulture na formu
        private void ApplyCulture(string culture)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(culture);

            foreach (Control control in this.Controls)
            {
                var resources = new ComponentResourceManager(typeof(RankListsForm));
                resources.ApplyResources(control, control.Name);
            }

            var res = new ComponentResourceManager(typeof(RankListsForm));
            res.ApplyResources(this, "$this");
        }

        // metoda koja učitava igrače te ih na temelju statistika provjerava i sortira prema postignutim golovima te prikazuje u panelu
        private async Task LoadPlayerRankingsByGoals()
        {
            try
            {
                var players = await SetPlayerStats();
                var sortedPlayers = players.OrderByDescending(p => p.Goals).ThenBy(p => p.Name).ToList();
                DisplayPlayersInPanel(sortedPlayers, pnlRankedGoals, "Goals");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load player rankings by goals: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // metoda koja učitava igrače te ih na temelju statistika provjerava i sortira prema žutim kartonima te prikazuje u panelu
        private async Task LoadPlayerRankingsByYellowCards()
        {
            try
            {
                var players = await SetPlayerStats();
                var sortedPlayers = players.OrderByDescending(p => p.YellowCards).ThenBy(p => p.Name).ToList();
                DisplayPlayersInPanel(sortedPlayers, pnlRankedYellow, "YellowCards");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load player rankings by yellow cards: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // pomoćna metoda za prikazivanje sortiranim igrača u panele uz sve provjere i čišćenje bugova
        private void DisplayPlayersInPanel(List<Player> players, Panel panel, string statType)
        {
            panel.Controls.Clear();

            int yOffset = 10; 
            int rank = 1; 

            foreach (var player in players)
            {
                var label = new Label
                {
                    Text = $"{rank}. {player.Name} - {statType}: {(statType == "Goals" ? player.Goals : player.YellowCards)}",
                    AutoSize = true, 
                    Location = new System.Drawing.Point(10, yOffset), 
                    Tag = player,
                    Cursor = Cursors.Hand 
                };

                label.Click += PlayerLabel_Click;

                panel.Controls.Add(label);

                yOffset += 25;
                rank++; 
            }
        }

        // metoda koja prikazuje karticu odabranog igrača prilikom klika na njegove statistike i ime
        private void PlayerLabel_Click(object? sender, EventArgs e)
        {
            var label = sender as Label;
            if (label != null && label.Tag is Player player)
            {
                DisplayPlayerDetails(player);
            }
        }

        // pomoćna metoda za prikazivanje igračeve kartice
        private void DisplayPlayerDetails(Player player)
        {
            pnlPlayerData.Controls.Clear();
            var playerDataControl = new PlayerData(player, false, _fileRepository)
            {
                Dock = DockStyle.Fill
            };
            pnlPlayerData.Controls.Add(playerDataControl);
        }

        // pomoćna metoda za dohvaćanje matcheva za određeni team
        private async Task<List<Match>> GetMatchesForTeam()
        {
            try
            {
                return await _matchRepository.GetMatchesByTeamFifaCodeAsync(_selectedTeamFifaCode, _selectedWorldCup);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to retrieve matches for the team: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Match>();
            }
        }

        // pomoćna metoda za postavljanje statistika na odgovarajuće igrače za lakše sortiranje
        private async Task<List<Player>> SetPlayerStats()
        {
            var matches = await GetMatchesForTeam();
            var players = await _playerRepository.GetPlayersByFifaCodeAsync(_selectedTeamFifaCode, _selectedWorldCup);

            foreach (var player in players)
            {
                player.Goals = 0;
                player.YellowCards = 0;
            }

            foreach (var match in matches)
            {
                bool isHomeTeam = match.HomeTeam.FifaCode == _selectedTeamFifaCode;
                var relevantEvents = isHomeTeam ? match.HomeTeamEvents : match.AwayTeamEvents;

                if (relevantEvents == null) continue;

                foreach (var teamEvent in relevantEvents)
                {
                    var player = players.FirstOrDefault(p => p.Name == teamEvent.Player);
                    if (player != null)
                    {
                        switch (teamEvent.TypeOfEvent)
                        {
                            case "goal":
                            case "goal-penalty":
                                player.Goals += 1;
                                break;

                            case "yellow-card":
                            case "yellow-card-second":
                                player.YellowCards += 1;
                                break;
                        }
                    }
                }
            }
            return players;
        }

        // metoda za učitavanje matcheva i rangiranje po ukupnoj dolaznosti
        private async Task LoadMatchRankingsByAttendance()
        {
            try
            {
                var matches = await GetMatchesForTeam();

                var sortedMatches = matches.OrderByDescending(m => m.Attendance).ToList();

                DisplayMatchesInPanel(sortedMatches);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load match rankings by attendance: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // pomoćna metoda za prikazivanje rangiranih matcheva u paneluuz rutinske provjere i čišćenje bugova
        private void DisplayMatchesInPanel(List<Match> matches)
        {
            pnlRankedMatches.Controls.Clear();

            int yOffset = 10;
            int rank = 1;

            foreach (var match in matches)
            {
                var label = new Label
                {
                    Text = $"{rank}. {match.Location} - {match.HomeTeamCountry} vs {match.AwayTeamCountry} - Attendance: {match.Attendance}",
                    AutoSize = true, 
                    Location = new System.Drawing.Point(10, yOffset), 
                    Tag = match, 
                    Cursor = Cursors.Hand 
                };

                pnlRankedMatches.Controls.Add(label);

                yOffset += 25;
                rank++; 
            }
        }

        // metoda koja omogućava printanje rang listi u pdf formatu pritiskom na gumb
        public void PrintPDF()
        {
            string filePath = "rankLists.pdf";

            iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4);

            try
            {
                PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

                document.Open();

                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                var title = new iTextSharp.text.Paragraph("Rank Lists", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);
                document.Add(new iTextSharp.text.Paragraph("\n"));

                AddPanelContentToPdf(document, pnlRankedGoals, "Player Ranking by Goals");

                AddPanelContentToPdf(document, pnlRankedYellow, "Player Ranking by Yellow Cards");

                AddPanelContentToPdf(document, pnlRankedMatches, "Match Ranking by Attendance");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating PDF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                document.Close();
            }

            MessageBox.Show("PDF generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // pomoćna metoda za postavljanje rang listi u pdf dokument
        private void AddPanelContentToPdf(iTextSharp.text.Document document, Panel panel, string sectionTitle)
        {
            // Add a section title
            var sectionTitleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
            var sectionParagraph = new iTextSharp.text.Paragraph(sectionTitle, sectionTitleFont);
            sectionParagraph.SpacingBefore = 10f;
            sectionParagraph.SpacingAfter = 10f;
            document.Add(sectionParagraph);

            foreach (Control control in panel.Controls)
            {
                if (control is Label label)
                {
                    var contentFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
                    var contentParagraph = new iTextSharp.text.Paragraph(label.Text, contentFont);
                    document.Add(contentParagraph);
                }
            }

            document.Add(new iTextSharp.text.Paragraph("\n"));
        }

        private void btnPrintPdf_Click(object sender, EventArgs e)
        {
            PrintPDF();
        }
    }
}
