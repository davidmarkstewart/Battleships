using System.Collections.Generic;

namespace Battleships.Models.Interfaces
{
    public interface IGrid
    {
        int MaxXAxis { get; set; }

        int MaxYAxis { get; set; }

        List<Ship> Ships { get; set; }

        List<Coordinate> CoordinatesShotAt { get; set; }

        bool IsValidCoordinate(string input);
    }
}
