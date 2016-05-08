//
//  FamiliarInfoConveter.cs
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
    public static class FamiliarInfoConveter
    {
        public static FamiliarInfo Convert(byte[] data)
        {
            var reader = new BitReader(data);
            var info = new FamiliarInfo();

            info.Name = reader.ReadString(4).Replace("\0", "");
            info.Level = reader.ReadBits(7);
            info.Brand = (Brand)reader.ReadBits(3);
            info.InternalIndex = reader.ReadBits(10);
            info.Health = reader.ReadBits(7) * 8;
            info.Magic = reader.ReadBits(7) * 8;
            info.Attack = reader.ReadBits(7) * 8;
            info.Defense = reader.ReadBits(7) * 8;
            info.MagicAttack = reader.ReadBits(7) * 8;
            info.MagicDefense = reader.ReadBits(7) * 8;
            info.Ability = reader.ReadBits(7) * 8;

            return info;
        }
    }
}

