using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OWolverine.Models.Ogame
{
    public class Player
    {
        [XmlAttribute("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key, Column(Order = 0)]
        public int Id { get; set; }
        [Key, Column(Order = 1)]
        public int Server { get; set; }
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
        public string ServerAlliance { get; set; }

        //Status Property
        public bool IsAdmin { get; set; }
        public bool IsFlee { get; set; }
        public bool IsVocation { get; set; }
        public bool IsInactive { get; set; }
        public bool IsLeft { get; set; }
    }

    public enum PlayerStatus
    {
        Admin,
        Flee,
        Vocation,
        Inactive,
        Left
    }
}
