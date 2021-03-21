using Engine;

namespace SharpGui
{
    public interface IScreenPositioner
    {
        IntRect GetBottomRightRect(in IntSize2 size);
    }
}