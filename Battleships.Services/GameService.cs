using System;
using System.Collections.Generic;
using System.Linq;
using Battleships.Models;
using Battleships.Models.Interfaces;
using Battleships.Services.Extensions;
using Battleships.Services.Interfaces;

namespace Battleships.Services
{
    public class GameService : IGameService
    {
        private readonly IGrid _grid;

        public GameService(IGrid grid)
        {
            _grid = grid;
        }

        public void InitialiseGame()
        {
            SetShipsOrientation();
            SetShipsCoordinates();
        }

        public bool IsGameOver()
        {
            return LatestMove == Enums.Missile.AllShipsDestroyed;
        }

        public Enums.Missile GetGameStatus()
        {
            return LatestMove;
        }

        public void FireMissile(string coordinateFireAt) 
        {
            if (!_grid.IsValidCoordinate(coordinateFireAt))
            {
                LatestMove = Enums.Missile.InvalidCoordinate;
            }
            else
            {
                var coordinates = coordinateFireAt.ToCoordinate();

                if (HasCoordinateAlreadyBeenShotAt(coordinates))
                {
                    LatestMove = Enums.Missile.AlreadyFiredAt;
                }
                else
                {
                    LatestMove = Enums.Missile.Miss;

                    _grid.CoordinatesShotAt.Add(coordinates);

                    foreach (var ship in _grid.Ships)
                    {
                        foreach (var thisCoordinate in ship.Coordinates)
                        {
                            if (thisCoordinate.XAxis == coordinates.XAxis && thisCoordinate.YAxis == coordinates.YAxis)
                            {
                                thisCoordinate.IsHit = true;

                                if (ship.IsDestroyed())
                                {
                                    if (IsAllShipsDestroyed())
                                    {
                                        LatestMove = Enums.Missile.AllShipsDestroyed;
                                    }
                                    else
                                    {
                                        LatestMove = Enums.Missile.ShipDestroyed;
                                    }
                                }
                                else
                                {
                                    LatestMove = Enums.Missile.Hit;
                                }
                            }
                        }
                    }
                }
            }
        }

        private bool IsAllShipsDestroyed()
        {
            return _grid.Ships.All(ship => ship.Coordinates.All(coordinate => coordinate.IsHit));
        }

        private bool HasCoordinateAlreadyBeenShotAt(Coordinate shotAtCoordinate)
        {
            return _grid.CoordinatesShotAt.Any(coordinate => coordinate.XAxis == shotAtCoordinate.XAxis && coordinate.YAxis == shotAtCoordinate.YAxis);
        }

        private void SetShipsOrientation()
        {
            var random = new Random();

            _grid.Ships.ForEach(x => x.Orientation = (Enums.Orientation)random.Next(2));
        }

        private void SetShipsCoordinates()
        {
            var random = new Random();

            var xAxisMaxStartingPosition = _grid.MaxXAxis;
            var yAxisMaxStartingPosition = _grid.MaxYAxis;

            foreach (var ship in _grid.Ships)
            {
                if (ship.Orientation == Enums.Orientation.Horizontal)
                {
                    xAxisMaxStartingPosition = (_grid.MaxXAxis - ship.NumberOfSquares) + 1;
                }
                else
                {
                    yAxisMaxStartingPosition = (_grid.MaxYAxis - ship.NumberOfSquares) + 1;
                }

                while (ship.Coordinates.Count == 0)
                {
                    var yAxis = random.Next(1, yAxisMaxStartingPosition);
                    var xAxis = random.Next(1, xAxisMaxStartingPosition);

                    var coordinates = BuildCoordinatesList(ship, yAxis, xAxis);

                    if (IsCoordinatesAvailable(coordinates))
                    {
                        ship.Coordinates = coordinates;
                    }
                }
            }
        }

        private bool IsCoordinatesAvailable(IEnumerable<Coordinate> coordinates)
        {
            return _grid.Ships.All(ship => !ship.Coordinates.Any(coordinate => coordinates.Any(c => c.XAxis == coordinate.XAxis && c.YAxis == coordinate.YAxis)));
        }

        private List<Coordinate> BuildCoordinatesList(Ship ship, int yAxis, int xAxis)
        {
            var coordinates = new List<Coordinate>();

            if (ship.Orientation == Enums.Orientation.Horizontal)
            {
                for (var i = 0; i < ship.NumberOfSquares; i++)
                {
                    coordinates.Add(new Coordinate { XAxis = xAxis + i, YAxis = yAxis });
                }
            }
            else
            {
                for (var i = 0; i < ship.NumberOfSquares; i++)
                {
                    coordinates.Add(new Coordinate { XAxis = xAxis, YAxis = yAxis + i });
                }
            }

            return coordinates;
        }

        public Enums.Missile LatestMove { get; set; }
    }
}
