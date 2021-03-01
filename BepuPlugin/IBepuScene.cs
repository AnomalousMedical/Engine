using BepuPhysics;
using BepuPlugin.Characters;
using Engine.Platform;

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
        void Update(Clock clock, in System.Numerics.Vector3 cameraForward);
    }
}