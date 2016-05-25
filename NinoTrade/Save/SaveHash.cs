//
//  SaveHash.cs
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
using System.IO;
using System.Security.Cryptography;

namespace NinoTrade.Save
{
    public static class SaveHash
    {
        private const ushort PasswordOffset = 0xC5EC;
        private static readonly byte[] Password = { 0x6E, 0x6B, 0x6E, 0x6E };
        private const ushort SaveLength = 0xFFE2;
        private const ushort SaveStart = 0x0002;
        private const ushort HashStart = 0xFFE4;
        private const ushort HashLength = 0x14;

        public static byte[] Compute(Stream stream)
        {
            SHA1 sha1 = SHA1.Create();

            // Read until password
            stream.Position = SaveStart;
            var buffer = new byte[PasswordOffset - SaveStart];
            stream.Read(buffer, 0, buffer.Length);
            sha1.TransformBlock(buffer, 0, buffer.Length, buffer, 0);

            // Add password
            sha1.TransformBlock(Password, 0, Password.Length, Password, 0);

            // Add the rest of the save
            buffer = new byte[HashStart - (PasswordOffset + Password.Length)];
            sha1.TransformFinalBlock(buffer, 0, buffer.Length);

            return sha1.Hash;
        }

        public static byte[] GetFromStream(Stream stream)
        {
            var hash = new byte[HashLength];
            stream.Position = HashStart;
            stream.Read(hash, 0, hash.Length);
            return hash;
        }
    }
}

