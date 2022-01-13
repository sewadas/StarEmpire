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
            e => { e.Wealth++; return "Your Higness, we salvaged humongous asteroid with valuable resources. It increased our wealth."; },
            e => { foreach (var star in e.ConqueredSystems) star.ResourceGatheringHalted = true; e.Homeworld.ResourceGatheringHalted = true; return "Your Highness, our colonies started Interplanetary Strike. Wealth and Resource gathering have been halted."; },
            e => { e.Resources++; return "Your Highness, we discovered derelict starship. It increased our resources pool"; },
            e => 
            {
                if (e.ConqueredSystems.Any() == false) return "Your Highness, there is nothing to report.";
                var sys = e.ConqueredSystems.OrderBy(o => o.Resistance).First();
                var result = sys.Invade(EmpireFactory.Build(Context.Revolt, Math.Min((int)(e.Year * 1.5), 3)));
                if (result == InvasionResult.Failure) return $"Your Highness, there was large revolt on: {sys.Name}. Our troops sucessfully crushed the rebels.";
                else
                {
                    e.ConqueredSystems.Remove(sys);
                    sys.Owner = null;
                    return $"Your Highness, there was large revolt on: {sys.Name}. Our troops have been anihilated.";
                }
            },
            e => 
            {
                if (e.ConqueredSystems.Any() == false) return "Your Highness, there is nothing to report.";
                var sys = e.ConqueredSystems.Last();
                var result = sys.Invade(EmpireFactory.Build(Context.Invasion, Math.Min(e.Year, 2)));
                if (result == InvasionResult.Failure) return $"Your Highness, there was enemy empire invasion on: {sys.Name}. Our troops sucessfully crushed the invasion fleet.";
                else
                {
                    e.ConqueredSystems.Remove(sys);
                    sys.Owner = null;
                    return $"Your Highness, there was enemy empire invasion on: {sys.Name}. Our troops have been anihilated.";
                }
            },
            e => { return "Your Highness, there is nothing to report."; },
            e => 
            {
                if (e.ConqueredSystems.Any() == false) return "Your Highness, there is nothing to report.";
                var sys = e.ConqueredSystems.Last();
                var result = sys.Invade(EmpireFactory.Build(Context.Invasion, Math.Min(e.Year, 3)));
                if (result == InvasionResult.Failure) return $"Your Highness, there was large invasion of enemy empire on:  {sys.Name}. Our troops successfully crushed the invasion fleet.";
                else
                {
                    e.ConqueredSystems.Remove(sys);
                    sys.Owner = null;
                    return $"Your Highness, there was large invasion of enemy empire on: {sys.Name}.  Our troops have been anihilated.";
                }
            },
            e => 
            {
                if (e.ConqueredSystems.Any() == false) return "Your Highness, there is nothing to report.";
                var sys = e.ConqueredSystems.OrderBy(o => o.Resistance).First();
                var result = sys.Invade(EmpireFactory.Build(Context.Revolt, Math.Min(e.Year, 2)));
                if (result == InvasionResult.Failure) return $"Your Highness, there was revolt on: {sys.Name}. Our troops sucessfully crushed the rebels.";
                else
                {
                    e.ConqueredSystems.Remove(sys);
                    sys.Owner = null;
                    return $"Your Highness, there was revolt on: {sys.Name}. Our troops have been anihilated.";
                }
            }
        };
    }
}
