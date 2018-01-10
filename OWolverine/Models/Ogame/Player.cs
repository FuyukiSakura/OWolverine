using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWolverine.Models.Ogame
{
    public class Player
    {
        public int Id { get; set; }
        public string ServerId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string ServerAlliance { get; set; }
    }
}
