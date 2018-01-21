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
        [Display(Name = "Server")]
        public int ServerId { get; set; }
        [Display(Name = "Player Name")]
        public string PlayerName { get; set; }
        public string Alliance { get; set; }
        [Display(Name = "Player Status: ")]
        public PlayerStatusViewModel PlayerStatus { get; set; }
        [Display(Name = "Location")]
        public Coordinate Coords { get; set; }
        [Display(Name = "Range (+/-): ")]
        public int Range { get; set; }
        [Display(Name = "Moon Level")]
        public int MoonLevel { get; set; }

        //Display data
        public Universe[] Servers { get; set; }
        public StarSearchViewModel()
        {
            Coords = new Coordinate
            {
                Galaxy = 0,
                System = 0,
                Location = 0
            };
        }
    }
}
