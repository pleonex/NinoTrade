//
//  GameSave.cs
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
using System.Collections.Generic;
using System.IO;
using NinoTrade.Familiar;

namespace NinoTrade.Save
{
    public class GameSave
    {
        private readonly Stream save;
        private readonly List<FamiliarInfo> refugees;
        private readonly List<FamiliarInfo> equip;

        public GameSave(string path)
        {
            save = File.OpenRead(path);
            refugees = new List<FamiliarInfo>();
            equip = new List<FamiliarInfo>();
        }

        ~GameSave()
        {
            save.Dispose();
        }

        public void Read()
        {
            // TODO: Check checksum.
            var reader = new BinaryReader(save);

            // First read the familiars in the equip. Skip if it's a player (index < 0x10)
            save.Position = 0x2828;
            var equipIdx = new List<ushort>();

            const int MaxEquipMembers = 3 + 9 + 9;
            for (int i = 0; i < MaxEquipMembers; i++) {
                ushort idx = reader.ReadUInt16();
                if (idx < 0x10)
                    continue;
                equipIdx.Add(idx);
            }

            // Read the information from the equip members.
            foreach (var idx in equipIdx) {
                save.Position = 0x29AC + 0x64 * (idx - 0x10);
                equip.Add(FamiliarInfoConverter.FromSave(save));
            }

            // Get indexes for refugio familiars.
            save.Position = 0xC5F0;
            var refugreesIdx = new List<ushort>();

            const int MaxRefugees = 0x190;
            for (int i = 0; i < MaxRefugees; i++) {
                ushort idx = reader.ReadUInt16();
                // If we found -1 then there aren't more familiars.
                if (idx == 0xFFFF)
                    break;
                // Skip the familiars already in the equip.
                if (equipIdx.Contains(idx))
                    continue;
                refugreesIdx.Add(idx);
            }

            // Read the information from the refugees members.
            foreach (var idx in refugreesIdx) {
                save.Position = 0x29AC + 0x64 * (idx - 0x10);
                refugees.Add(FamiliarInfoConverter.FromSave(save));
            }
        }

        public bool IsEquipFull()
        {
            return equip.Count >= 18;
        }

        public bool IsRefugeeFull()
        {
            return refugees.Count + equip.Count >= 0x190;
        }
    }
}

