using System.Collections.Generic;
using System.Collections.Specialized;
using Battleships.Models;
using Battleships.Models.Interfaces;
using Battleships.Services;
using Moq;
using NUnit.Framework;

namespace Battleships.Tests
{
    [TestFixture]
    public class BattleshipTest
    {
        private Battleship _validBattleship;
        private Destroyer _validDestroyer;

        Mock<IConfigurationManager> _mockConfigurationManger;

        [SetUp]
        public void Set_Up_Game_Configuration()
        {
            _mockConfigurationManger = new Mock<IConfigurationManager>();
            _mockConfigurationManger.Setup(x => x.AppSettings).Returns(new NameValueCollection
                {
                    { "MaxYAxis", "10" },
                    { "MaxXAxis", "10" }
                }
            );

            _validBattleship = new Battleship
            {
                Coordinates = new List<Coordinate>
                {
                    new Coordinate { XAxis = 1, YAxis = 1 },
                    new Coordinate { XAxis = 2, YAxis = 1 },
                    new Coordinate { XAxis = 3, YAxis = 1 },
                    new Coordinate { XAxis = 4, YAxis = 1 },
                    new Coordinate { XAxis = 5, YAxis = 1 }
                }
            };

            _validDestroyer = new Destroyer
            {
                Coordinates = new List<Coordinate>
                {
                    new Coordinate { XAxis = 1, YAxis = 2 },
                    new Coordinate { XAxis = 2, YAxis = 2 },
                    new Coordinate { XAxis = 3, YAxis = 2 },
                    new Coordinate { XAxis = 4, YAxis = 2 },
                    new Coordinate { XAxis = 5, YAxis = 2 }
                }
            };
        }

        [Test]
        [Category("Missile Fired")]
        public void Can_Hit_Ship()
        {
            var grid = new Grid(_mockConfigurationManger.Object);

            grid.Ships.Add(_validBattleship);

            var gameService = new GameService(grid);

            const Enums.Missile expectedResult = Enums.Missile.Hit;

            gameService.FireMissile("a1");

            Assert.AreEqual(expectedResult, gameService.GetGameStatus());
        }

        [Test]
        [Category("Missile Fired")]
        public void Can_Miss_Ship()
        {
            var grid = new Grid(_mockConfigurationManger.Object);

            grid.Ships.Add(_validBattleship);

            var gameService = new GameService(grid);

            const Enums.Missile expectedResult = Enums.Missile.Miss;

            gameService.FireMissile("a6");

            Assert.AreEqual(expectedResult, gameService.GetGameStatus());
        }

        [Test]
        [Category("Missile Fired")]
        public void Can_Sink_Ship()
        {
            var grid = new Grid(_mockConfigurationManger.Object);

            grid.Ships.Add(_validBattleship);
            grid.Ships.Add(_validDestroyer);

            var gameService = new GameService(grid);

            const Enums.Missile expectedResult = Enums.Missile.ShipDestroyed;

            gameService.FireMissile("a1");
            gameService.FireMissile("b1");
            gameService.FireMissile("c1");
            gameService.FireMissile("d1");
            gameService.FireMissile("e1");

            Assert.AreEqual(expectedResult, gameService.GetGameStatus());
        }

        [Test]
        [Category("Missile Fired")]
        public void Can_Sink_Final_Ship_And_Finish_Game()
        {
            var grid = new Grid(_mockConfigurationManger.Object);

            grid.Ships.Add(_validBattleship);

            var gameService = new GameService(grid);

            const Enums.Missile expectedResult = Enums.Missile.AllShipsDestroyed;

            gameService.FireMissile("a1");
            gameService.FireMissile("b1");
            gameService.FireMissile("c1");
            gameService.FireMissile("d1");
            gameService.FireMissile("e1");

            Assert.AreEqual(expectedResult, gameService.GetGameStatus());
        }

        [Test]
        [Category("Missile Fired")]
        public void Can_Not_Fired_At_The_Same_Coordinates_Twice()
        {
            var grid = new Grid(_mockConfigurationManger.Object);

            grid.Ships.Add(_validBattleship);

            var gameService = new GameService(grid);

            const Enums.Missile expectedResult = Enums.Missile.AlreadyFiredAt;

            gameService.FireMissile("a1");
            gameService.FireMissile("a1");

            Assert.AreEqual(expectedResult, gameService.GetGameStatus());
        }

        [TestCase("p5")]
        [TestCase("")]
        [TestCase("foo")]
        [TestCase("12345")]
        [Category("Input Validation")]
        public void Can_Not_Enter_Invalid_Grid_References(string gridReference)
        {
            var grid = new Grid(_mockConfigurationManger.Object);

            var gameService = new GameService(grid);

            const Enums.Missile expectedResult = Enums.Missile.InvalidCoordinate;

            gameService.FireMissile(gridReference);

            Assert.AreEqual(expectedResult, gameService.GetGameStatus());
        }
    }
}
