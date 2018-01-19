using OWolverine.Models.Ogame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWolverine.Models.StarMapViewModels
{
    public class StarSearchResultViewModel
    {
        public List<PlanetViewModel> Planets { get; set; } = new List<PlanetViewModel>();
        public StarSearchResultViewModel(Planet[] planets)
        {
            foreach(var planet in planets)
            {
                //Cast to VM object
                Planets.Add(new PlanetViewModel(planet));
            }
        }
    }

    public class PlanetViewModel
    {
        public Planet Planet { get; set; }
        public PlanetViewModel(Planet planet)
        {
            Planet = planet;
        }

        public bool HasMoon => Planet.Moon != null;
    }
}
