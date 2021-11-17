using System;
using Terminal.Gui;

namespace StarEmpire
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.Init();
            var top = Application.Top;
            top.Add(new MenuView(new MenuController()));
            Application.Run();
        }
    }
}
