using Engine.Platform;

namespace SceneTest
{
    interface IBattleTrigger
    {
        bool UpdateRandomEncounter(Clock clock, bool moving);
    }
}