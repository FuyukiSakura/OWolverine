using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWolverine.Models.Ogame
{
    public class Universe
    {
        //Info
        public string Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }

        //Settings
        public float Speed { get; set; }
        public float FleetSpeed { get; set; }
        public int Galaxies { get; set; }
        public int Systems { get; set; }
        public bool RapidFire { get; set; }
        public float DebrisFactor { get; set; }
        public float DefToDebris { get; set; }
        public int TopScore { get; set; }
        public bool DonutGalaxy { get; set; }
        public bool DonutSystem { get; set; }
        public bool WreckField { get; set; }
        public float DeuteriumSaveFactor { get; set; }

        //Data
        public ICollection<Planet> Planets { get; set; }
        public ICollection<Player> Players { get; set; }
    }
}
