using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace StarEmpire
{
    public struct Context
    {
        public const string Revolt = "Revolt";
        public const string Invasion = "Invasion";
        public const string RobotWorkers = "RobotWorkers";
        public const string InterstellarBanking = "InterstellarBanking";
        public const string HyperTelevision = "HyperTelevision";
        public const string PlanetaryDefenses = "PlanetaryDefenses";
        public const string ForwardStarbases = "ForwardStarbases";
        public const string InterstellarDiplomacy = "InterstellarDiplomacy";
        public const string CapitalShips = "CapitalShips";
        public const string InterspeciesCommerce = "InterspeciesCommerce";

        public static int RandomFactor => new Random((int)DateTime.UtcNow.Ticks).Next(1, 6);
        public static int RandomFactorGalaxyDistance => new Random().Next(7, 11);
        public static double RandomFactorGalaxyAngle => new Random().NextDouble();

        public static int ScreenWidth { get { Application.Top.GetCurrentWidth(out int width); return width; } }
        public static int ScreenHeight { get { Application.Top.GetCurrentHeight(out int height); return height; } }
    }
}
