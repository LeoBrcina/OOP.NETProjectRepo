using DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Repos.Interfaces
{
    public interface ITeamRepository
    {
        // interface za team repository za dohvaćanje podataka
        Task<List<Team>> GetAllTeamsAsync();
        Task<Team> GetTeamByFifaCodeAsync(string fifaCode, string worldCupSelection);
    }
}
