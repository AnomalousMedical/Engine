using BepuPhysics;
using BepuPlugin.Characters;
using Engine.Platform;
using System;
using System.Numerics;

namespace BepuPlugin
{
    public interface IBepuScene
    {
        event Action<IBepuScene> OnUpdated;

        Simulation Simulation { get; }

        CharacterMover CreateCharacterMover(in BodyDescription bodyDescription, CharacterMoverDescription desc);

        void DestroyCharacterMover(CharacterMover mover);

        void Update(Clock clock, in Vector3 cameraForward);
    }
}