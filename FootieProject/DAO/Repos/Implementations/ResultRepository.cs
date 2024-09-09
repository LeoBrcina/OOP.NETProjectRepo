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
    public class ResultRepository : IResultRepository
    {
        private readonly API _apiService;

        // result repository koji prima api servis za dohvaćanje potrebnih rezultata
        public ResultRepository(API apiService)
        {
            _apiService = apiService;
        }

        // metoda za dohvaćanje svih rezultata
        public async Task<List<Result>> GetAllResultsAsync()
        {
            return await _apiService.GetResultsAsync();
        }

        // metoda za dohvaćanje rezultata po id-u
        public async Task<Result> GetResultByIdAsync(int id)
        {
            var results = await GetAllResultsAsync();
            return results.Find(result => result.Id == id);
        }
    }
}
