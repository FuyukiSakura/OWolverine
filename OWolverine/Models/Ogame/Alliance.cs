using CSharpUtilities;
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
        public double Timestamp { get; set; }
        [XmlIgnore]
        public DateTime LastUpdate => DateTimeHelper.UnixTimeStampToDateTime(Timestamp);
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
        public int FounderId { get; set; }
        [XmlElement("player")]
        public List<PlayerId> Members { get; set; } = new List<PlayerId>();
        [XmlAttribute("open")]
        public bool IsOpen { get; set; }

        /// <summary>
        /// Update alliance with another alliance object
        /// </summary>
        /// <param name="obj"></param>
        public void Update(IUpdatable obj)
        {
            if (obj is Alliance alliance)
            {
                if (FounderId != alliance.FounderId) FounderId = alliance.FounderId;
                if (Name != alliance.Name) Name = alliance.Name;
                if (Tag != alliance.Tag) Tag = alliance.Tag;
                //Remove members that have left
                Members.RemoveAll(e => !alliance.Members.Any(a => a.Id == e.Id));
                //Add new members
                Members.AddRange(alliance.Members.Where(a => !Members.Any(e => e.Id == a.Id)));
            }
        }
    }
}
