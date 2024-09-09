using DAO.Models;
using DAO.Repos.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Repos.Implementations
{
    public class FileRepository : IFileRepository
    {
        private readonly string APP_DIRECTORY = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FootieProject");
        private readonly string PATH;
        private readonly string FAVORITES_PATH;
        private readonly string IMAGES_PATH;

        // provjeravamo postoji li naše mjesto za spremanje svih fileova te postavljamo pathove za sve fileove koje će obje aplikacije koristiti
        public FileRepository()
        {
            if (!Directory.Exists(APP_DIRECTORY))
            {
                Directory.CreateDirectory(APP_DIRECTORY);
            }

            PATH = Path.Combine(APP_DIRECTORY, "settings.txt");
            FAVORITES_PATH = Path.Combine(APP_DIRECTORY, "favorites.txt");
            IMAGES_PATH = Path.Combine(APP_DIRECTORY, "playerImages.txt");
        }

        // spremamo sve podatke koje će koristiti forms i wpf aplikacije u njihove određene linije uz sve potrebne promjene
        public void SaveSettings(string worldCupSelection, string language, string selectedTeamFifaCode)
        {
            var lines = File.Exists(PATH) ? File.ReadAllLines(PATH).ToList() : new List<string>();

            if (lines.Count < 3)
            {
                while (lines.Count < 3)
                {
                    lines.Add(string.Empty);
                }
            }

            lines[0] = worldCupSelection;
            lines[1] = language;
            lines[2] = selectedTeamFifaCode;

            File.WriteAllLines(PATH, lines);
        }

        // metoda za vraćanje postavki po potrebi
        public string[] GetSettings()
        {
            if (!SettingsExist())
                return Array.Empty<string>();

            return File.ReadAllLines(PATH); 
        }

        // provjera postoje li postavke
        public bool SettingsExist()
        {
            return File.Exists(PATH);
        }

        // spremanje omiljenih igrača u file u json formatu
        public void SaveFavoritePlayers(List<Player> favoritePlayers)
        {
            var json = JsonConvert.SerializeObject(favoritePlayers, Formatting.Indented);
            File.WriteAllText(FAVORITES_PATH, json);
        }

        // metoda za loadanje igrača nazad u panel po potrebi te deserijalizacija
        public List<Player> LoadFavoritePlayers()
        {
            if (!FavoritesExist())
                return new List<Player>();

            var json = File.ReadAllText(FAVORITES_PATH);
            return JsonConvert.DeserializeObject<List<Player>>(json);
        }

        // provjera postoje li omiljeni igrači
        public bool FavoritesExist()
        {
            return File.Exists(FAVORITES_PATH);
        }

        // metoda za spremanje slika po igraču u formatu ime|putanja te ostale potrebne provjere
        public void SavePlayerImagePath(string playerName, string imagePath)
        {
            var lines = File.Exists(IMAGES_PATH) ? File.ReadAllLines(IMAGES_PATH).ToList() : new List<string>();
            var lineIndex = lines.FindIndex(line => line.StartsWith(playerName + "|"));

            if (lineIndex >= 0)
            {
                lines[lineIndex] = $"{playerName}|{imagePath}";
            }
            else
            {
                lines.Add($"{playerName}|{imagePath}");
            }

            File.WriteAllLines(IMAGES_PATH, lines);
        }

        // metoda za loadanje slike igrača u njihovu karticu uz potrebne provjere
        public string LoadPlayerImagePath(string playerName)
        {
            if (!File.Exists(IMAGES_PATH))
                return string.Empty;

            var lines = File.ReadAllLines(IMAGES_PATH);
            var line = lines.FirstOrDefault(l => l.StartsWith(playerName + "|"));

            return line?.Split('|')[1] ?? string.Empty;
        }

        // metoda za dodavanje rezolucije na kraj settings.txt filea, linija broj 4 koju koristi samo wpf aplikacija uz sve rutinske provjere
        public void AppendResolution(string resolution)
        {
            var lines = File.Exists(PATH) ? File.ReadAllLines(PATH).ToList() : new List<string>();

            if (lines.Count >= 4)
            {
                lines[3] = resolution; 
            }
            else
            {
                while (lines.Count < 3)
                {
                    lines.Add(string.Empty);  
                }
                lines.Add(resolution);  
            }

            File.WriteAllLines(PATH, lines);
        }

        // metoda za vraćanje rezolucije uz provjere
        public string GetResolution()
        {
            if (SettingsExist())
            {
                var lines = File.ReadAllLines(PATH);
                if (lines.Length == 4)
                {
                    return lines[3]; 
                }
            }
            return null; 
        }
    }
}

