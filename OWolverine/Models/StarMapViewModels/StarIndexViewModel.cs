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
        public List<Player> Players { get; set; } = new List<Player>();
        public List<UniverseViewModel> Servers { get; set; } = new List<UniverseViewModel>();

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
    }

    public class UniverseViewModel
    {
        public Universe Universe { get; set; }
        public string FeelingLucky { get; set; }
        private List<DateTime> ServerDates { get; set; } = new List<DateTime>();
        public UniverseViewModel(Universe server, string playerName)
        {
            Universe = server;
            FeelingLucky = playerName;
            if (server.PlayersLastUpdate != null) ServerDates.Add((DateTime)server.PlayersLastUpdate);
            if (server.AllianceLastUpdate != null) ServerDates.Add((DateTime)server.AllianceLastUpdate);
            if (server.PlanetsLastUpdate != null) ServerDates.Add((DateTime)server.PlanetsLastUpdate);
        }

        [Display(Name = "Server ID")]
        public int Id => Universe.Id;
        public string Name => Universe.Name;
        public int ActivePlayers => Universe.Players.Where(p => p.IsActive).Count();
        [Display(Name = "Last Update")]
        public DateTime? LastUpdate => DateTimeHelper.GetLatestDate(ServerDates);
    }
}
