using DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Repos.Interfaces
{
    public interface IPlayerRepository
    {
        // interface za player repository za dohvaćanje podataka
        Task<List<Player>> GetPlayersByFifaCodeAsync(string fifaCode, string worldCupType);
    }
}
