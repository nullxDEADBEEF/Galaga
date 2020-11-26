using System;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;

// TODO: fix white screen of death
// BUG(1): if the enemies are all killed, and then you try to start a new game, it continues from
// BUG(1): the previous game 

namespace Galaga.GalagaStates {
    public class GamePaused : IGameState {
        private static GamePaused instance;

        private Text[] pauseMenuButtons;
        private int activeButton;
        private int maxMenuButtons;
        private Image backgroundImage;

        public static GamePaused GetInstance() {
            return GamePaused.instance ?? (GamePaused.instance = new GamePaused());
        }

        public GamePaused() {
            backgroundImage = new Image(Path.Combine("Assets", "Images", "SpaceBackground.png"));
            pauseMenuButtons = new[] {
                new Text("Continue", new Vec2F(0.3f, 0.5f), new Vec2F(0.2f, 0.3f)),
                new Text("Main Menu", new Vec2F(0.3f, 0.25f), new Vec2F(0.2f, 0.3f)),
            };
            
            InitializeGameState();
        }

        public void GameLoop() {
        }

        public void InitializeGameState() {
            activeButton = 0;
            maxMenuButtons = 1;
        }

        public void UpdateGameLogic() {
        }

        public void RenderState() {
            backgroundImage.Render(new StationaryShape(0.0f, 0.0f, 1.0f, 1.0f));
            for (int i = 0; i < pauseMenuButtons.Length; i++) {
                if (activeButton == i) {
                    pauseMenuButtons[activeButton].SetColor(new Vec3I(255, 255, 255));
                    pauseMenuButtons[i].RenderText();
                } else {
                    pauseMenuButtons[i].SetColor(new Vec3I(255, 0, 0));
                    pauseMenuButtons[i].RenderText();
                }
            }
        }

        public void HandleKeyEvent(string keyValue, string keyAction) {
            switch (keyAction) {
                case "KEY_PRESS":
                    switch (keyValue) {
                        case "KEY_UP":
                            if (activeButton > maxMenuButtons) {
                                activeButton -= 1;
                            } else {
                                activeButton %= maxMenuButtons;
                            }

                            break;
                            
                        case "KEY_DOWN":
                            if (activeButton < maxMenuButtons) {
                                activeButton += 1;
                            } else {
                                activeButton %= maxMenuButtons;
                            }

                            break;
                            
                        case "KEY_ENTER":
                            if (activeButton == 0) {
                                GalagaBus.GetBus().RegisterEvent(
                                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                                        GameEventType.GameStateEvent, this,
                                        "CHANGE_STATE", "GAME_RUNNING", ""));
                            } else if (activeButton == 1) {
                                GalagaBus.GetBus().RegisterEvent(
                                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                                        GameEventType.GameStateEvent, this,
                                        "CHANGE_STATE", "MAIN_MENU", ""));
                            }

                            break;
                    }

                    break;
            }
        }
    }
}