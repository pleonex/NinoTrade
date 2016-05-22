//
//  KeyDecoder.cs
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
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace NinoTrade.Decoder
{
    public static class KeyDecoder
    {
        private static readonly string Alphabet1 = "0123456789abcdefghijkmnpqrstuvwxyz" + 
            "ABCDEFGHJKLMNPQRSTUVWXYZ";
        private static readonly string[] Alphabet2 = { "!#", "$%", "&(", ")*", "+,", "-.",
            "/0", "12", "34", "56", "78", "9:", ";<", "=>", "?@", "AB", "CD", "EF", "GH",
            "IJ", "KL", "MN", "OP", "QR", "ST", "UV", "WX", "YZ", "[\\", "]_", "ab", "cd",
            "ef", "gh", "ij", "kl", "mn", "op", "qr", "st", "uv", "wx", "yz", "{|", "}~",
            "☆", "✭", "○", "●", "◎", "◇", "◆", "□", "■", "△", "▲", "▽", "▼" };

        public static int KeyLength { get { return 22; } }

        public static byte[] Decode(string input)
        {
            if (!IsValid(input))
                throw new ArgumentException("Invalid input length", "input");

            // From alphabet 2 to alphabet 1.
            input = Alphabet2ToAlphabet1(input);

            // From string to bytes.
            var key = TextToKey(input);

            // Swap bits.
            Swap(key);

            // Decryption 2.
            Encryption2(key);

            // Check CRCs
            var crcNew = CalculateCrc(key);
            var crcOld = GetCrc(key);
            if (crcNew != crcOld)
                throw new FormatException("CRC doesn't match.");

            // Remove CRC since it is not needed anymore
            RemoveCrc(ref key);

            // Decryption 1.
            Encryption(key);

            // Reverse bytes and return
            return key.Reverse().ToArray();
        }

        public static IEnumerable<string> ReadKey(string key)
        {
            int idx = 0;
            while (idx < key.Length) {
                char ch = key[idx++];
                yield return ch + (ch < 0x80 ? key[idx++].ToString() : "");
            }

            yield break;
        }

        public static bool IsValid(string key)
        {
            int count = 0;
            foreach (var symbol in ReadKey(key)) {
                int idx = Array.IndexOf(Alphabet2, symbol);

                // If the char is not in the alphabet return.
                if (idx == -1)
                    return false;

                // If it's a symbol count two, otherwise one.
                count++;
            }

            return count == KeyLength;
        }

        /// <summary>
        /// Convert the string from alphabet 2 representation to alphabet 1.
        /// </summary>
        /// <returns>The converted input in alphabet 1 format.</returns>
        /// <param name="input">Alphabet 2 input.</param>
        private static string Alphabet2ToAlphabet1(string input)
        {
            // The alphabet 2 that we are using is different from the original
            // jap games. This is because it was using kanjis and in this way
            // it is easier for user to write the input.
            // For this reason we are converting two chars into one.
            const int Step = 2;
            var alphabet1Key = new StringBuilder();

            foreach (string symbol in ReadKey(input)) {
                int idx = Array.IndexOf(Alphabet2, symbol);
                alphabet1Key.Append(Alphabet1[idx]);
            }

            return alphabet1Key.ToString();
        }

        /// <summary>
        /// Convert the text representation of a key into a list of bytes.
        /// </summary>
        /// <returns>The key in bytes.</returns>
        /// <param name="text">The text key.</param>
        private static byte[] TextToKey(string text)
        {
            const int SubKeyLength = 0x0B;
            const int BinKeyLength = 0x10;

            // Password is splitted into two substrings.
            var key = new byte[BinKeyLength];
            for (int i = 0; i < 2; i++) {
                // Get the subkey
                var subkeyText = text.Substring(i * SubKeyLength, SubKeyLength);

                // Iterate for each char to generate the 64-bit number.
                ulong subkeyNumber = 0;
                for (int j = 0; j < SubKeyLength; j++) {
                    ulong idx = (ulong)Alphabet1.IndexOf(subkeyText[j]);
                    subkeyNumber += idx * (ulong)Math.Pow(Alphabet1.Length, j);
                }

                // Convert the number into bytes.
                var subkeyBytes = BitConverter.GetBytes(subkeyNumber);
                Array.Copy(subkeyBytes, 0, key, i * 8, 8);
            }

            return key;
        }

        /// <summary>
        /// Swap a CRC byte part with another key value.
        /// </summary>
        /// <param name="key">Key to apply the swap.</param>
        private static void Swap(byte[] key)
        {
            // Game implementation of the indexes.
            // They are constants but it's funny to implement.
            uint rnd = 0x014A76E0;          // Init random number
            rnd = (rnd * 0x021FC436) + 1;   // Update random number
            uint idx = rnd % (uint)(key.Length - 2);    // Get swap index, it's ALWAYS 0xB
            int crcIdx = key.Length - 2;

            // Swap
            byte tmp = key[crcIdx];
            key[crcIdx] = key[idx];
            key[idx] = tmp;
        }

        /// <summary>
        /// Encrypt or decrypt with the second algorithm.
        /// </summary>
        /// <param name="key">Key to apply second encryption algorithm.</param>
        private static void Encryption2(byte[] key)
        {
            uint rnd = 0x05888F27u + GetCrc(key);
            for (int i = 0; i < key.Length - 2; i++) {
                rnd = (rnd * 0x021FC436) + 1;   // Update random number
                key[i] ^= (byte)(rnd >> 24);    // (D)Encrypt with the last byte
            }
        }

        /// <summary>
        /// Calculate the CRC-16-GENIBUS of the key.
        /// </summary>
        /// <returns>The CRC.</returns>
        /// <param name="key">Key to calculate CRC.</param>
        private static ushort CalculateCrc(byte[] key)
        {
            // Calculate the CRC of the key
            var crcData = Crc16Genibus(key, key.Length - 2);

            // Do it like the game because it's funny
            var constant = new byte[] { 
                0x5D, 0x10, 0x7A, 0x33, 0x00, 0x77, 0x13, 0x92, 0xDE};
            constant = constant.Reverse().ToArray();
            for (int i = 0; i < constant.Length; i++)
                constant[i] = (byte)~constant[i];
            var crcConstant = Crc16Genibus(constant, constant.Length);

            // Mix CRCs
            return (ushort)(crcConstant ^ crcData ^ 0x62D3);
        }

        /// <summary>
        /// Calculate the CRC-16-GENIBUS checksum.
        /// </summary>
        /// <returns>The GENIBUS CRC.</returns>
        /// <param name="data">Data to calculate checksum.</param>
        /// <param name="length">Length of the data.</param>
        private static ushort Crc16Genibus(IList<byte> data, int length)
        {
            // Parameters:
            // Width: 16
            // Poly: 0x11021
            // Init: 0xFFFF
            // RefIn: False
            // RefOut: False
            // XOR Out: 0xFFFF
            // Check: 0xD64E
            // Name: CRC-16/GENIBUS
            long crc = 0xFFFF;
            for (int i = 0; i < length; i++) {
                crc ^= data[i] << 8;
                for (int nbits = 8; nbits > 0; nbits--)
                    crc = ((crc & 0x8000) != 0 ? (crc << 1) ^ 0x1021 : crc << 1);
            }

            return (ushort)(crc ^ 0xFFFF);
        }

        /// <summary>
        /// Gets the CRC from the key bytes.
        /// </summary>
        /// <returns>The CRC.</returns>
        /// <param name="key">Key to get CRC.</param>
        private static ushort GetCrc(byte[] key)
        {
            return (ushort)((key[key.Length - 1] << 8) | key[key.Length - 2]);
        }

        /// <summary>
        /// Remove the CRC from the key.
        /// </summary>
        /// <param name="key">Key to remove CRC.</param>
        private static void RemoveCrc(ref byte[] key)
        {
            Array.Resize(ref key, key.Length - 2);
        }

        /// <summary>
        /// Encrypt and decrypt with the first algorithm.
        /// </summary>
        /// <param name="key">Key.</param>
        private static void Encryption(byte[] key)
        {
            var encryptionKey = key[0];
            for (int i = 1; i < key.Length; i++) {
                key[i] ^= encryptionKey;
                encryptionKey = (byte)((encryptionKey << 1) | (encryptionKey >> 7));
                encryptionKey ^= 0xFF;
            }
        }
    }
}

