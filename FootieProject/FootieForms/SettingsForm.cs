using DAO.Repos.Implementations;
using DAO.Services;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace FootieForms
{
    public partial class SettingsForm : Form
    {
        public event Action SettingsChanged;

        private readonly FileRepository _fileRepo;
        private readonly API _apiService;

        // konstruktor za settings formu koji prima potrebni file repository te api servis za prosljeðivanje u glavnu formu
        public SettingsForm(FileRepository fileRepo, API apiService)
        {
            _fileRepo = fileRepo;
            _apiService = apiService;
            InitializeComponent();
        }

        // uèitavanje potrebnih fileova za postavke prilikom podizanja forme
        private void SettingsForm_Load(object sender, EventArgs e)
        {
            if (_fileRepo.SettingsExist())
            {
                var settings = _fileRepo.GetSettings();
                if (settings.Length >= 2)
                {
                    cbWorldCup.SelectedItem = settings[0];
                    cbLanguage.SelectedItem = settings[1];
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // metoda za svu potrebnu logiku spremanja postavki i kulture prilikom pritiska gumba za potvrdu
        private void btnApply_Click(object sender, EventArgs e)
        {
            if (cbWorldCup.SelectedItem == null || cbLanguage.SelectedItem == null)
            {
                MessageBox.Show("Please select both a World Cup and a Language.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedWorldCup = cbWorldCup.SelectedItem.ToString();
            var selectedLanguage = cbLanguage.SelectedItem.ToString();
            var culture = selectedLanguage == "Croatian" ? "hr" : "en";
            var selectedTeamFifaCode = _fileRepo.GetSettings().Length >= 3 ? _fileRepo.GetSettings()[2] : "";

            _fileRepo.SaveSettings(selectedWorldCup, selectedLanguage, selectedTeamFifaCode);

            ApplyCulture(culture);

            SettingsChanged?.Invoke();  

            this.Close();
        }

        // pomoæna metoda za primjenjivanje kulture na bilo koju potrebnu formu
        private void ApplyCulture(string culture)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(culture);

            foreach (Control control in this.Controls)
            {
                var resources = new ComponentResourceManager(typeof(SettingsForm));
                resources.ApplyResources(control, control.Name);
            }

            var res = new ComponentResourceManager(typeof(SettingsForm));
            res.ApplyResources(this, "$this");
        }

        // metoda koja omoguæava shortcutove za enter i escape gumbe na tipkovnici
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                btnApply.PerformClick();
                return true;
            }

            if (keyData == Keys.Escape)
            {
                btnExit.PerformClick();
                return true; 
            }

            return base.ProcessCmdKey(ref msg, keyData); 
        }

        // metoda za zatvaranje forme koja traži potvrdu
        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to exit?",
                "Confirm Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }
    }
}
