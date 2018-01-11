﻿using System;
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
        public ICollection<Planet> Planets { get; set; } = new List<Planet>();
        [XmlIgnore]
        public ICollection<Player> Players { get; set; } = new List<Player>();
    }
}
