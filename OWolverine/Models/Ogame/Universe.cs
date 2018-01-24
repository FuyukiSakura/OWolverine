using CSharpUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OWolverine.Models.Ogame
{
    [XmlRoot("serverData")]
    public class Universe
    {
        //Info
        [XmlElement("number")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Server ID")]
        public int Id { get; set; }
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
        [XmlIgnore]
        public DateTime LastUpdate { get; set; }

        //Data
        [XmlIgnore]
        public List<Player> Players { get; set; } = new List<Player>();
        public DateTime? PlayersLastUpdate { get; set; }
        [XmlIgnore]
        public List<Alliance> Alliances { get; set; } = new List<Alliance>();
        public DateTime? AllianceLastUpdate { get; set; }
        [XmlIgnore]
        public List<Planet> Planets { get; set; } = new List<Planet>();
        public DateTime? PlanetsLastUpdate { get; set; }
    }

    public class UniverseViewModel
    {
        public Universe Universe { get; set; }
        public string FeelingLucky { get; set; }
        private List<DateTime> ServerDates { get; set; } = new List<DateTime>();
        public UniverseViewModel(Universe server, string playerName)
        {
            Universe = server;
            FeelingLucky = playerName;
            if (server.PlayersLastUpdate != null) ServerDates.Add((DateTime)server.PlayersLastUpdate);
            if (server.AllianceLastUpdate != null) ServerDates.Add((DateTime)server.AllianceLastUpdate);
            if (server.PlanetsLastUpdate != null) ServerDates.Add((DateTime)server.PlanetsLastUpdate);
        }

        [Display(Name = "Server ID")]
        public int Id => Universe.Id;
        public string Name => Universe.Name;
        [Display(Name = "Active")]
        public int ActivePlayers => Universe.Players.Where(p => p.IsActive).Count();
        [Display(Name = "Moons")]
        public int Moons => Universe.Planets.Where(p => p.Moon != null).Count();
        [Display(Name = "Map update day")]
        public string MapUpdateDay => Universe.PlanetsLastUpdate == null ? "" : ((DateTime)Universe.PlanetsLastUpdate).ToString("ddd");
        [Display(Name = "Last Update (Server time GMT +8)")]
        public DateTime? LastUpdate => DateTimeHelper.GetLatestDate(ServerDates);
    }
}
