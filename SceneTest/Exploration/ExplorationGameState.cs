using BepuPlugin;
using Engine;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class ExplorationGameState : IGameState
    {
        private readonly ICoroutineRunner coroutineRunner;
        private readonly IBepuScene bepuScene;
        private readonly IBattleTrigger battleTrigger;
        private readonly ILevelManager levelManager;
        private readonly SceneObjectManager<ILevelManager> sceneObjects;
        private readonly IBattleGameState battleGameState;
        private readonly IDebugGui debugGui;
        private bool showDebugGui = true;

        public ExplorationGameState(
            ICoroutineRunner coroutineRunner,
            IBepuScene bepuScene,
            IBattleTrigger battleTrigger,
            ILevelManager levelManager,
            SceneObjectManager<ILevelManager> sceneObjects,
            IBattleGameState battleGameState,
            IDebugGui debugGui)
        {
            this.coroutineRunner = coroutineRunner;
            this.bepuScene = bepuScene;
            this.battleTrigger = battleTrigger;
            this.levelManager = levelManager;
            this.sceneObjects = sceneObjects;
            this.battleGameState = battleGameState;
            this.debugGui = debugGui;
            this.battleGameState.AfterBattleState = this;
        }

        public IEnumerable<SceneObject> SceneObjects => sceneObjects;

        public void SetActive(bool active)
        {
            
        }

        public IGameState Update(Clock clock)
        {
            IGameState nextState = this;

            if (battleTrigger.UpdateRandomEncounter(clock, levelManager.IsPlayerMoving))
            {
                nextState = battleGameState;
            }
            else
            {
                bepuScene.Update(clock, new System.Numerics.Vector3(0, 0, 1));
            }

            if (showDebugGui)
            {
                var result = this.debugGui.Update();
                switch (result)
                {
                    case IDebugGui.Result.StartBattle:
                        nextState = battleGameState;
                        break;
                }
            }

            return nextState;
        }
    }
}
