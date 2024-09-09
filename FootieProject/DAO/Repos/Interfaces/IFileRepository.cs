using DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Repos.Interfaces
{
    public interface IFileRepository
    {
        // Interface za spremanje podataka o jeziku, svjetskom prvenstvu i odabranoj reprezentaciji i ucitavanje istih
        void SaveSettings(string worldCupSelection, string language, string selectedTeamFifaCode);
        string[] GetSettings();
        bool SettingsExist();

        // Interface za odvojeno spremanje podataka o odabranim najdrazim igracima u panelu i ucitavanje istih
        void SaveFavoritePlayers(List<Player> favoritePlayerNames);
        List<Player> LoadFavoritePlayers();
        bool FavoritesExist();

        // Interface za spremanje dictionarya s parovima ime igrača|putanja slike

        void SavePlayerImagePath(string playerName, string imagePath); 
        string LoadPlayerImagePath(string playerName);

        // metode specifične za manipulaciju rezolucijom u wpf aplikaciji
        void AppendResolution(string resolution);
        string GetResolution();
    }
}
