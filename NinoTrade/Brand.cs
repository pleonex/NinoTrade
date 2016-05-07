//
//  Brand.cs
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
    public enum Brand
    {
        Sun        = 0,
        Moon       = 1,
        Star       = 2,
        Planet     = 3,
        TwinSun    = 4,
        TwinMoon   = 5,
        TwinStar   = 6,
        TwinPlanet = 7
    }

    public static class BrandExtensions
    {
        public static string ToName(this Brand brand)
        {
            switch (brand) {
            case Brand.Sun:        return "Sol";
            case Brand.Moon:       return "Luna";
            case Brand.Star:       return "Estrella";
            case Brand.Planet:     return "Planeta";
            case Brand.TwinSun:    return "Soles gemelos";
            case Brand.TwinMoon:   return "Lunas gemelas";
            case Brand.TwinStar:   return "Estrellas gemelas";
            case Brand.TwinPlanet: return "Planetas gemelos";
            default:               return "Desconocido";
            }
        }
    }
}

