using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.SidescrollerCore
{
    public class CameraTarget : BehaviorInterface
    {
        public event Action<Vector3> Moved;

        protected override void positionUpdated()
        {
            if(Moved != null)
            {
                Moved.Invoke(this.Owner.Translation);
            }
        }
    }
}
