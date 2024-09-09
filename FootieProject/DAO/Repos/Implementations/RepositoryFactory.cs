using DAO.Repos.Interfaces;
using DAO.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Repos.Implementations
{
    public class RepositoryFactory
    {
        private readonly API _apiService;

        // repository factory koji prima api servis te omogućuje dohvaćanje potrebnih repozitorija za daljnju manipulaciju
        public RepositoryFactory(API apiService)
        {
            _apiService = apiService;
        }

        public TeamRepository GetTeamRepository() => new TeamRepository(_apiService);
        public MatchRepository GetMatchRepository() => new MatchRepository(_apiService);
        public PlayerRepository GetPlayerRepository()
        {
            var matchRepository = GetMatchRepository();
            return new PlayerRepository(matchRepository);
        }
        public ResultRepository GetResultRepository() => new ResultRepository(_apiService);
        public FileRepository GetFileRepository() => new FileRepository();

    }
}
