using DiligentEngine.RT;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    interface IGameState
    {
        public void SetActive(bool active);

        public IGameState Update(Clock clock);

        public RTInstances Instances { get; }
    }
}
