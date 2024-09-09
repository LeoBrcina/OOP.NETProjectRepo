using DAO.Models;
using DAO.Repos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Repos.Implementations
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly IMatchRepository _matchRepository;

        // player repository koji prima api servis te metode za daljnju manipulaciju podacima
        public PlayerRepository(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        // metoda za uzimanje igrača za određeni tim, starting eleven + substitutes za dobivanje svih igrača te distinct za uklanjanje zalutalih duplikata
        public async Task<List<Player>> GetPlayersByFifaCodeAsync(string fifaCode, string worldCupSelection)
        {
            // Retrieve matches involving the selected team
            var matches = await _matchRepository.GetMatchesByTeamFifaCodeAsync(fifaCode, worldCupSelection);

            var players = new List<Player>();

            foreach (var match in matches)
            {
                if (match.HomeTeam.FifaCode == fifaCode)
                {
                        players.AddRange(match.HomeTeamStatistics.StartingEleven);
                        players.AddRange(match.HomeTeamStatistics.Substitutes);
                }
                else if (match.AwayTeam.FifaCode == fifaCode)
                {
                        players.AddRange(match.AwayTeamStatistics.StartingEleven);
                        players.AddRange(match.AwayTeamStatistics.Substitutes);
                }
            }

            return players.Distinct().ToList();
        }
    }
}
