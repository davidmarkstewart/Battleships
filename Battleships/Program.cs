using Autofac;
using Battleships.Models;
using Battleships.Models.Interfaces;
using Battleships.Services;
using Battleships.Services.Interfaces;
using System;

namespace Battleships
{
    class Program
    {
        static void Main()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<GameService>().As<IGameService>();
            builder.RegisterType<ConfigurationManagerWrapper>().As<IConfigurationManager>();
            builder.RegisterType<Grid>().As<IGrid>();
            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var gameEngine = scope.Resolve<IGameService>();
                gameEngine.InitialiseGame();
                
                Console.WriteLine("Welcome to Battleships. Please enter a grid reference between a1 and j10.");

                while(!gameEngine.IsGameOver())
                {
                    var coordinateFiredAt = Console.ReadLine();

                    gameEngine.FireMissile(coordinateFiredAt);

                    OutputGameStatus(gameEngine.GetGameStatus());
                }

                Console.ReadKey();
            }
        }

        private static void OutputGameStatus(Enums.Missile gameStatus)
        {
            switch (gameStatus)
            {
                case Enums.Missile.Miss:
                    Console.WriteLine("Missed");
                    break;
                case Enums.Missile.Hit:
                    Console.WriteLine("Hit");
                    break;
                case Enums.Missile.ShipDestroyed:
                    Console.WriteLine("Hit and ship destroyed");
                    break;
                case Enums.Missile.AllShipsDestroyed:
                    Console.WriteLine("Hit and all ships destroyed");
                    break;
                case Enums.Missile.AlreadyFiredAt:
                    Console.WriteLine("You already fired at this coordinate");
                    break;
                case Enums.Missile.InvalidCoordinate:
                    Console.WriteLine("Please enter a valid coordinate");
                    break;
                default:
                    throw new ArgumentOutOfRangeException("gameStatus", gameStatus, null);
            }
        }
    }
}
