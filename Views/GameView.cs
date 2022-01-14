using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace StarEmpire
{
    public class GameView : View
    {
        private GameController _gameController;
        private View stats;
        private View military;
        private View resources;
        private View wealth;
        private View starMap;
        private View year;
        private View vp;
        private Button endTurn;
        private Button improveMilitary;
        private Button tech;
        private Button improveResources;
        private Button improveWealth;

        public GameView(GameController gameController)
        {
            _gameController = gameController;
            _gameController.Stats = OnStatsRefresh;
            _gameController.StarMap = OnStarMapRefresh;
            _gameController.AllowMilitaryImprovement = OnMilitaryImprovement;
            _gameController.AllowTechImprovement = OnTechImprovement;
            _gameController.AllowResourcesExchange = OnResourcesExchange;
            _gameController.AllowWealthExchange = OnWealthExchange;
            _gameController.Techs = OnTechView;
            Height = Dim.Fill();
            Width = Dim.Fill();
            stats = new FrameView() { Height = 9, Width = 40, X = 0, Y = 0 };
            military = new Label($"Military: {0}") { X = 1, Y = 1, Width = 12, Height = 1 };
            improveMilitary = new Button("+1") { X = Pos.Right(military) + 1, Y = 1, Width = 6, Height = 1 };
            improveMilitary.Clicked += _gameController.BuildMilitary;
            resources = new Label($"Resources: {0}") { X = 1, Y = Pos.Bottom(military), Width = 12, Height = 1 };
            improveResources = new Button("+1") { X = Pos.Right(resources) + 1, Y = Pos.Bottom(military), Width = 6, Height = 1, Enabled = false };
            improveResources.Clicked += _gameController.ExchangeResources;
            wealth = new Label($"Wealth: {0}") { X = 1, Y = Pos.Bottom(resources), Width = 12, Height = 1 };
            improveWealth = new Button("+1") { X = Pos.Right(wealth) + 1, Y = Pos.Bottom(resources), Width = 6, Height = 1, Enabled = false };
            improveWealth.Clicked += _gameController.ExchangeWealth;
            year = new Label($"Era: {1} Turn: {1}") { X = 1, Y = Pos.Bottom(wealth), Width = 12, Height = 1 };
            vp = new Label($"VP:  {0}") { X = 1, Y = Pos.Bottom(year), Width = 12, Height = 1 };

            endTurn = new Button("END TURN") { X = Pos.Right(improveMilitary) + 2, Y = 1, Width = 12, Height = 1 };
            endTurn.Clicked += _gameController.EndTurn;
            tech = new Button("TECH") { X = Pos.Right(improveMilitary) + 2, Y = Pos.Bottom(endTurn) + 2, Width = 12, Height = 1, TextAlignment = TextAlignment.Centered };
            tech.Clicked += _gameController.ShowTech;
            
            stats.Add(military, improveMilitary, resources, improveResources, wealth, improveWealth, year, vp, endTurn, tech);

            starMap = new FrameView() { Height = Dim.Fill(), Width = Dim.Fill(), X = 0, Y = 0 };

            Add(starMap, stats);
        }

        public void OnMilitaryImprovement(bool canImprove)
        {
            Application.MainLoop.Invoke(() => 
            {
                this.improveMilitary.Enabled = canImprove;
            });
        }

        public void OnTechImprovement(bool canImprove)
        {
            Application.MainLoop.Invoke(() => 
            {
                this.tech.Enabled = canImprove;
            });
        }

        public void OnResourcesExchange(bool canExchange)
        {
            Application.MainLoop.Invoke(() =>
            {
                this.improveResources.Enabled = canExchange;
            });
        }

        public void OnWealthExchange(bool canExchange)
        {
            Application.MainLoop.Invoke(() =>
            {
                this.improveWealth.Enabled = canExchange;
            });
        }

        public void OnStatsRefresh(Empire empire)
        {
            Application.MainLoop.Invoke(() =>
            {
                this.military.Text = $"Military: {empire.Military}";
                this.resources.Text = $"Resources: {empire.Resources}";
                this.wealth.Text = $"Wealth: {empire.Wealth}";
                this.year.Text = $"Era: {empire.Era} Turn: {empire.Turn}";
                this.vp.Text = $"VP: {empire.VictoryPoints}";
            });
        }

        public void OnStarMapRefresh(List<IStarSystem> stars)
        {
            Application.MainLoop.Invoke(() =>
            {
                starMap.RemoveAll();
                for (int i = 0; i < stars.Count; i++)
                {
                    var star = stars[i];
                    double distance = i + 5;
                    double angle = i * (2 * Math.PI) / (stars.Count + 2);

                    Pos x = star.LocationX;
                    Pos y = star.LocationY;
                    var system = new Label() { X = x, Y = y, Text = $"* {(star.IsExplored || star.IsHomeworld ? star.Name : string.Empty)}", Data = star, Width = star.Name.Length + 2, Height = 1 };
                    if (star.IsHomeworld) system.ColorScheme = new ColorScheme() { Normal = new Terminal.Gui.Attribute(Color.Green) };
                    if (star.Owner == null) system.ColorScheme = new ColorScheme() { Normal = new Terminal.Gui.Attribute(Color.Red), Focus = new Terminal.Gui.Attribute(Color.BrightGreen) };
                    if (star.IsExplored == false) system.ColorScheme = new ColorScheme() { Normal = new Terminal.Gui.Attribute(Color.Gray), Focus = new Terminal.Gui.Attribute(Color.BrightGreen) };
                    if (star.Distance == DistanceEnum.Distant) system.ColorScheme = new ColorScheme() { Normal = new Terminal.Gui.Attribute(Color.DarkGray), Focus = new Terminal.Gui.Attribute(Color.BrightGreen) };
                    system.Clicked += () => _gameController.ExploreOrInvade(star);
                    starMap.Add(system);
                }
            });
        }

        public void OnTechView(List<ITech> available)
        {
            Application.MainLoop.Invoke(() =>
            {
                var dialog = new Dialog() { X = Pos.Center(), Y = Pos.Center() };
                var info = new Label() { Y = 12, X = Pos.Center(), Width = 30 };
                var techs = new ListView(available) { Height = 12, Width = 50 } ;
                techs.SelectedItemChanged += f => info.Text = ((ITech)f.Value).Description;

                var cancel = new Button("Cancel");
                cancel.Clicked += () => Remove(dialog);
                var buy = new Button("Buy");
                buy.Clicked += () => { _gameController.DiscoverTechnology(available[techs.SelectedItem]); Remove(dialog); };
                dialog.Add(techs);
                dialog.Add(info);
                dialog.AddButton(buy);
                dialog.AddButton(cancel);
                Add(dialog);
            });
        }
    }
}
