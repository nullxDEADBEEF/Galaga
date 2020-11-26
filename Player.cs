using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga {
    public class Player : IGameEventProcessor<object> {
        private Entity player;

        public Player(Vec2F position, Vec2F extent) {     
            player = new Entity(
                new DynamicShape(position, extent), 
                new Image(Path.Combine("Assets", "Images", "Player.png")));
            
            GalagaBus.GetBus().Subscribe(GameEventType.PlayerEvent, this);
        }

        public Vec2F GetPos {
            get { return player.Shape.Position; }
        }

        public void Render() {
            player.RenderEntity();
        }

        public void Move() {
            if (player.Shape.Position.X <= 0.0f) {
                player.Shape.Position.X = 0.0f;
            } else if (player.Shape.Position.X >= 1.0f - player.Shape.Extent.X) {
                player.Shape.Position.X = 1.0f - player.Shape.Extent.X;
            }
            player.Shape.Move();
        }
        
        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.PlayerEvent) {
                switch (gameEvent.Message) {
                    case "PLAYER_LEFT":
                        ((DynamicShape) player.Shape).Direction.X = -0.01f;
                        break;
                    case "PLAYER_RIGHT":
                        ((DynamicShape) player.Shape).Direction.X = 0.01f;
                        break;
                    case "PLAYER_STOP":
                        ((DynamicShape) player.Shape).Direction.X = 0.0f;
                        break;
                }
            }
        }
    }
}