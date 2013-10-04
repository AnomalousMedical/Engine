using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using Engine;
using System.Runtime.InteropServices;

namespace BulletPlugin
{
    public interface Generic6DofConstraintElement : TypedConstraintElement
    {
        SimElementDefinition saveToDefinition();

        void setFrameOffsetA(Vector3 origin);

        void setFrameOffsetA(Quaternion basis);

        void setFrameOffsetA(Vector3 origin, Quaternion basis);

        void setFrameOffsetB(Vector3 origin);

        void setFrameOffsetB(Quaternion basis);

        void setFrameOffsetB(Vector3 origin, Quaternion basis);

        Vector3 getFrameOffsetOriginA();

        Quaternion getFrameOffsetBasisA();

        Vector3 getFrameOffsetOriginB();

        Quaternion getFrameOffsetBasisB();

        void setLimit(int axis, float lo, float hi);

        void setLinearLowerLimit(Vector3 linearLower);

        void setLinearUpperLimit(Vector3 linearUpper);

        void setAngularLowerLimit(Vector3 angularLower);

        void setAngularUpperLimit(Vector3 angularUpper);

        void setParam(ConstraintParam num, float value, int axis);
    }
}
