using Engine;
using Engine.Attributes;
using Engine.ObjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Behaviors.Animation
{
    public class PositionBroadcaster : Interface
    {
        [DoNotCopy]
        [DoNotSave]
        public event Action<SimObject> PositionChanged;

        protected override void positionUpdated()
        {
            if(PositionChanged != null)
            {
                PositionChanged.Invoke(this.Owner);
            }
        }
    }
}
