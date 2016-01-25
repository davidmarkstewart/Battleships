using System;
using Battleships.Models;

namespace Battleships.Services.Extensions
{
    public static class StringExtensions
    {
        public static int ConvertLetterToNumber(this string input)
        {
            var c = Convert.ToChar(input);

            return char.ToUpper(c) - 64;
        }

        public static Coordinate ToCoordinate(this string input)
        {
            return new Coordinate
            {
                XAxis = input.Substring(0, 1).ConvertLetterToNumber(),
                YAxis = Convert.ToInt32(input.Substring(1))
            };
        }
    }
}