//
//  FamiliarInfo.cs
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
namespace NinoTrade
{
    public struct FamiliarInfo
    {
        public string Name { get; set; }
        public int Level   { get; set; }
        public Brand Brand { get; set; }
        public int InternalIndex { get; set; }

        public int Health  { get; set; }
        public int Magic   { get; set; }
        public int Attack  { get; set; }
        public int Defense { get; set; }
        public int MagicAttack  { get; set; }
        public int MagicDefense { get; set; }
        public int Ability { get; set; }
    }
}

