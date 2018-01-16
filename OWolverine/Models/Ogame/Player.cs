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
    [XmlRoot("players")]
    public class PlayerList
    {
        [XmlElement("player")]
        public List<Player> Players { get; set; }
    }

    public class Player
    {
        public int Id { get; set; }
        [XmlAttribute("id")]
        public int PlayerId { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("status")]
        [NotMapped]
        public string Status {
            get
            {
                string statusText = "";
                if (IsAdmin) statusText += "a";
                if (IsFlee) statusText += "o";
                if (IsVocation) statusText += "v";
                if (IsInactive) statusText += "i";
                if(IsLeft) statusText += "I";
                return statusText;
            }
            set
            {
                IsAdmin = value.Contains("a");
                IsVocation = value.Contains("u");
                IsFlee = value.Contains("o");
                IsInactive = value.Contains("i");
                IsLeft = value.Contains("I");
            }
        }

        [XmlAttribute("alliance")]
        [NotMapped]
        private int _AllianceId { get; set; }
        public int? AllianceId
        {
            get
            {
                if (_AllianceId == -1) {
                    return null;
                }
                else
                {
                    return _AllianceId;
                }
            }
            set => _AllianceId = value == null ? -1 : (int)value;
        }
        public Alliance Alliance { get; set; }

        //Status Property
        public bool IsAdmin { get; set; }
        public bool IsFlee { get; set; }
        public bool IsVocation { get; set; }
        public bool IsInactive { get; set; }
        public bool IsLeft { get; set; }

        public List<Planet> Planets { get; set; } = new List<Planet>();
        public int ServerId { get;set; }
        public Universe Server { get; set; }
        public DateTime LastUpdate { get; set; }
    }

    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.ToTable("Player", "og");
            builder.HasAlternateKey(e => new { e.PlayerId, e.ServerId });
            builder.HasOne(e => e.Server)
                .WithMany(u => u.Players)
                .HasForeignKey(e => e.ServerId);
            builder.HasOne(e => e.Alliance)
                .WithMany(a => a.Members)
                .HasForeignKey(e => e.AllianceId)
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasMany(e => e.Planets)
                .WithOne(p => p.Owner)
                .HasForeignKey(p => p.OwnerId);
        }
    }
}
