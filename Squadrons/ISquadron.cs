using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using Galaga.GalagaEntities;

namespace Galaga.Squadrons {
    public interface ISquadron {
        EntityContainer<Enemy> Enemies { get; }
        int MaxEnemies { get; }

        void CreateEnemies(List<Image> enemyStrides);
    }
}