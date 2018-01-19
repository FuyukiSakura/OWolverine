using OWolverine.Models.Ogame;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OWolverine.Models.StarMapViewModels
{
    public class StarSearchViewModel
    {
        [Required]
        public int ServerId { get; set; }
        public string PlayerName { get; set; }

        //Display data
        public Universe[] Servers { get; set; }
    }
}
