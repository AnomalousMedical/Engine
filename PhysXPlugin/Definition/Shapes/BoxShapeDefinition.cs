using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysXWrapper;
using Engine.Saving;
using EngineMath;
using Engine.Editing;

namespace PhysXPlugin
{
    public class BoxShapeDefinition : ShapeDefinitionBase<PhysBoxShapeDesc>
    {
        public BoxShapeDefinition()
            : base(new PhysBoxShapeDesc())
        {

        }

        [Editable]
        public Vector3 Dimensions
        {
            get
            {
                return shapeDesc.Dimensions;
            }
            set
            {
                shapeDesc.Dimensions = value;
            }
        }

        #region Saveable

        private const String DIMENSIONS = "Dimensions";

        private BoxShapeDefinition(LoadInfo loadInfo)
            : base(new PhysBoxShapeDesc(), loadInfo)
        {
            Dimensions = loadInfo.GetVector3(DIMENSIONS);
        }

        public override void getInfo(SaveInfo info)
        {
            base.getInfo(info);
            info.AddValue(DIMENSIONS, Dimensions);
        }

        #endregion Saveable
    }
}
