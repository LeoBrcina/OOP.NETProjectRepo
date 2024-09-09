using DAO.Models;

namespace FootieWPF.ViewModels
{
    public class TeamStatsViewModel
    {
        public string Country { get; }
        public string GroupLetter { get; }
        public long GamesPlayed { get; }
        public long Wins { get; }
        public long Draws { get; }
        public long Losses { get; }
        public long GoalsFor { get; }
        public long GoalsAgainst { get; }
        public long GoalDifferential { get; }
        public long Points { get; }

        // konstruktor za team stats view model koji prima sve potrebne podatke iz rezultata za daljnje slanje na UI
        public TeamStatsViewModel(Result result)
        {
            Country = result.Country;
            GroupLetter = result.GroupLetter;
            GamesPlayed = result.GamesPlayed;
            Wins = result.Wins;
            Draws = result.Draws;
            Losses = result.Losses;
            GoalsFor = result.GoalsFor;
            GoalsAgainst = result.GoalsAgainst;
            GoalDifferential = result.GoalDifferential;
            Points = result.Points;
        }
    }
}
