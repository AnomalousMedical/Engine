using Engine;
using Engine.Attributes;
using Engine.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.SidescrollerCore
{
    [Flags]
    public enum HealthGroup
    {
        Player = 1,
        PlayerBullet = 2,
        Enemy = 4,
        EnemyBullet = 8,
    }

    public class Health : BehaviorInterface
    {
        [Editable]
        public float Amount { get; set; }

        [Editable]
        public HealthGroup Group { get; set; }

        public Health()
        {
            Amount = 100.0f;
        }

        public void takeDamage(Damage damage)
        {
            if((this.Group & damage.Attacks) != 0)
            {
                this.Amount -= damage.Amount;
                if(DamageTaken != null)
                {
                    DamageTaken.Invoke(this);
                }
                if (this.Amount <= 0)
                {
                    if (Dead != null)
                    {
                        Dead.Invoke(this);
                    }
                }
            }
        }

        [DoNotCopy]
        [DoNotSave]
        public event Action<Health> Dead;

        [DoNotCopy]
        [DoNotSave]
        public event Action<Health> DamageTaken;
    }
}
