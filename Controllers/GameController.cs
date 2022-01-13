using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace StarEmpire
{
    public class GameController
    {
        private Game _game;
        public Action<int, int, int, int, int> Stats { get; set; }
        public Action<List<IStarSystem>> StarMap { get; set; }
        public Action<bool> AllowMilitaryImprovement { get; set; }
        public Action<bool> AllowTechImprovement { get; set; }
        public Action<List<ITech>, List<ITech>> Techs { get; set; }
        public bool fleetResupply { get; set; } = false;

        public GameController(Game gameModel)
        {
            _game = gameModel;
        }

        public void ShowStarMap()
        {
            StarMap(_game.StarMap);
        }

        public void ShowTech()
        {
            Techs(_game.Player.OwnedTechs, TechFactory.TechTemplateList.Where(o => o.RequiresYear <= _game.Player.Year && _game.Player.OwnedTechs.Exists(p => p.Name == o.Name) == false).ToList());
        }

        public void ExploreOrInvade(IStarSystem star)
        {
            if (fleetResupply)
            {
                MessageBox.ErrorQuery("Information", "Your Highness, our fleet is underway replenishment.", "Ok");
                return;
            }

            if (_game.Player.ConqueredSystems.Contains(star))
            {
                MessageBox.Query("Information", $"{star.Name} {Environment.NewLine} Resistance: {star.Resistance} Resources: {star.ResourceRate} Wealth: {star.WealthRate} VP: {star.VictoryPoints}", "Ok");
                return;
            }

            int res;
            if (star.IsExplored) res = MessageBox.Query("Invasion", $"Your Highness, do you want to invade {star.Name}? {Environment.NewLine} Resistance: {star.Resistance} Resources: {star.ResourceRate} Wealth: {star.WealthRate} VP: {star.VictoryPoints}", "Yes", "No");
            else res = MessageBox.Query("Exploration", $"Your Highness, do you want to explore new star system?", "Yes", "No");

            if (res != 0) return;

            var result = star.Invade(_game.Player);
            if (result == InvasionResult.NotPossible)
            {
                MessageBox.ErrorQuery("Invasion", "Your Highness, invasion is not possible!", "Ok");
                return;
            }

            if (result == InvasionResult.Failure)
            {
                MessageBox.Query("Invasion", $"Your Highness, invasion on {star.Name} has failed. Our fleet has been badly damaged.", "Ok");
                _game.Player.Military--;
                _game.Player.Military = Math.Max(_game.Player.Military, 0);
            }
            if (result == InvasionResult.Success)
            {
                MessageBox.Query("Invasion", $"Your Highness, invasion on {star.Name} has succeeded.", "Ok");
                star.Owner = _game.Player;
                _game.Player.ConqueredSystems.Add(star);
            }
            Stats(_game.Player.Military, _game.Player.Resources, _game.Player.Wealth, _game.Player.Year, _game.Player.VictoryPoints);
            ShowStarMap();
            fleetResupply = true;
        }


        public void CollectResources()
        {
            var systems = _game.Player.ConqueredSystems.ToList();
            systems.Add(_game.Player.Homeworld);
            systems.ForEach(o => o.AddResources());
            Stats(_game.Player.Military, _game.Player.Resources, _game.Player.Wealth, _game.Player.Year, _game.Player.VictoryPoints);
        }

        public void BuildMilitary()
        {
            if (_game.Player.Resources <= 0) { MessageBox.ErrorQuery("", "Your Highness, we do not have enough resources", "Ok"); return; }
            if (_game.Player.Wealth <= 0) { MessageBox.ErrorQuery("", "Your Highness, we do not have enough wealth", "Ok"); return; }
            if (_game.Player.OwnedTechs.Any(o => o.Name == Context.CapitalShips) == false && _game.Player.Military == 3) { MessageBox.ErrorQuery("", "Your Highness, we need capital ship technology needed to further improve our fleet", "Ok"); return; }
            _game.Player.Wealth--;
            _game.Player.Resources--;
            _game.Player.Military++;
            _game.Player.Military = Math.Min(_game.Player.Military, 5);
            Stats(_game.Player.Military, _game.Player.Resources, _game.Player.Wealth, _game.Player.Year, _game.Player.VictoryPoints);
            AllowMilitaryImprovement(false);
        }

        public void DiscoverTechnology(ITech technology)
        {
            if (_game.Player.Wealth < technology.Cost) MessageBox.ErrorQuery("", "Your Highness, we do not have enough wealth", "Ok");
            _game.Player.Wealth -= technology.Cost;
            _game.Player.OwnedTechs.Add(technology);
            Stats(_game.Player.Military, _game.Player.Resources, _game.Player.Wealth, _game.Player.Year, _game.Player.VictoryPoints);
            AllowTechImprovement(false);
        }


        public void EndTurn()
        {
            IncidentOccured();
            CollectResources();
            AllowMilitaryImprovement(true);
            AllowTechImprovement(true);
            fleetResupply = false;
        }



        public void ExchangeResources()
        {
            throw new NotImplementedException();
        }

        public void ExchangeWealth()
        {
            throw new NotImplementedException();
        }

        public void IncidentOccured()
        {
            if (_game.Incidents.Any() == false)
            {
                _game.Incidents = new Queue<Func<Empire, string>>(IncidentFactory.IncidentTemplateList.OrderBy(c => Guid.NewGuid()).ToList());
                _game.Player.Year++;
            }
            var incident = _game.Incidents.Dequeue();
            MessageBox.Query("Incident", incident(_game.Player), "Ok");
            Stats(_game.Player.Military, _game.Player.Resources, _game.Player.Wealth, _game.Player.Year, _game.Player.VictoryPoints);
            ShowStarMap();
        }
    }
}
