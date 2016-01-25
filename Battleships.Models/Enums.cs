namespace Battleships.Models
{
    public class Enums
    {
        public enum Orientation
        {
            Horizontal,
            Vertical
        }

        public enum Missile
        {
            Miss,
            Hit,
            ShipDestroyed,
            AllShipsDestroyed,
            AlreadyFiredAt,
            InvalidCoordinate
        }
    }
}
