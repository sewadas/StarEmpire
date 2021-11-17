using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarEmpire
{
    public class IncidentFactory
    {
        public static Func<Empire, string>[] IncidentTemplateList = new Func<Empire, string>[]
        {
            e => { e.Wealth++; return "Humongous asteroid. Wealth +1"; },
            e => { foreach (var star in e.ConqueredSystems) star.ResourceGatheringHalted = true; return "Interplanetary Strike"; },
            e => { e.Resources++; return "Derelict starship. Resources +1"; },
            e => 
            {
                if (e.ConqueredSystems.Any() == false) return "Peace'n'Quiet";
                var sys = e.ConqueredSystems.OrderBy(o => o.Resistance).First();
                var result = sys.Invade(EmpireFactory.Build(Context.Revolt, Math.Min((int)(e.Year * 1.5), 3)));
                if (result == InvasionResult.Failure) return $"Large revolt on: {sys.Name} failed";
                else
                {
                    e.ConqueredSystems.Remove(sys);
                    sys.Owner = null;
                    return $"Large revolt on: {sys.Name}. System lost.";
                }
            },
            e => 
            {
                if (e.ConqueredSystems.Any() == false) return "Peace'n'Quiet";
                var sys = e.ConqueredSystems.Last();
                var result = sys.Invade(EmpireFactory.Build(Context.Invasion, Math.Min(e.Year, 2)));
                if (result == InvasionResult.Failure) return $"Invasion on: {sys.Name} failed";
                else
                {
                    e.ConqueredSystems.Remove(sys);
                    sys.Owner = null;
                    return $"Invasion on: {sys.Name}. System lost.";
                }
            },
            e => { return "Peace'n'Quiet"; },
            e => 
            {
                if (e.ConqueredSystems.Any() == false) return "Peace'n'Quiet";
                var sys = e.ConqueredSystems.Last();
                var result = sys.Invade(EmpireFactory.Build(Context.Invasion, Math.Min(e.Year, 3)));
                if (result == InvasionResult.Failure) return $"Large invasion on: {sys.Name} failed";
                else
                {
                    e.ConqueredSystems.Remove(sys);
                    sys.Owner = null;
                    return $"Large Invasion on: {sys.Name}. System lost.";
                }
            },
            e => 
            {
                if (e.ConqueredSystems.Any() == false) return "Peace'n'Quiet";
                var sys = e.ConqueredSystems.OrderBy(o => o.Resistance).First();
                var result = sys.Invade(EmpireFactory.Build(Context.Revolt, Math.Min(e.Year, 2)));
                if (result == InvasionResult.Failure) return $"Revolt on: {sys.Name} failed";
                else
                {
                    e.ConqueredSystems.Remove(sys);
                    sys.Owner = null;
                    return $"Revolt on: {sys.Name}. System lost.";
                }
            }
        };
    }
}
