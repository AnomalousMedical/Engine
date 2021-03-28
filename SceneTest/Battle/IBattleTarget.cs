using Engine;
using RpgMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    interface IBattleTarget
    {
        IBattleStats Stats { get; }

        Vector3 DamageDisplayLocation { get; }

        Vector3 CursorDisplayLocation { get; }

        public void RequestDestruction();

        public void ApplyDamage(IDamageCalculator calculator, long damage);

        public bool IsDead { get; }
    }
}
