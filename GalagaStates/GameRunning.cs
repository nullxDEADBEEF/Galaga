using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.State;
using Galaga.MovementStrategy;
using Galaga.Squadrons;

// TODO: remember background image

namespace Galaga.GalagaStates {
    public class GameRunning : IGameState {
        private static GameRunning instance;
        private Image backgroundImage;
        private Player player;
        private List<ISquadron> enemeySquadrons;

        private List<Image> greenEnemy;

        private List<Image> blueEnemy;

        private List<Image> redEnemy;
        private List<List<Image>> enemyTypes;
        private ZigZagDown zigZagDown;
        private EntityContainer projectiles;
        private List<Image> explosionStrides;
        private AnimationContainer explosions;
        private int explosionLength = 500;
        private Image laserImage;  

        public static GameRunning GetInstance() {
            return GameRunning.instance ?? (GameRunning.instance = new GameRunning());
        }

        public GameRunning() {
            backgroundImage = new Image(Path.Combine("Assets", "Images", "SpaceBackground.png"));
            player = new Player(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f));
            
            enemeySquadrons = new List<ISquadron>() {new BlueSquadron(), 
                new GreenSquadron(),
                new RedSquadron()};
            greenEnemy = ImageStride.CreateStrides(2,
                Path.Combine("Assets", "Images", "GreenMonster.png"));
            blueEnemy = ImageStride.CreateStrides(4,
                Path.Combine("Assets", "Images", "BlueMonster.png"));
            redEnemy = ImageStride.CreateStrides(2,
                Path.Combine("Assets", "Images", "RedMonster.png"));
            enemyTypes = new List<List<Image>>() {blueEnemy, greenEnemy, redEnemy};
            zigZagDown = new ZigZagDown();
            explosionStrides = ImageStride.CreateStrides(8,
                Path.Combine("Assets", "Images", "Explosion.png"));
            explosions = new AnimationContainer(80);
            projectiles = new EntityContainer();
            laserImage = new Image(Path.Combine("Assets", "Images", "BulletRed2.png"));

            for (int i = 0; i < enemeySquadrons.Count; i++) {
                enemeySquadrons[i].CreateEnemies(enemyTypes[i]);
            }
        }
        
        
        public void AddExplosion(float posX, float posY, float extentX, float extentY) {
            explosions.AddAnimation(
                new StationaryShape(posX, posY, extentX, extentY), explosionLength,
                new ImageStride(explosionLength / 8, explosionStrides));
        }

        public void AddProjectiles() {
            var laser = new DynamicShape(
                new Vec2F(player.GetPos.X + 0.0425f, player.GetPos.Y + 0.1f), 
                new Vec2F(0.008f, 0.027f));
            projectiles.AddDynamicEntity(laser, laserImage);
        }

        public void MoveProjectiles() {
            projectiles.Iterate(projectile => {
                projectile.Shape.Move();
                ((DynamicShape) projectile.Shape).Direction.Y = 0.02f;
            });
        }

        public void CheckProjectileCollision() {
            projectiles.Iterate(projectile => {
                if (projectile.Shape.Position.Y > 1.0f) {
                    projectile.DeleteEntity();
                }

                for (int i = 0; i < enemeySquadrons.Count; i++) {
                    enemeySquadrons[i].Enemies.Iterate(enemy => {
                        if (CollisionDetection.Aabb(
                            (DynamicShape)projectile.Shape, enemy.Shape).Collision) {
                            AddExplosion(enemy.Shape.Position.X, enemy.Shape.Position.Y,
                                enemy.Shape.Extent.X, enemy.Shape.Extent.Y);
                            projectile.DeleteEntity();
                            enemy.DeleteEntity();
                        }
                    });
                }
            });
        }


        public void GameLoop() {
            throw new NotImplementedException();
        }

        public void InitializeGameState() {
            throw new NotImplementedException();
        }

        public void UpdateGameLogic() {
            MoveProjectiles();
            CheckProjectileCollision();
            for (int i = 0; i < enemeySquadrons.Count; i++) {
                zigZagDown.MoveEnemies(enemeySquadrons[i].Enemies);
            }
            player.Move();
        }

        public void RenderState() {
            backgroundImage.Render(new StationaryShape(0.0f, 0.0f, 1.0f, 1.0f));
            player.Render();
            projectiles.RenderEntities();
            for (int i = 0; i < enemeySquadrons.Count; i++) {
                enemeySquadrons[i].Enemies.RenderEntities();
            }
            explosions.RenderAnimations();
        }

        public void HandleKeyEvent(string keyValue, string keyAction) {
            switch (keyAction) {
                case "KEY_PRESS":
                    switch (keyValue) {
                        case "KEY_ESCAPE":
                            GalagaBus.GetBus().RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent, this, 
                                "CHANGE_STATE", "GAME_PAUSED", ""));
                            break;
                            
                        case "KEY_LEFT":
                            GalagaBus.GetBus().RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForAllProcessors(
                                    GameEventType.PlayerEvent, this,
                                    "PLAYER_LEFT", "KEY_PRESS", ""));
                            break;
                            
                        case "KEY_RIGHT":
                            GalagaBus.GetBus().RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForAllProcessors(
                                    GameEventType.PlayerEvent, this,
                                    "PLAYER_RIGHT", "KEY_PRESS", ""));
                            break;
                          
                        case "KEY_SPACE":
                            AddProjectiles();
                            break;
                    }

                    break;
                case "KEY_RELEASE":
                    switch (keyValue) {
                        case "KEY_LEFT":
                            GalagaBus.GetBus().RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForAllProcessors(
                                    GameEventType.PlayerEvent, this,
                                    "PLAYER_STOP", "KEY_RELEASE", ""));
                            break;
                            
                        case "KEY_RIGHT":
                            GalagaBus.GetBus().RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForAllProcessors(
                                    GameEventType.PlayerEvent, this,
                                    "PLAYER_STOP", "KEY_RELEASE", ""));
                            break;
                    }

                    break;
            }
        }
    }
}