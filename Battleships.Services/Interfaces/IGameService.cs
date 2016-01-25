using Battleships.Models;

namespace Battleships.Services.Interfaces
{
    public interface IGameService
    {
        void InitialiseGame();

        bool IsGameOver();

        void FireMissile(string coordinate);

        Enums.Missile GetGameStatus();
    }
}
