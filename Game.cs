using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.EventBus;
using DIKUArcade.Physics;
using DIKUArcade.Timers;
using Galaga.GalagaEntities;
using Galaga.GalagaStates;
using Galaga.MovementStrategy;
using Galaga.Squadrons;

namespace Galaga {
    public class Game : IGameEventProcessor<object> {
        private Window win;
        private GameTimer gameTimer;
        private GameEventBus<object> eventBus;
        private StateMachine stateMachine;

        public Game() {
            win = new Window("Galaga", 500, AspectRatio.R1X1);
            
            eventBus = GalagaBus.GetBus();
            eventBus.InitializeEventBus(new List<GameEventType>() {
                GameEventType.InputEvent,
                GameEventType.WindowEvent,
                GameEventType.PlayerEvent,
                GameEventType.GameStateEvent
            });
            win.RegisterEventBus(eventBus);
            stateMachine = new StateMachine();
            gameTimer = new GameTimer(60, 60);
            eventBus.Subscribe(GameEventType.InputEvent, this);
            eventBus.Subscribe(GameEventType.WindowEvent, this);
        }

        public void GameLoop() {
            while (win.IsRunning()) {
                gameTimer.MeasureTime();
                while (gameTimer.ShouldUpdate()) {
                    win.PollEvents();
                    eventBus.ProcessEvents();
                    stateMachine.ActiveState.UpdateGameLogic();
                }

                if (gameTimer.ShouldRender()) {
                    win.Clear();
                    stateMachine.ActiveState.RenderState();
                    win.SwapBuffers();
                }

                if (gameTimer.ShouldReset()) {
                    win.Title = "Galaga | UPS: " + gameTimer.CapturedUpdates +
                                ", FPS: " + gameTimer.CapturedFrames;
                }
            }
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.WindowEvent) {
                switch (gameEvent.Message) {
                    case "CLOSE_WINDOW":
                        win.CloseWindow();
                        break;
                }
            }
        }
    }
}