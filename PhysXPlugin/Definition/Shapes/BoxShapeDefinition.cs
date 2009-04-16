using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysXWrapper;
using Engine.Saving;

namespace PhysXPlugin
{
    public class BoxShapeDefinition : ShapeDefinition
    {
        public BoxShapeDefinition()
            : base(new PhysBoxShapeDesc())
        {

        }

        private BoxShapeDefinition(LoadInfo loadInfo)
            : base(new PhysBoxShapeDesc())
        {
            restoreShape(loadInfo);
        }
    }
}
