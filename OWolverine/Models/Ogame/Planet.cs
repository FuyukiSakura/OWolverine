using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
        public double LastUpdate { get; set; }
    }

    [Serializable()]
    [XmlRoot(ElementName = "planet")]
    public class Planet : IUpdatable
    {
        public int Id { get; set; }
        [XmlAttribute("id")]
        public int PlanetId { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlElement("moon")]
        public Moon Moon { get; set; }

        [NotMapped]
        public Coordinate Coords { get; set; } = new Coordinate();
        [XmlAttribute("coords")]
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
        [XmlIgnore]
        public Player Owner { get; set; }

        public int ServerId { get; set; }
        public Universe Server { get; set; }
        public DateTime LastUpdated { get; set; }

        public void Update(IUpdatable obj)
        {
            if(obj is Planet planet)
            {
                Name = planet.Name;
                if (Moon == null) Moon = new Moon(); //Create new moon if not exists
                Moon.Update(planet.Moon);
            }
        }
    }

    public class Coordinate
    {
        public int Galaxy { get; set; }
        public int System { get; set; }
        public int Location { get; set; }
    }

    public class PlanetConfiguration : IEntityTypeConfiguration<Planet>
    {
        public void Configure(EntityTypeBuilder<Planet> builder)
        {
            builder.ToTable("Planet", "og")
                .HasAlternateKey(e => new { e.PlanetId, e.ServerId });
            builder.HasOne(e => e.Server)
                .WithMany(u => u.Planets)
                .HasForeignKey(e => e.ServerId);
        }
    }

    public class Moon : IUpdatable
    {
        public int Id { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("size")]
        public int Size { get; set; }

        public void Update(IUpdatable obj)
        {
            if(obj is Moon moon)
            {
                Name = moon.Name;
                Size = moon.Size;
            }
        }
    }

    public class MoonConfiguration : IEntityTypeConfiguration<Moon>
    {
        public void Configure(EntityTypeBuilder<Moon> builder)
        {
            builder.ToTable("Moon", "og");
            builder.HasOne<Planet>()
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
