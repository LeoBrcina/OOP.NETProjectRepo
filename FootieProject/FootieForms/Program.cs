using DAO.Repos.Implementations;
using DAO.Repos.Interfaces;
using DAO.Services;

namespace FootieForms
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FileRepository fileRepo = new FileRepository();
            API apiService = API.Instance;

            // provjera postoji li settings file prema kojemu odreðujemo koju æemo formu prvu pokrenuti
            if (!fileRepo.SettingsExist())
            {
                using (var settingsForm = new SettingsForm(fileRepo, apiService))
                {
                    settingsForm.ShowDialog();

                    if (fileRepo.SettingsExist())  
                    {
                        ApplySettings(fileRepo, apiService);
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                ApplySettings(fileRepo, apiService);
            }
        }

        // metoda za primjenu odabranih postavki na aplikaciju te pokretanje glavne forme u skladu s istima
        static void ApplySettings(FileRepository fileRepo, API apiService)
        {
            var settings = fileRepo.GetSettings();

            string selectedWorldCup = settings.Length >= 1 ? settings[0] : "Men's World Cup 2018";
            string selectedLanguage = settings.Length >= 2 ? settings[1] : "English";
            string selectedTeamFifaCode = settings.Length >= 3 ? settings[2] : "";

            string culture = selectedLanguage == "Croatian" ? "hr" : "en";
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(culture);

            apiService.ConfigureUrls(selectedWorldCup);

            Application.Run(new MainForm(selectedWorldCup, selectedLanguage, selectedTeamFifaCode, apiService));
        }
    }
}