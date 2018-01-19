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
    [XmlRoot("alliances")]
    public class AllianceList
    {
        [XmlElement("alliance")]
        public List<Alliance> Alliances { get; set; }
        [XmlAttribute("timestamp")]
        public double LastUpdate { get; set; }
    }


    public class Alliance : IUpdatable
    {
        public int Id { get; set; }
        [XmlAttribute("id")]
        public int AllianceId { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("tag")]
        public string Tag { get; set; }
        [XmlAttribute("founder")]
        [NotMapped]
        public int _founderId { get; set; }
        public int? FounderId {
            get
            {
                if (_founderId == -1)
                {
                    return null;
                }
                return _founderId;
            }
            set
            {
                _founderId = value == null ? -1 : (int)value;
            }
        }
        [XmlIgnore]
        public Player Founder { get; set; }
        [XmlElement("player")]
        public List<Player> Members { get; set; } = new List<Player>();
        [XmlAttribute("open")]
        public bool IsOpen { get; set; }

        public int ServerId { get; set; }
        public Universe Server { get; set; }

        /// <summary>
        /// Update alliance with another alliance object
        /// </summary>
        /// <param name="obj"></param>
        public void Update(IUpdatable obj)
        {
            if(obj is Alliance alliance)
            {
                FounderId = alliance.FounderId;
                Name = alliance.Name;
                Tag = alliance.Tag;
                //Remove members that have left
                Members.RemoveAll(e => !alliance.Members.Any(a => a.Id == e.Id));
                //Add new members
                Members.AddRange(alliance.Members.Where(a => !Members.Any(e => e.Id == a.Id)));
            }
        }
    }

    public class AllianceConfiguration : IEntityTypeConfiguration<Alliance>
    {
        public void Configure(EntityTypeBuilder<Alliance> builder)
        {
            builder.ToTable("Alliance", "og");
            builder.HasAlternateKey(e => new { e.AllianceId, e.ServerId });
            builder.HasOne(e => e.Server)
                .WithMany(u => u.Alliances)
                .HasForeignKey(e => e.ServerId);
            builder.HasOne(e => e.Founder)
                .WithOne()
                .HasForeignKey<Alliance>(e => e.FounderId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
