using CSharpUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using OWolverine.Models.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OWolverine.Models.Ogame
{
    [XmlRoot("universe")]
    public class PlanetList
    {
        [XmlElement("planet")]
        public List<Planet> Planets { get; set; }
        [XmlAttribute("timestamp")]
        public double Timestamp { get; set; }
        [XmlIgnore]
        public DateTime LastUpdate => DateTimeHelper.UnixTimeStampToDateTime(Timestamp);
    }

    [Serializable()]
    [XmlRoot(ElementName = "planet")]
    public class Planet : IUpdatable
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlElement("moon")]
        public Moon Moon { get; set; }
        public Coordinate Coords { get; set; } = new Coordinate();
        [XmlAttribute("coords")]
        [Display(Name = "Coordinates")]
        [JsonIgnore]
        public string Coord {
            get {
                return String.Format("{0}:{1}:{2}", Coords.Galaxy, Coords.System, Coords.Location);
            }
            set {
                var arr = value.Split(":");
                Coords.Galaxy = Convert.ToInt32(arr[0]);
                Coords.System = Convert.ToInt32(arr[1]);
                Coords.Location = Convert.ToInt32(arr[2]);
            }
        }
        [XmlAttribute("player")]
        public int OwnerId { get; set; }
        [JsonIgnore]
        public Player Owner { get; set; }
        public DateTime LastUpdated { get; set; }

        public void Update(IUpdatable obj)
        {
            if(obj is Planet planet)
            {
                if(Name != planet.Name)
                    Name = planet.Name;
                if (OwnerId != planet.OwnerId)
                    OwnerId = planet.OwnerId;
                if (planet.Moon != null)
                {
                    if (Moon == null)
                    {
                        //Create new moon if not exists
                        Moon = new Moon();
                    }
                    Moon.Update(planet.Moon);
                }
            }
        }
    }

    public class Coordinate
    {
        public int Galaxy { get; set; }
        public int System { get; set; }
        public int Location { get; set; }
        [JsonIgnore]
        public bool IsEmpty => Galaxy == 0 && System == 0 && Location == 0;
        [JsonIgnore]
        public bool IsAddress => Galaxy != 0 && System != 0 && Location != 0;
        public bool IsEqual(Coordinate coord)
        {
            return Galaxy == coord.Galaxy && System == coord.System && Location == coord.Location;
        }
    }

    public class Moon : IUpdatable
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("size")]
        public int Size { get; set; }

        public void Update(IUpdatable obj)
        {
            if(obj is Moon moon)
            {
                if(Name != moon.Name)
                    Name = moon.Name;
                if(Size != moon.Size)
                    Size = moon.Size;
            }
        }
    }
}
