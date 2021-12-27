using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarEmpire
{
    public interface IStarSystem
    {
        bool IsHomeworld { get; }
        bool IsExplored { get; }
        bool ResourceGatheringHalted { get; set; }
        string Name { get; }
        DistanceEnum Distance { get; }
        int ResourceRate { get; }
        int WealthRate { get; }
        int Resistance { get; }
        int VictoryPoints { get; }
        Empire Owner { get; set; }
        InvasionResult Invade(Empire empire);
        void AddResources();
        int LocationX { get; set; }
        int LocationY { get; set; }

    }
}
