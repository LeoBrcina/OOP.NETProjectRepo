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
    public class TeamRepository : ITeamRepository
    {
        private readonly API _apiService;

        // team repository koji prima api servis te omogućuje dohvaćanje timova
        public TeamRepository(API apiService)
        {
            _apiService = apiService;
        }

        // metoda za dohvaćanje svih timova
        public async Task<List<Team>> GetAllTeamsAsync()
        {
            return await _apiService.GetTeamsAsync();
        }

        // metoda za dohvaćanje timova po fifa kodu uz dodatak na url
        public async Task<Team> GetTeamByFifaCodeAsync(string fifaCode, string worldCupSelection)
        {
            string url = worldCupSelection == "Men"
                ? "http://worldcup-vua.nullbit.hr/men/teams"
                : "http://worldcup-vua.nullbit.hr/women/teams";

            var teams = await GetAllTeamsAsync();
            return teams.FirstOrDefault(team => team.FifaCode == fifaCode);
        }
    }
}

