using DIKUArcade.Entities;
using Galaga.GalagaEntities;

namespace Galaga.MovementStrategy {
    public interface IMovementStrategy {
        void MoveEnemy(Enemy enemy);
        void MoveEnemies(EntityContainer<Enemy> enemies);
    }
}