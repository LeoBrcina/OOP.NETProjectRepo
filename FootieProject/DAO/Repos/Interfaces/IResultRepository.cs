using DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Repos.Interfaces
{
    public interface IResultRepository
    {
        // interface za result repository za dohvaćanje podataka
        Task<List<Result>> GetAllResultsAsync();
        Task<Result> GetResultByIdAsync(int id);
    }
}
