//
//  FamiliarInfoConverter.cs
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
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace NinoTrade
{
    public static class FamiliarInfoConverter
    {
        private const string Prefix = "NinoTrade.Resources.";
        private static readonly Assembly MyAssembly = Assembly.GetExecutingAssembly();
        private static readonly List<string> FamiliarNames;

        static FamiliarInfoConverter()
        {
            var stream = MyAssembly.GetManifestResourceStream(Prefix + "ImagenParam.xml");
            var imagenParam = XDocument.Load(stream);

            FamiliarNames = new List<string>();
            foreach (var xmlName in imagenParam.Root.Elements("String"))
                FamiliarNames.Add(string.Format("{0} ({1})",
                    xmlName.Value,
                    xmlName.Attribute("Original").Value));

            stream.Dispose();
        }

        public static FamiliarInfo Convert(byte[] data)
        {
            var reader = new BitReader(data);
            var info = new FamiliarInfo();

            info.Name  = reader.ReadString(4).Replace("\0", "");    // Max: 4 chars
            info.Level = reader.ReadBits(7);                        // Max: 128
            info.Brand = (Brand)reader.ReadBits(3);                 // Max: 8
            info.InternalIndex = reader.ReadBits(10);               // Max: 1024
            info.Health  = reader.ReadBits(7) * 8;                  // Max: 128*8 = 1024
            info.Magic   = reader.ReadBits(7) * 8;                  // Max: 128*8 = 1024
            info.Attack  = reader.ReadBits(7) * 8;                  // Max: 128*8 = 1024
            info.Defense = reader.ReadBits(7) * 8;                  // Max: 128*8 = 1024
            info.MagicAttack  = reader.ReadBits(7) * 8;             // Max: 128*8 = 1024
            info.MagicDefense = reader.ReadBits(7) * 8;             // Max: 128*8 = 1024
            info.Ability = reader.ReadBits(7) * 8;                  // Max: 128*8 = 1024
            info.Family = FamiliarNames[info.InternalIndex];
            // Next 11 bits is the random number.

            return info;
        }
    }
}

