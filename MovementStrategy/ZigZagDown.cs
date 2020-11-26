using System;
using DIKUArcade.Entities;
using Galaga.GalagaEntities;

namespace Galaga.MovementStrategy {
    public class ZigZagDown : IMovementStrategy {
        private float movementSpeed = 0.0003f;
        private float sineWavePeriod = 0.045f;
        private float amplitude = 0.05f;
        
        public void MoveEnemy(Enemy enemy) {
            enemy.Shape.Position.Y -= movementSpeed;
            enemy.Shape.Position.X = enemy.StartPos.X + amplitude *
                                     (float)Math.Sin((2 * Math.PI *
                                            (enemy.StartPos.Y - enemy.Shape.Position.Y)) /
                                            sineWavePeriod);
        }

        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            enemies.Iterate(MoveEnemy);
        }
    }
}