using CSharpUtilities;
using OWolverine.Models.Ogame;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OWolverine.Models.StarMapViewModels
{
    public class StarIndexViewModel
    {
        //Search Form
        [Required]
        public StarSearchViewModel SearchViewModel { get; set; }

        //Display Elements
        public List<PlayerViewModel> Players { get; set; } = new List<PlayerViewModel>();
        public List<Planet> Planets { get; set; } = new List<Planet>();
        public List<UniverseViewModel> Servers { get; set; } = new List<UniverseViewModel>();
        public bool IsSearch { get; set; }
        public StarIndexViewModel(Universe[] universe)
        {
            //Init SearchViewModel
            SearchViewModel = new StarSearchViewModel()
            {
                Servers = universe
            };

            foreach (var u in universe)
            {
                //Get a random player from server
                var tryResult = u.Players.Where(e => e.IsActive).OrderBy(r => Guid.NewGuid()).Take(1).ToArray();
                var playerName = "";
                if (tryResult.Any())
                {
                    playerName = tryResult[0].Name;
                }

                //Cast universe data to VM object
                Servers.Add(new UniverseViewModel(u, playerName));
            }
        }

        /// <summary>
        /// Cast players into PlayerViewModel
        /// </summary>
        /// <param name="players"></param>
        public void AssignPlayers(List<Player> players)
        {
            Players = new List<PlayerViewModel>();
            foreach (var p in players)
            {
                Players.Add(new PlayerViewModel(p));
            }
        }
    }

    /// <summary>
    /// Player status ASP displayable format
    /// </summary>
    public class PlayerStatusViewModel
    {
        [Display(Name = "g")]
        public bool IsBanned { get; set; }
        [Display(Name = "o")]
        public bool IsFlee { get; set; }
        [Display(Name = "i")]
        public bool IsInactive { get; set; }
        [Display(Name = "I")]
        public bool IsLeft { get; set; }
    }
}
