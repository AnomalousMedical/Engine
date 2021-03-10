using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPlugin.Characters;
using Engine.Platform;
using System;

namespace BepuPlugin
{
    public interface IBepuScene
    {
        Simulation Simulation { get; }

        event System.Action<IBepuScene> OnUpdated;

        void AddToInterpolation(in BodyHandle body);
        CharacterMover CreateCharacterMover(in BodyDescription bodyDescription, CharacterMoverDescription desc);
        void DestroyCharacterMover(CharacterMover mover);
        void GetInterpolatedPosition(in BodyHandle body, ref Engine.Vector3 position, ref Engine.Quaternion orientation);
        void RemoveFromInterpolation(in BodyHandle body);
        /// <summary>
        /// Starts listening for events related to the given collidable. When an event is fired call the given event handler.
        /// </summary>
        /// <param name="collidable">Collidable to start listening for.</param>
        void RegisterCollisionListener(CollidableReference collidable, Action<CollisionEvent> eventHandler);

        /// <summary>
        /// Stops listening for events related to the given collidable.
        /// </summary>
        /// <param name="collidable">Collidable to stop listening for.</param>
        void UnregisterCollisionListener(CollidableReference collidable);
        void Update(Clock clock, in System.Numerics.Vector3 cameraForward);
    }
}