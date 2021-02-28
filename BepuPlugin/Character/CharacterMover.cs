using BepuPhysics;
using BepuPhysics.Collidables;
using System.Numerics;
using System;
using System.Diagnostics;
using BepuUtilities;
using Engine.Platform;

namespace BepuPlugin.Characters
{
    /// <summary>
    /// Convenience structure that wraps a CharacterController reference and its associated body.
    /// </summary>
    /// <remarks>
    /// <para>This should be treated as an example- nothing here is intended to suggest how you *must* handle characters. 
    /// On the contrary, this does some fairly inefficient stuff if you're dealing with hundreds of characters in a predictable way.
    /// It's just a fairly convenient interface for demos usage.</para>
    /// <para>Note that all characters are dynamic and respond to constraints and forces in the simulation.</para>
    /// </remarks>
    public class CharacterMover
    {
        BodyHandle bodyHandle;
        CharacterControllers characters;
        float speed;
        float sprintMultiple;

        public Vector2 movementDirection;
        public bool sprint;
        public bool tryJump;

        public BodyHandle BodyHandle { get { return bodyHandle; } }

        public CharacterMover(CharacterControllers characters, in BodyDescription bodyDescription, CharacterMoverDescription description)
        {
            this.characters = characters;
            this.bodyHandle = characters.Simulation.Bodies.Add(bodyDescription);

            
            ref var character = ref characters.AllocateCharacter(bodyHandle);
            character.LocalUp = description.LocalUp.ToSystemNumerics();
            character.CosMaximumSlope = MathF.Cos(description.MaximumSlope);
            character.JumpVelocity = description.JumpVelocity;
            character.MaximumVerticalForce = description.MaximumVerticalForce;
            character.MaximumHorizontalForce = description.MaximumHorizontalForce;
            character.MinimumSupportDepth = description.MinimumSupportDepth;
            character.MinimumSupportContinuationDepth = -description.SpeculativeMargin;

            this.speed = description.Speed;
            this.sprintMultiple = description.SprintMultiple;
        }

        /// <summary>
        /// Removes the character's body from the simulation and the character from the associated characters set.
        /// </summary>
        public void Dispose()
        {
            characters.Simulation.Bodies.Remove(bodyHandle);
            characters.RemoveCharacterByBodyHandle(bodyHandle);
        }

        public void UpdateCharacterGoals(in Vector3 viewDirection, float simulationTimestepDuration)
        {
            var movementDirectionCalc = this.movementDirection;
            movementDirectionCalc.X *= -1; //Need to invert this vs our input
            var movementDirectionLengthSquared = movementDirectionCalc.LengthSquared();
            if (movementDirectionLengthSquared > 0)
            {
                movementDirectionCalc /= MathF.Sqrt(movementDirectionLengthSquared);
            }

            ref var character = ref characters.GetCharacterByBodyHandle(bodyHandle);
            character.TryJump = tryJump;
            var characterBody = new BodyReference(bodyHandle, characters.Simulation.Bodies);
            var effectiveSpeed = sprint ? speed * sprintMultiple : speed;
            var newTargetVelocity = movementDirectionCalc * effectiveSpeed;
            
            //Modifying the character's raw data does not automatically wake the character up, so we do so explicitly if necessary.
            //If you don't explicitly wake the character up, it won't respond to the changed motion goals.
            //(You can also specify a negative deactivation threshold in the BodyActivityDescription to prevent the character from sleeping at all.)
            if (!characterBody.Awake &&
                ((character.TryJump && character.Supported) ||
                newTargetVelocity != character.TargetVelocity ||
                (newTargetVelocity != Vector2.Zero && character.ViewDirection != viewDirection)))
            {
                characters.Simulation.Awakener.AwakenBody(character.BodyHandle);
            }
            character.TargetVelocity = newTargetVelocity;
            character.ViewDirection = viewDirection;

            //The character's motion constraints aren't active while the character is in the air, so if we want air control, we'll need to apply it ourselves.
            //(You could also modify the constraints to do this, but the robustness of solved constraints tends to be a lot less important for air control.)
            //There isn't any one 'correct' way to implement air control- it's a nonphysical gameplay thing, and this is just one way to do it.
            //Note that this permits accelerating along a particular direction, and never attempts to slow down the character.
            //This allows some movement quirks common in some game character controllers.
            //Consider what happens if, starting from a standstill, you accelerate fully along X, then along Z- your full velocity magnitude will be sqrt(2) * maximumAirSpeed.
            //Feel free to try alternative implementations. Again, there is no one correct approach.
            if (!character.Supported && movementDirectionLengthSquared > 0)
            {
                QuaternionEx.Transform(character.LocalUp, characterBody.Pose.Orientation, out var characterUp);
                var characterRight = Vector3.Cross(character.ViewDirection, characterUp);
                var rightLengthSquared = characterRight.LengthSquared();
                if (rightLengthSquared > 1e-10f)
                {
                    characterRight /= MathF.Sqrt(rightLengthSquared);
                    var characterForward = Vector3.Cross(characterUp, characterRight);
                    var worldMovementDirection = characterRight * movementDirectionCalc.X + characterForward * movementDirectionCalc.Y;
                    var currentVelocity = Vector3.Dot(characterBody.Velocity.Linear, worldMovementDirection);
                    //We'll arbitrarily set air control to be a fraction of supported movement's speed/force.
                    const float airControlForceScale = .2f;
                    const float airControlSpeedScale = .2f;
                    var airAccelerationDt = characterBody.LocalInertia.InverseMass * character.MaximumHorizontalForce * airControlForceScale * simulationTimestepDuration;
                    var maximumAirSpeed = effectiveSpeed * airControlSpeedScale;
                    var targetVelocity = MathF.Min(currentVelocity + airAccelerationDt, maximumAirSpeed);
                    //While we shouldn't allow the character to continue accelerating in the air indefinitely, trying to move in a given direction should never slow us down in that direction.
                    var velocityChangeAlongMovementDirection = MathF.Max(0, targetVelocity - currentVelocity);
                    characterBody.Velocity.Linear += worldMovementDirection * velocityChangeAlongMovementDirection;
                    Debug.Assert(characterBody.Awake, "Velocity changes don't automatically update objects; the character should have already been woken up before applying air control.");
                }
            }
        }
    }
}


