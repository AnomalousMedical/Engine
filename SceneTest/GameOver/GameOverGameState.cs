using Engine;
using Engine.Platform;
using SharpGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest.GameOver
{
    class GameOverGameState : IGameOverGameState
    {
        private readonly ISharpGui sharpGui;
        private readonly SceneObjectManager<IBattleManager> sceneObjects;
        private readonly IScreenPositioner screenPositioner;
        private readonly ICoroutineRunner coroutineRunner;
        private readonly ILevelManager levelManager;
        private IGameState explorationState;
        private SharpButton restart = new SharpButton() { Text = "Restart" };
        private SharpText gameOver = new SharpText("Game Over");
        private ILayoutItem layout;

        public IEnumerable<SceneObject> SceneObjects => sceneObjects;

        public GameOverGameState
        (
            ISharpGui sharpGui,
            SceneObjectManager<IBattleManager> sceneObjects,
            IScreenPositioner screenPositioner,
            ICoroutineRunner coroutineRunner,
            ILevelManager levelManager
        )
        {
            this.sharpGui = sharpGui;
            this.sceneObjects = sceneObjects;
            this.screenPositioner = screenPositioner;
            this.coroutineRunner = coroutineRunner;
            this.levelManager = levelManager;
            layout = new ColumnLayout(gameOver, restart) { Margin = new IntPad(10) };
        }

        public void Link(IGameState explorationState)
        {
            this.explorationState = explorationState;
        }

        public void SetActive(bool active)
        {

        }

        public IGameState Update(Clock clock)
        {
            IGameState nextState = this;

            var size = layout.GetDesiredSize(sharpGui);
            layout.GetDesiredSize(sharpGui);
            var rect = screenPositioner.GetCenterRect(size);
            layout.SetRect(rect);

            sharpGui.Text(gameOver);
            if (sharpGui.Button(restart))
            {
                coroutineRunner.RunTask(levelManager.Restart());
                nextState = explorationState;
            }

            return nextState;
        }
    }
}
