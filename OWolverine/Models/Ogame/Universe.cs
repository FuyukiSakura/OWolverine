using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWolverine.Models.Ogame
{
    public class Universe : UniverseXml
    {
        //Data
        public ICollection<Planet> Planets { get; set; }
        public ICollection<Player> Players { get; set; }
    }
}
