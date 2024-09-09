using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DAO.Models;
using RestSharp;

namespace DAO.Services
{
    public sealed class API
    {
        // singleton implementacija api servisa uz metodu za konfiguraciju istoga za željeno i odabrano svjetsko prvenstvo
        private static readonly Lazy<API> _instance = new Lazy<API>(() => new API());

        private readonly RestClient _restClient;
        private string _resultsUrl;
        private string _teamsUrl;
        private string _matchesUrl;

        // privatni konstruktor za sprječavanje novog instanciranja api servisa
        private API()
        {
            _restClient = new RestClient("https://worldcup-vua.nullbit.hr");
        }

        // public property za dohvaćanje instance
        public static API Instance => _instance.Value;

        // metoda koja na jednom mjestu konfigurira potrebne url-ove za odabrano prvenstvo prilikom dohvaćanja instance
        public void ConfigureUrls(string worldCup)
        {
            if (worldCup == "Men's World Cup 2018")
            {
                _resultsUrl = "/men/teams/results";
                _teamsUrl = "/men/teams";
                _matchesUrl = "/men/matches";
            }
            else if (worldCup == "Women's World Cup 2019")
            {
                _resultsUrl = "/women/teams/results";
                _teamsUrl = "/women/teams";
                _matchesUrl = "/women/matches";
            }
            else
            {
                throw new ArgumentException("Invalid World Cup type.");
            }
        }

        // metoda za dohvacanje rezultata i prosljedivanje repositoryu
        public async Task<List<Result>> GetResultsAsync()
        {
            return await GetAllDataAsync<Result>(_resultsUrl);
        }

        // metoda za dohvacanje timova i prosljedivanje repositoryu
        public async Task<List<Team>> GetTeamsAsync()
        {
            return await GetAllDataAsync<Team>(_teamsUrl);
        }

        // metoda za dohvacanje matcheva i prosljedivanje repositoryu
        public async Task<List<Match>> GetMatchesAsync()
        {
            return await GetAllDataAsync<Match>(_matchesUrl);
        }

        // generička metoda za dohvaćanje bilo kojeg potrebnog objekta sa api-a uz rutinske provjere
        private async Task<List<T>> GetAllDataAsync<T>(string url)
        {
            try
            {
                var request = new RestRequest(url);
                var response = await _restClient.ExecuteGetAsync<List<T>>(request);
                List<T> result = JsonConvert.DeserializeObject<List<T>>(response.Content);

                if (response.IsSuccessful)
                {
                    return result;
                }
                else
                {
                    Console.WriteLine($"Error fetching data: {response.ErrorMessage}");
                    return new List<T>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
                return new List<T>();
            }
        }

        // metoda za dohvaćanje matcheva po url-u
        public async Task<List<Match>> GetMatchesByUrlAsync(string url)
        {
            return await GetAllDataAsync<Match>(url);
        }
    }
}
