using BepuPhysics;
using Engine.Platform;
using System.Numerics;

namespace BepuPlugin
{
    public interface IBepuScene
    {
        Simulation Simulation { get; }

        void CreateCharacter(Vector3 position);
        void Update(Clock clock, Vector3 cameraForward);
    }
}