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
            year = new Label($"Year: {1}") { X = 1, Y = Pos.Bottom(wealth), Width = 12, Height = 1 };
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
            Application.MainLoop.Invoke(() => {
                this.improveMilitary.Enabled = canImprove;
            });
        }

        public void OnStatsRefresh(int military, int resources, int wealth, int year, int vp)
        {
            Application.MainLoop.Invoke(() => {
                this.military.Text = $"Military: {military}";
                this.resources.Text = $"Resources: {resources}";
                this.wealth.Text = $"Wealth: {wealth}";
                this.year.Text = $"Year: {year}";
                this.vp.Text = $"VP: {vp}";
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

                    Pos x = star.IsHomeworld ? Pos.Center() : Pos.Center() + (int)(Math.Sin(angle) * distance);
                    Pos y = star.IsHomeworld ? Pos.Center() : Pos.Center() + (int)(Math.Cos(angle) * distance);
                    var system = new Label() { X = x, Y = y, Text = $"* {(star.IsExplored || star.IsHomeworld ? star.Name : string.Empty)}", Data = star, Width = star.Name.Length +2, Height = 1 };
                    if (star.IsHomeworld) system.ColorScheme = new ColorScheme() { Normal = new Terminal.Gui.Attribute(Color.Green) };
                    if (star.Owner == null) system.ColorScheme = new ColorScheme() { Normal = new Terminal.Gui.Attribute(Color.Red) };
                    if (star.IsExplored == false) system.ColorScheme = new ColorScheme() { Normal = new Terminal.Gui.Attribute(Color.Gray) };
                    if (star.Distance == DistanceEnum.Distant) system.ColorScheme = new ColorScheme() { Normal = new Terminal.Gui.Attribute(Color.Blue) };
                    system.Clicked += () => _gameController.ExploreOrInvade(star);
                    starMap.Add(system);
                }
            });
        }

        public void OnTechView(List<ITech> owned, List<ITech> available)
        {
            Application.MainLoop.Invoke(() =>
            {
                var dialog = new Dialog() { X = Pos.Center(), Y = Pos.Center() };
                dialog.AddButton(new Button("OK"));
                Add(dialog);
            });
        }
    }
}
