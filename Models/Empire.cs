using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarEmpire
{
    public class Empire
    {
        public string Name { get; set; }
        public List<IStarSystem> ConqueredSystems { get; private set; } = new List<IStarSystem>();
        public List<ITech> OwnedTechs { get; private set; } = new List<ITech>();
        public IStarSystem Homeworld { get; set; } 
        public int Military { get; set; }
        public int Resources { get; set; }
        public int Wealth { get; set; }
        public int Era { get; set; } = 1;
        public int Turn { get; set; } = 1;
        public int VictoryPoints 
        { 
            get 
            {
                return ConqueredSystems.Sum(o => o.VictoryPoints) +
                    OwnedTechs.Count() +
                    (OwnedTechs.Count() == TechFactory.TechTemplateList.Count() ? 1 : 0) +
                    (ConqueredSystems.Count() == 10 ? 3 : 0);
            } 
        }
    }
}
