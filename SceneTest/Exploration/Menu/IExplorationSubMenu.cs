using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest.Exploration.Menu
{
   interface IExplorationSubMenu
    {
        void Update(IExplorationGameState explorationGameState, IExplorationMenu menu);
    }
}
