namespace Engine
{
    public interface IScaleHelper
    {
        float ScaleFactor { get; }

        int Scaled(int originalValue);
        IntPad Scaled(IntPad originalValue);
        IntVector2 Scaled(IntVector2 originalValue);
        IntRect Scaled(IntRect originalValue);
        IntSize2 Scaled(IntSize2 originalValue);
        uint Scaled(uint originalValue);
        int Unscaled(int originalValue);
    }
}