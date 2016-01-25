using System.Collections.Generic;

namespace Battleships.Models
{
    public class Battleship : Ship
    {
        public Battleship()
        {
            NumberOfSquares = 5;
            Coordinates = new List<Coordinate>();
        }
    }
}
