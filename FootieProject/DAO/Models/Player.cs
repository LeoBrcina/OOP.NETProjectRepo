using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Models
{
    public class Player
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("captain")]
        public bool Captain { get; set; }

        [JsonProperty("shirt_number")]
        public long ShirtNumber { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }
        public int Goals { get; set; }
        public int YellowCards { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Player other)
            {
                return Name == other.Name && ShirtNumber == other.ShirtNumber;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, ShirtNumber);
        }

        public override string ToString() => $"{ShirtNumber} - {Name} ({Position}){(Captain ? " [C]" : "")}";
    }
}
