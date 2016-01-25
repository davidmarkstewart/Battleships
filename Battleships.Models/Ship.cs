using System.Collections.Generic;
using System.Linq;

namespace Battleships.Models
{
    public abstract class Ship
    {
        public int NumberOfSquares { get; set; }

        public Enums.Orientation Orientation { get; set; }

        public List<Coordinate> Coordinates { get; set; }

        public bool IsDestroyed()
        {
            return Coordinates.Count(x => !x.IsHit) == 0;
        }
    }
}
