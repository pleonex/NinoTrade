//
//  FamiliarInfoView.cs
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

namespace NinoTrade
{
    public class FamiliarInfoView : MarkdownView
    {
        private FamiliarInfo info;
        private const string TextFormat = @"
# Información del únimo
1. **Nombre**: {0}  
2. **Nivel**: {1}  
3. **Naturaleza**: {2}  
4. **Familia**: {3}  
5. **Estadísticas**:  
  1. **PV**: {4}  
  2. **PM**: {5}  
  3. **Ataque**: {6}  
  4. **Defensa**: {7}  
  5. **Ataque mágico**: {8}  
  6. **Defensa mágica**: {9}  
  7. **Habilidad**: {10}";

        public FamiliarInfoView()
        {
            CreateComponents();
        }

        private void CreateComponents()
        {
            WidthRequest = 300;
            UpdateView();
        }

        public FamiliarInfo Info {
            get { return info; }
            set {
                info = value;
                UpdateView();
            }
        }

        public void UpdateView()
        {
            Markdown = string.Format(TextFormat, 
                info.Name,
                info.Level,
                info.Brand.ToName(),
                info.Family,
                info.Health,
                info.Magic,
                info.Attack,
                info.Defense,
                info.MagicAttack,
                info.MagicDefense,
                info.Ability);
        }
    }
}

