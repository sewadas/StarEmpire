using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarEmpire
{
    public static class TechFactory
    {
        public static List<ITech> TechTemplateList
        {
            get
            {
                var tech = new List<ITech>();
                tech.Add(new Tech() { Cost = 2, Name = Context.InterspeciesCommerce, RequiresYear = 1, Description = "May exchange Resources for Wealth or Wealth for Resources" });
                tech.Add(new Tech() { Cost = 3, Name = Context.HyperTelevision, RequiresYear = 1, Description = "Add +1 to Resistance during revolt events" });
                tech.Add(new Tech() { Cost = 2, Name = Context.RobotWorkers, RequiresYear = 1, Description = "Receive resources instead of zero during a strike event" });
                tech.Add(new Tech() { Cost = 3, Name = Context.CapitalShips, RequiresYear = 1, Description = "Required for advancing Military Strength beyond 3" });
                tech.Add(new Tech() { Cost = 3, Name = Context.InterstellarBanking, RequiresYear = 2, Description = "Required to stockpile more than 3 Resources/Wealth" });
                tech.Add(new Tech() { Cost = 5, Name = Context.InterstellarDiplomacy, RequiresYear = 2, Description = "Select world for successfully explore-attack a world" });
                tech.Add(new Tech() { Cost = 4, Name = Context.PlanetaryDefenses, RequiresYear = 2, Description = "Add +1 to Resistance during invasion events" });
                tech.Add(new Tech() { Cost = 4, Name = Context.ForwardStarbases, RequiresYear = 2, Description = "Required for selecting Distant Systems to explore/conquer" });
                return tech;
            }
        }
    }
}
