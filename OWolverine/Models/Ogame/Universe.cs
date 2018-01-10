using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWolverine.Models.Ogame
{
    public class Universe : UniverseXml
    {
        public DateTime LastUpdate { get; set; }

        //Data
        public ICollection<Planet> Planets { get; set; } = new List<Planet>();
        public ICollection<Player> Players { get; set; } = new List<Player>();
    }
}
