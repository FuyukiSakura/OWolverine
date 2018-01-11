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
        public int ServerId { get; set; }
        [ForeignKey("ServerId")]
        public Universe Server { get; set; }

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
                if (value.Contains("a")) IsAdmin = true;
                if (value.Contains("u")) IsVocation = true;
                if (value.Contains("o")) IsFlee = true;
                if (value.Contains("i")) IsInactive = true;
                if (value.Contains("I")) IsLeft = true;
            }
        }
        [XmlAttribute("alliance")]
        public int AllianceId { get; set; }

        //Status Property
        public bool IsAdmin { get; set; }
        public bool IsFlee { get; set; }
        public bool IsVocation { get; set; }
        public bool IsInactive { get; set; }
        public bool IsLeft { get; set; }

        [Timestamp]
        public DateTime LastUpdate { get; set; }
    }

    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.ToTable("Player", "og")
                .HasAlternateKey(e => new { e.Id, e.ServerId });
            builder.HasOne(p => p.Server)
                .WithOne();
        }
    }
}
