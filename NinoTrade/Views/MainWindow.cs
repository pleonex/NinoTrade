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
using NinoTrade.Decoder;
using NinoTrade.Familiar;
using NinoTrade.Save;

namespace NinoTrade.Views
{
    public class MainWindow : Window
    {
        private FamiliarInfoView familiarInfoView;
        private Button importButton;
        private Button validateButton;
        private TextEntry textCode;

        public MainWindow()
        {
            CreateComponents();
        }

        private void CreateComponents()
        {
            Width  = 750;
            Height = 350;
            Title  = "NinoTrade ~~ GradienWords";

            var mainContent = new VBox();
            mainContent.BackgroundColor = Colors.White;

            var label = new Label {
                Text = "NinoTrade",
                MarginTop = 10,
                TextColor = Colors.SaddleBrown,
                Font = Font.SystemFont.WithSize(20).WithWeight(FontWeight.Bold)
            };
            mainContent.PackStart(label, false, hpos: WidgetPlacement.Center);

            textCode = new TextEntry {
                HeightRequest = 50,
                MarginLeft = 10,
                MarginRight = 10,
                Font = Font.SystemFont.WithSize(20).WithWeight(FontWeight.Bold),
                PlaceholderText = "Escribe aquí el código del únimo..."
            };
            mainContent.PackStart(textCode, false);

            // Symbol buttons
            var symbolButtons = new HBox();
            symbolButtons.MarginLeft = 10;
            symbolButtons.MarginRight = 10;
            symbolButtons.PackStart(CreateSymbolButton("☆"));
            symbolButtons.PackStart(CreateSymbolButton("✭"));
            symbolButtons.PackStart(CreateSymbolButton("○"));
            symbolButtons.PackStart(CreateSymbolButton("●"));
            symbolButtons.PackStart(CreateSymbolButton("◎"));
            symbolButtons.PackStart(CreateSymbolButton("◇"));
            symbolButtons.PackStart(CreateSymbolButton("◆"));
            symbolButtons.PackStart(CreateSymbolButton("□"));
            symbolButtons.PackStart(CreateSymbolButton("■"));
            symbolButtons.PackStart(CreateSymbolButton("△"));
            symbolButtons.PackStart(CreateSymbolButton("▲"));
            symbolButtons.PackStart(CreateSymbolButton("▽"));
            symbolButtons.PackStart(CreateSymbolButton("▼"));
            mainContent.PackStart(symbolButtons, false, hpos: WidgetPlacement.Center);

            // Unimo stats and import button
            var middleBox = new HBox { HeightRequest = 250 };
            middleBox.MarginLeft = 10;
            middleBox.MarginRight = 10;
            middleBox.MarginTop = 20;

            var verticalBox = new VBox();
            validateButton = new Button("Validar");
            validateButton.HeightRequest = 50;
            validateButton.Clicked += ValidateClicked;
            verticalBox.PackStart(validateButton, true, WidgetPlacement.Center);

            importButton = new Button("¡Canjear!");
            importButton.Sensitive = false;
            importButton.HeightRequest = 50;
            importButton.Clicked += ImportClicked;
            verticalBox.PackStart(importButton, true, WidgetPlacement.Start);

            middleBox.PackStart(verticalBox, true);
            familiarInfoView = new FamiliarInfoView();
            middleBox.PackStart(familiarInfoView, true, hpos: WidgetPlacement.End);
            mainContent.PackStart(middleBox, false);

            Content = mainContent;
            Padding = new WidgetSpacing();
            CloseRequested += WindowCloseRequested;
        }

        private Button CreateSymbolButton(string symbol)
        {
            var button = new Button(symbol);
            button.WidthRequest = 35;
            button.Clicked += delegate {
                textCode.Text += symbol;
                textCode.SetFocus();
                textCode.SelectionStart = textCode.Text.Length;
                textCode.SelectionLength = 0;
            };
            return button;
        }

        private void ValidateClicked(object sender, EventArgs e)
        {
            try {
                var code = KeyDecoder.Decode(textCode.Text);
                var info = FamiliarInfoConverter.FromKey(code);
                familiarInfoView.Info = info;
                importButton.Sensitive = true;
            } catch (Exception ex) {
                importButton.Sensitive = false;
                MessageDialog.ShowError("Código inválido",
                    "Error al descifrar la clave. Revisa el código.\n" + ex.Message);
            }
        }

        private void ImportClicked(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog("Selecciona el archivo de partida (save)");
            if (!dialog.Run(this))
                return;

            var save = new GameSave(dialog.FileName);
            save.Read();

            if (save.IsRefugeeFull()) {
                MessageDialog.ShowError(
                    "Refugio lleno",
                    "El refugio de únimos está lleno. No se puede añadir más únimos.");
                return;
            } else if (save.IsEquipFull()) {
                MessageDialog.ShowMessage(
                    "Equipo completo",
                    "Tu equipo está completo, añadiendo al refugio.");
            }

            MessageDialog.ShowMessage("Nada por ahora");
        }

        private void WindowCloseRequested(object sender, CloseRequestedEventArgs args)
        {
            if (args.AllowClose)
                Application.Exit();
        }
    }
}

