using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga.GalagaEntities {
    public class Enemy : Entity {
        public Vec2F StartPos {get;}
        
        public Enemy(StationaryShape shape, IBaseImage image) : base(shape, image) {
            StartPos = shape.Position.Copy();
        }
    }
}