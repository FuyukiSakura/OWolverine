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
    [Serializable()]
    [XmlRoot(ElementName = "planet")]
    public class Planet
    {
        public int Id { get; set; }
        [XmlAttribute("id")]
        public int PlanetId { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }

        private Coordinate _coords { get; set; } = new Coordinate();
        [XmlAttribute("coords")]
        public string Coords {
            get {
                return String.Format("{0}:{1}:{2}", _coords.Galaxy, _coords.System, _coords.Location);
            }
            set {
                var arr = value.Split(":");
                _coords.Galaxy = Convert.ToInt32(arr[0]);
                _coords.System = Convert.ToInt32(arr[1]);
                _coords.Location = Convert.ToInt32(arr[2]);
            }
        }

        [XmlAttribute("player")]
        public int OwnerId { get; set; }
        [XmlIgnore]
        public Player Owner { get; set; } = new Player();

        public int ServerId { get; set; }
        public Universe Server { get; set; }
        public DateTime LastUpdated { get; set; }
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
                .HasAlternateKey(e => new { e.Id, e.ServerId });
            builder.HasOne(e => e.Server)
                .WithMany(u => u.Planets)
                .HasForeignKey(e => e.ServerId);
        }
    }
}
