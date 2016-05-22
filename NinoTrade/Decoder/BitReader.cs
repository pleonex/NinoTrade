//
//  BitReader.cs
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
using System.Text;


namespace NinoTrade.Decoder
{
    public class BitReader
    {
        private readonly byte[] data;

        public BitReader(byte[] data)
        {
            this.data = data;
            Position = 0;
        }

        public int Position { get; private set; }

        public byte[] GetData()
        {
            return (byte[])data.Clone();
        }

        public int ReadBit()
        {
            int byteIdx = Position / 8;
            int bitIdx = Position % 8;

            Position++;
            return (data[byteIdx] >> bitIdx) & 1;
        }

        public int ReadBits(int numBits)
        {
            int value = 0;
            for (int i = 0; i < numBits; i++)
                value |= ReadBit() << i;
            return value;
        }

        public byte ReadByte()
        {
            return (byte)ReadBits(8);
        }

        public string ReadString(int numCharacters)
        {
            var text = new StringBuilder();
            for (int i = 0; i < numCharacters; i++)
                text.Append((char)ReadByte());
            return text.ToString();
        }
    }
}

