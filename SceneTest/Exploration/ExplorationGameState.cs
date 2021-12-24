using BepuPlugin;
using DiligentEngine.RT;
using Engine;
using Engine.Platform;
using SceneTest.Exploration.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class ExplorationGameState : IExplorationGameState
    {
        private readonly ICoroutineRunner coroutineRunner;
        private readonly IBepuScene bepuScene;
        private readonly IBattleTrigger battleTrigger;
        private readonly ILevelManager levelManager;
        private readonly RTInstances<ILevelManager> rtInstances;
        private readonly IExplorationMenu explorationMenu;
        private IGameState battleState;
        private IGameState nextState; //This is changed per update to be the next game state

        public RTInstances Instances => rtInstances;

        public ExplorationGameState(
            ICoroutineRunner coroutineRunner,
            IBepuScene bepuScene,
            IBattleTrigger battleTrigger,
            ILevelManager levelManager,
            RTInstances<ILevelManager> rtInstances,
            IExplorationMenu explorationMenu)
        {
            this.coroutineRunner = coroutineRunner;
            this.bepuScene = bepuScene;
            this.battleTrigger = battleTrigger;
            this.levelManager = levelManager;
            this.rtInstances = rtInstances;
            this.explorationMenu = explorationMenu;

            coroutineRunner.RunTask(levelManager.Restart());
        }

        public void Link(IGameState battleState)
        {
            this.battleState = battleState;
        }

        public void SetActive(bool active)
        {
            //Stopping them both directions
            levelManager.StopPlayer();
        }

        public void RequestBattle()
        {
            nextState = battleState;
        }

        public IGameState Update(Clock clock)
        {
            nextState = this;

            if (explorationMenu.Update(this))
            {
                //If menu did something
            }
            else
            {
                if (battleTrigger.UpdateRandomEncounter(clock, levelManager.IsPlayerMoving))
                {
                    nextState = battleState;
                }
                else
                {
                    bepuScene.Update(clock, new System.Numerics.Vector3(0, 0, 1));
                }
            }

            return nextState;
        }
    }
}
