using DIKUArcade.Entities;
using Galaga.GalagaEntities;

namespace Galaga.MovementStrategy {
    public class MoveDown : IMovementStrategy {
        public void MoveEnemy(Enemy enemy) {
            enemy.Shape.Position.Y -= 0.001f;
        }

        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            enemies.Iterate(MoveEnemy);
        }
    }
}