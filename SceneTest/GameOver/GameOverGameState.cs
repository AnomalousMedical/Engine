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
        private readonly IScreenPositioner screenPositioner;

        public IEnumerable<SceneObject> SceneObjects => Enumerable.Empty<SceneObject>();

        public GameOverGameState
        (
            ISharpGui sharpGui,
            IScreenPositioner screenPositioner
        )
        {
            this.sharpGui = sharpGui;
            this.screenPositioner = screenPositioner;
        }

        public void SetActive(bool active)
        {

        }

        public IGameState Update(Clock clock)
        {
            var size = sharpGui.MeasureText("Game Over");
            var rect = screenPositioner.GetCenterRect(size);
            sharpGui.Text(rect.Left, rect.Top, Color.White, "Game Over");

            return this;
        }
    }
}
