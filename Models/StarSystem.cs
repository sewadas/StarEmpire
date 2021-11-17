using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarEmpire
{
    public class StarSystem : IStarSystem
    {
        public bool IsHomeworld { get; set; }
        public bool IsExplored { get; set; }
        public string Name { get; set; }
        public DistanceEnum Distance{ get; set; }
        public int ResourceRate { get; set; }
        public int WealthRate { get; set; }
        public int Resistance { get; set; }
        public int VictoryPoints { get; set; }
        public Empire Owner { get; set; }
        public bool ResourceGatheringHalted { get; set; }

        public InvasionResult Invade(Empire empire)
        {
            if (Distance == DistanceEnum.Distant && empire.OwnedTechs.Any(o => o.Name == Context.ForwardStarbases) == false) return InvasionResult.NotPossible;
            if (empire.ConqueredSystems.Contains(this)) return InvasionResult.NotPossible;
            if (empire.Homeworld == this) return InvasionResult.NotPossible;
            int resistance = Resistance;
            IsExplored = true;
            if (empire.Name == Context.Revolt && Owner.OwnedTechs.Any(o => o.Name == Context.HyperTelevision)) resistance++;
            if (empire.Name == Context.Invasion && Owner.OwnedTechs.Any(o => o.Name == Context.PlanetaryDefenses)) resistance++;
            if (empire.Military + Context.RandomFactor >= resistance) return InvasionResult.Success;
            else return InvasionResult.Failure;
        }

        public void AddResources()
        {
            if (ResourceGatheringHalted == false)
            {
                Owner.Resources += ResourceRate;
                Owner.Wealth += WealthRate;
            }
            else
            {
                if (Owner.OwnedTechs.Any(o => o.Name == Context.RobotWorkers))
                {
                    Owner.Resources += (int)Math.Ceiling((decimal)ResourceRate / 2);
                    Owner.Wealth += (int)Math.Ceiling((decimal)WealthRate / 2);
                }
                ResourceGatheringHalted = false;
            }
            if (Owner.OwnedTechs.Any(o => o.Name == Context.InterstellarBanking))
            {
                Owner.Resources = Math.Min(Owner.Resources, 5);
                Owner.Wealth = Math.Min(Owner.Wealth, 5);
            }
            else
            {
                Owner.Resources = Math.Min(Owner.Resources, 3);
                Owner.Wealth = Math.Min(Owner.Wealth, 3);
            }
        }
    }
}
