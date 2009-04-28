using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysXWrapper;
using Engine.Editing;
using Engine.Reflection;
using Engine.Saving;
using Engine.Attributes;
using EngineMath;

namespace PhysXPlugin
{
    public abstract class ShapeDefinition : Saveable
    {
        public abstract EditInterface getEditInterface();

        internal abstract PhysShapeDesc PhysShapeDesc
        {
            get;
        }

        public abstract void getInfo(SaveInfo info);

        public abstract float Density { get; set; }

        public abstract ushort Group { get; set; }

        public abstract float Mass { get; set; }

        public abstract ushort MaterialIndex { get; set; }

        public abstract uint NonInteractingCompartmentTypes { get; set; }

        public abstract ShapeFlag ShapeFlags { get; set; }

        public abstract float SkinWidth { get; set; }

        public abstract Vector3 LocalTranslation { get; set; }

        public abstract Quaternion LocalRotation { get; set; }
    }
}
