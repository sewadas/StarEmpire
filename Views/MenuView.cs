using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace StarEmpire
{
    public class MenuView : View
	{
        private MenuController _menuController;

        public MenuView(MenuController menuController)
		{
			_menuController = menuController;
			Width = Dim.Fill();
			Height = Dim.Fill();
			X = 0;
			Y = 0;
			var title = new Label("STAR EMPIRE!") { X = Pos.Center(), Y = 5 };

            var empireName = new Label("Your empire name:") { X = Pos.Center(), Y = Pos.Center() };
            var empireText = new TextField("Kurwix")
            {
                X = Pos.Left(empireName),
                Y = Pos.Top(empireName) + 2,
                Width = Dim.Width(empireName)
            };
            var startButton = new Button("Start", true) { X = Pos.Left(empireName), Y = Pos.Top(empireText) + 2, Width = Dim.Width(empireText), TextAlignment = TextAlignment.Justified };
			startButton.Clicked += () =>
			{
				_menuController.OnStartButtonClick(empireText.Text.ToString());
			};

			Add(title, empireName, empireText, startButton);
		}
	}
}
