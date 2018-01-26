using CSharpUtilities;
using Newtonsoft.Json;
using OWolverine.Services.Cosmos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OWolverine.Models.Ogame
{
    [XmlRoot("serverData")]
    public class Universe
    {
        //Info
        [JsonProperty("id")]
        public string Id => StarMapBLL.GetServerId(ServerId);
        [XmlElement("number")]
        [Display(Name = "Server ID")]
        public int ServerId { get; set; }
        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("domain")]
        public string Domain { get; set; }

        //Settings
        [XmlElement("speed")]
        public float Speed { get; set; }
        [XmlElement("speedFleet")]
        public float FleetSpeed { get; set; }
        [XmlElement("galaxies")]
        public int Galaxies { get; set; }
        [XmlElement("systems")]
        public int Systems { get; set; }
        [XmlElement("acs")]
        public bool Acs { get; set; }
        [XmlElement("rapidFire")]
        public bool RapidFire { get; set; }
        [XmlElement("debrisFactor")]
        public float DebrisFactor { get; set; }
        [XmlElement("debrisFactorDef")]
        public float DefToDebris { get; set; }
        [XmlElement("topScore")]
        public int TopScore { get; set; }
        [XmlElement("donutGalaxy")]
        public bool DonutGalaxy { get; set; }
        [XmlElement("donutSystem")]
        public bool DonutSystem { get; set; }
        [XmlElement("wfEnabled")]
        public bool WreckField { get; set; }
        [XmlElement("globalDeuteriumSaveFactor")]
        public float DeuteriumSaveFactor { get; set; }
        [XmlElement("probeCargo")]
        public int ProbeCargo { get; set; }

        [JsonIgnore]
        [XmlAttribute("timestamp")]
        public double Timestamp { get; set; }
        [XmlIgnore]
        public DateTime LastUpdate { get; set; }

        //Data
        [XmlIgnore]
        public List<Player> Players { get; set; } = new List<Player>();
        public DateTime PlayersLastUpdate { get; set; }
        [XmlIgnore]
        public List<Alliance> Alliances { get; set; } = new List<Alliance>();
        public DateTime AllianceLastUpdate { get; set; }
        public DateTime PlanetsLastUpdate { get; set; }
        [XmlIgnore]
        public UniverseStat Statistic { get; set; } = new UniverseStat();
    }

    public class UniverseStat
    {
        [Display(Name = "Active")]
        public int ActivePlayerCount { get; set; }
        [Display(Name = "Players")]
        public int PlayerCount { get; set; }
        [Display(Name = "Moons")]
        public int MoonCount { get; set; }
        [Display(Name = "Planets")]
        public int PlanetCount { get; set; }
        [Display(Name = "Map update day")]
        public string MapUpdateDay { get; set; }
        [Display(Name = "Last Update (GMT +8)")]
        public DateTime LastUpdate { get; set; }
    }
}
