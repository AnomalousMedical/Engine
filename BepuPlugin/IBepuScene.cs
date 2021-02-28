using BepuPhysics;
using BepuPlugin.Characters;
using Engine.Platform;
using System.Numerics;

namespace BepuPlugin
{
    public interface IBepuScene
    {
        Simulation Simulation { get; }

        CharacterMover CreateCharacterMover(in BodyDescription bodyDescription, CharacterMoverDescription desc);

        void DestroyCharacterMover(CharacterMover mover);

        void Update(Clock clock, Vector3 cameraForward);
    }
}