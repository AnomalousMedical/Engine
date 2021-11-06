using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest.Battle
{
    interface IBattleScene
    {
        IEnumerable<Enemy> CreateEnemies(IObjectResolver objectResolver, Party party);
    }
}
