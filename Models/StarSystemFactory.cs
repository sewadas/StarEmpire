using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarEmpire
{
    public static class StarSystemFactory
    {
        public static IStarSystem Build(Empire owner, string name, DistanceEnum distance, int resourceRate, int wealthRate, int resistance = 0, int victoryPoints = 0, bool isHomeworld = false)
        {
            var star = new StarSystem();
            star.Owner = owner;
            star.Name = name;
            star.Distance = distance;
            star.ResourceRate = resourceRate;
            star.WealthRate = wealthRate;
            star.Resistance = resistance;
            star.VictoryPoints = victoryPoints;
            if (isHomeworld)
            {
                star.IsHomeworld = isHomeworld;
                star.IsExplored = true;
            }
            return star;
        }

        public static List<IStarSystem> SystemTemplateList = new List<IStarSystem>
        {
            Build(null, "Wolf 359", DistanceEnum.Near, 1, 0, 5, 1),
            Build(null, "Proxima", DistanceEnum.Near, 1, 0, 6, 1),
            Build(null, "Epsilon Eridani", DistanceEnum.Near, 0, 0, 8, 1),
            Build(null, "Cygnus", DistanceEnum.Near, 0, 1, 5, 1),
            Build(null, "Tau Ceti", DistanceEnum.Near, 0, 0, 4, 1),
            Build(null, "Procyon", DistanceEnum.Near, 0, 1, 7, 1),
            Build(null, "Sirius", DistanceEnum.Near, 0, 0, 6, 1),
            Build(null, "Canopus", DistanceEnum.Distant, 0, 1, 9, 2),
            Build(null, "Polaris", DistanceEnum.Distant, 1, 0, 9, 2),
            Build(null, "Alpha Centauri", DistanceEnum.Distant, 0, 0, 10, 3)
        };
    }
}
