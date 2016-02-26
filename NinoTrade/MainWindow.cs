//
//  MainWindow.cs
//
//  Author:
//       Benito Palacios Sánchez (aka pleonex) <benito356@gmail.com>
//
//  Copyright (c) 2016 Benito Palacios Sánchez
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using Xwt;
using Xwt.Drawing;

namespace NinoTrade
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            CreateComponents();
        }

        private void CreateComponents()
        {
            Width  = 750;
            Height = 400;
            Title  = string.Format("NinoTrade");

            var mainContent = new VBox();
            mainContent.BackgroundColor = Colors.White;

            var label = new Label {
                Text = "Introduce el código",
                MarginTop = 10,
                TextColor = Colors.SaddleBrown,
                Font = Font.SystemFont.WithSize(20).WithWeight(FontWeight.Bold)
            };
            mainContent.PackStart(label, false, WidgetPlacement.End, WidgetPlacement.Center);

            var textEntry = new TextEntry {
                HeightRequest = 50,
                MarginLeft = 10,
                MarginRight = 10,
                Font = Font.SystemFont.WithSize(20).WithWeight(FontWeight.Bold)
            };
            mainContent.PackStart(textEntry, false);


            mainContent.PackStart(new HBox() { HeightRequest = 200 }, true);

            Content = mainContent;

            Padding = new WidgetSpacing();
            CloseRequested += WindowCloseRequested;
        }

        private void WindowCloseRequested(object sender, CloseRequestedEventArgs args)
        {
            if (args.AllowClose)
                Application.Exit();
        }
    }
}

