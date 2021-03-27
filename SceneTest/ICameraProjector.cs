using Engine;

namespace SceneTest
{
    interface ICameraProjector
    {
        Vector2 Project(in Vector3 position);
    }
}