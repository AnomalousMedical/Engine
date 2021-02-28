using BepuPhysics;
using Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace BepuPlugin.Characters
{
    public class CharacterMoverDescription
    {
        /// <summary>
        /// Character's up direction in the local space of the character's body.
        /// </summary>
        public Vector3 LocalUp = new Vector3(0, 1, 0);
        /// <summary>
        /// Velocity at which the character pushes off the support during a jump.
        /// </summary>
        public float JumpVelocity = 6f;
        /// <summary>
        /// Maximum force the character can apply tangent to the supporting surface to move.
        /// </summary>
        public float MaximumHorizontalForce = 20f;
        /// <summary>
        /// Maximum force the character can apply to glue itself to the supporting surface.
        /// </summary>
        public float MaximumVerticalForce = 100;
        /// <summary>
        /// Maximum slope angle that the character can treat as a support.
        /// </summary>
        public float MaximumSlope = MathF.PI * 0.25f;
        /// <summary>
        /// Depth threshold beyond which a contact is considered a support if it the normal allows it. Multiply half the shape size * -0.01f to get a good value.
        /// </summary>
        public float MinimumSupportDepth = -0.01f; //Good starting guess for unit sized shapes, but usually scaled to the shape size
        /// <summary>
        /// Depth threshold beyond which a contact is considered a support if the previous frame had support, even if it isn't deep enough to meet the MinimumSupportDepth.
        /// </summary>
        public float SpeculativeMargin = 0.1f;
        /// <summary>
        /// Normal movement speed.
        /// </summary>
        public float Speed = 4f;
        /// <summary>
        /// Multiple of speed for sprinting.
        /// </summary>
        public float SprintMultiple = 1.75f;
    }
}
