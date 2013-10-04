using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using System.Runtime.InteropServices;

namespace BulletPlugin
{
    public enum ConstraintParam : int
    {
        CONSTRAINT_ERP = 1,
        CONSTRAINT_STOP_ERP,
        CONSTRAINT_STOP_CFM,
        CONSTRAINT_CFM,
    };

    public interface TypedConstraintElement : ISimElement
    {
        RigidBody RigidBodyA { get; }

        RigidBody RigidBodyB { get; }
    }
}
