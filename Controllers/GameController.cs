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
        public Action<Empire> Stats { get; set; }
        public Action<List<IStarSystem>> StarMap { get; set; }
        public Action<bool> AllowMilitaryImprovement { get; set; }
        public Action<bool> AllowTechImprovement { get; set; }
        public Action<bool> AllowResourcesExchange { get; set; }
        public Action<bool> AllowWealthExchange { get; set; }
        public Action<List<ITech>> Techs { get; set; }
        public bool fleetResupply { get; set; } = false;
        public bool diplomacyAvailable { get; set; } = false;

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
            Techs(TechFactory.TechTemplateList.Where(o => o.RequiresYear <= _game.Player.Era && _game.Player.OwnedTechs.Exists(p => p.Name == o.Name) == false).ToList());
        }

        public void ExploreOrInvade(IStarSystem star)
        {
            if (diplomacyAvailable)
            {
                int diplomaticJoin = MessageBox.Query("Exploration", $"Your Highness, do you want to join new star system?", "Yes", "No");
                if (diplomaticJoin != 0) return;
                MessageBox.Query("Invasion", $"Your Highness, star system {star.Name} joined our Empire.", "Ok");
                star.Owner = _game.Player;
                _game.Player.ConqueredSystems.Add(star);
                diplomacyAvailable = false;
                Stats(_game.Player);
                ShowStarMap();
                return;
            }
            
            if (_game.Player.ConqueredSystems.Contains(star))
            {
                MessageBox.Query("Information", $"{star.Name} {Environment.NewLine} Resistance: {star.Resistance} Resources: {star.ResourceRate} Wealth: {star.WealthRate} VP: {star.VictoryPoints}", "Ok");
                return;
            }

            if (fleetResupply)
            {
                MessageBox.ErrorQuery("Information", "Your Highness, our fleet is underway replenishment.", "Ok");
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
            Stats(_game.Player);
            ShowStarMap();
            fleetResupply = true;
        }


        public void CollectResources()
        {
            var systems = _game.Player.ConqueredSystems.ToList();
            systems.Add(_game.Player.Homeworld);
            systems.ForEach(o => o.AddResources());
            Stats(_game.Player);
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
            Stats(_game.Player);
            AllowMilitaryImprovement(false);
        }

        public void DiscoverTechnology(ITech technology)
        {
            if (_game.Player.Wealth < technology.Cost) { MessageBox.ErrorQuery("", "Your Highness, we do not have enough wealth", "Ok"); return; }
            _game.Player.Wealth -= technology.Cost;
            _game.Player.OwnedTechs.Add(technology);
            if (technology.Name == Context.InterstellarDiplomacy) diplomacyAvailable = true;
            Stats(_game.Player);
            AllowTechImprovement(false);
        }

        public void EndTurn()
        {
            IncidentOccured();
            CollectResources();
            AllowMilitaryImprovement(true);
            AllowTechImprovement(true);
            if (_game.Player.OwnedTechs.Any(o => o.Name == Context.InterspeciesCommerce))
            {
                AllowResourcesExchange(true);
                AllowWealthExchange(true);
            }
            fleetResupply = false;
            _game.Player.Turn++;
        }

        public void ExchangeResources()
        {
            if (_game.Player.Wealth < 2) { MessageBox.ErrorQuery("", "Your Highness, we do not have enough wealth", "Ok"); return; }
            if (_game.Player.OwnedTechs.Any(o => o.Name == Context.InterspeciesCommerce) == false) { MessageBox.ErrorQuery("", "Your Highness, we need interspecies commerce technology needed to exchange wealth", "Ok"); return; }
            _game.Player.Wealth -= 2;
            _game.Player.Resources++;
            Stats(_game.Player);
            AllowResourcesExchange(false);
        }

        public void ExchangeWealth()
        {
            if (_game.Player.Resources < 2) { MessageBox.ErrorQuery("", "Your Highness, we do not have enough resources", "Ok"); return; }
            if (_game.Player.OwnedTechs.Any(o => o.Name == Context.InterspeciesCommerce) == false) { MessageBox.ErrorQuery("", "Your Highness, we need interspecies commerce technology needed to exchange resources", "Ok"); return; }
            _game.Player.Resources -= 2;
            _game.Player.Wealth++;
            Stats(_game.Player);
            AllowWealthExchange(false);
        }

        public void IncidentOccured()
        {
            if (_game.Incidents.Any() == false)
            {
                _game.Incidents = new Queue<Func<Empire, string>>(IncidentFactory.IncidentTemplateList.OrderBy(c => Guid.NewGuid()).ToList());
                _game.Player.Era++;
                if (_game.Player.Era >= 3)
                {
                    Application.Top.RemoveAll();
                    int res = MessageBox.Query("Victory!", $"Your score: {_game.Player.VictoryPoints}", "Ok");
                    Application.Top.Add(new MenuView(new MenuController()));
                    return;
                }
            }
            var incident = _game.Incidents.Dequeue();
            MessageBox.Query("Incident", incident(_game.Player), "Ok");
            Stats(_game.Player);
            ShowStarMap();
        }
    }
}
