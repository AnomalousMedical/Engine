using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest.GameOver
{
    class GameOverGameState : IGameOverGameState
    {
        public IEnumerable<SceneObject> SceneObjects => Enumerable.Empty<SceneObject>();

        public void SetActive(bool active)
        {

        }

        public IGameState Update(Clock clock)
        {
            return this;
        }
    }
}
