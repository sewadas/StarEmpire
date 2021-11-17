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
                tech.Add(new Tech() { Cost = 2, Name = Context.InterspeciesCommerce, RequiresYear = 1 });
                tech.Add(new Tech() { Cost = 3, Name = Context.HyperTelevision, RequiresYear = 1 });
                tech.Add(new Tech() { Cost = 2, Name = Context.RobotWorkers, RequiresYear = 1 });
                tech.Add(new Tech() { Cost = 3, Name = Context.CapitalShips, RequiresYear = 1 });
                tech.Add(new Tech() { Cost = 3, Name = Context.InterstellarBanking, RequiresYear = 2 });
                tech.Add(new Tech() { Cost = 5, Name = Context.InterstellarDiplomacy, RequiresYear = 2 });
                tech.Add(new Tech() { Cost = 4, Name = Context.PlanetaryDefenses, RequiresYear = 2 });
                tech.Add(new Tech() { Cost = 4, Name = Context.ForwardStarbases, RequiresYear = 2 });
                return tech;
            }
        }
    }
}
