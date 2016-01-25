using System.Collections.Generic;

namespace Battleships.Models
{
    public class Destroyer : Ship
    {
        public Destroyer()
        {
            NumberOfSquares = 4;
            Coordinates = new List<Coordinate>();
        }
    }
}
