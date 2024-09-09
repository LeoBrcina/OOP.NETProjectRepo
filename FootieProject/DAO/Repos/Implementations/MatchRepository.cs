using DAO.Models;
using DAO.Repos.Interfaces;
using DAO.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Repos.Implementations
{
    public class MatchRepository : IMatchRepository
    {
        private readonly API _apiService;

        // match repository koji u konstruktor prima api servis s kojeg uzimamo podatke te ih šaljemo na daljnje manipuliranje
        public MatchRepository(API apiService)
        {
            _apiService = apiService;
        }

        // metoda za vraćanje svih matcheva
        public async Task<List<Match>> GetAllMatchesAsync()
        {
            return await _apiService.GetMatchesAsync();
        }

        // metoda za vraćanje matcheva željenog tima
        public async Task<List<Match>> GetMatchesByTeamFifaCodeAsync(string fifaCode, string worldCupType)
        {
            string url = worldCupType == "Men's World Cup 2018"
                ? $"https://worldcup-vua.nullbit.hr/men/matches/country?fifa_code={fifaCode}"
                : $"https://worldcup-vua.nullbit.hr/women/matches/country?fifa_code={fifaCode}";

            return await _apiService.GetMatchesByUrlAsync(url);
        }
    }
}
