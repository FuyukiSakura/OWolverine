using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OWolverine.Models.Ogame
{
    public class Player
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string ServerId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string ServerAlliance { get; set; }
    }
}
