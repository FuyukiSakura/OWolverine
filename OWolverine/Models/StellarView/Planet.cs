using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OWolverine.Models.StellarView
{
    [Serializable()]
    [XmlRoot(ElementName = "planet")]
    public class Planet
    {
        [XmlAttribute("name")]
        public string Name;
        [XmlAttribute("coords")]
        public string Coords;
    }
}
