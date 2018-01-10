using CSharpUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OWolverine.Models.Ogame
{
    [XmlRoot("serverData")]
    public class UniverseXml : IUpCastable
    {
        //Info
        [XmlElement("number")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("domain")]
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
    }
}
