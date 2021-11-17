using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace StarEmpire
{
    public class MenuController
    {
        public void OnStartButtonClick(string homeworldName)
        {
            Application.Top.RemoveAll();
            
            var game = new Game(homeworldName);
            var gameController = new GameController(game);
            var gameView = new GameView(gameController);
            gameController.ShowStarMap();
            Application.Top.Add(gameView);        
            Application.Run(); 
        }
    }
}
