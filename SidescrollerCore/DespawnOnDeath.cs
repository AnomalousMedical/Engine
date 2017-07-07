using Engine;
using Engine.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.SidescrollerCore
{
    class DespawnOnDeath : BehaviorInterface
    {
        [Editable]
        public String HealthName { get; set; }

        public DespawnOnDeath()
        {
            HealthName = "Health";
        }

        protected override void link()
        {
            base.link();
            var health = Owner.getElement(HealthName) as Health;
            if(health == null)
            {
                blacklist($"Health '{HealthName}' for death not found.");
            }

            health.Dead += Health_Dead;
        }

        private void Health_Dead(Health obj)
        {
            Owner.destroy();
        }
    }
}
