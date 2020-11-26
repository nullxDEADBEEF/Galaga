using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using Galaga.GalagaEntities;

namespace Galaga.Squadrons {
    public class BlueSquadron : ISquadron {
        public EntityContainer<Enemy> Enemies { get; }
        public int MaxEnemies { get; }

        public BlueSquadron() {
            MaxEnemies = 2;
            Enemies = new EntityContainer<Enemy>();
        }

        public void CreateEnemies(List<Image> enemyStrides) {
            var enemy = new List<Enemy>();
            for (float x = 0.31f; x <= 0.6f; x += 0.29f) {
                enemy.Add(new Enemy(
                    new StationaryShape(new Vec2F(x, 0.5f), new Vec2F(0.1f, 0.1f)),
                    new ImageStride(80, enemyStrides)));
            }

            for (int i = 0; i < MaxEnemies; i++) {
                Enemies.AddDynamicEntity(enemy[i]);
            }
        }

    }
}