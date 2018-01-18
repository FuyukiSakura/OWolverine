using OWolverine.Models.Ogame;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OWolverine.Models.StellaViewViewModels
{
    public class StarMapSearchViewModel
    {
        //Search Form
        [Required]
        public int ServerId { get; set; }
        public string PlayerName { get; set; }
        
        //Display Elements
        public List<UniverseViewModel> Servers { get; set; }
    }

    public class UniverseViewModel
    {
        public Universe Universe { get; set; }
        public string FeelingLucky { get; set; }
        public UniverseViewModel(Universe server, string playerName)
        {
            Universe = server;
            FeelingLucky = playerName;
        }

        public int Id => Universe.Id;
        public string Name => Universe.Name;
        public int ActivePlayers => Universe.Players.Where(p => p.IsActive).Count();
    }
}
