using DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Repos.Interfaces
{
    public interface IMatchRepository
    {
        // interface za match repository za dohvaćanje podataka
        Task<List<Match>> GetAllMatchesAsync();
        Task<List<Match>> GetMatchesByTeamFifaCodeAsync(string fifaCode, string worldCupType);
    }
}
