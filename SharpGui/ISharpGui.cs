using DiligentEngine;
using Engine;
using Engine.Platform;

namespace SharpGui
{
    public interface ISharpGui
    {
        void Begin(Clock clock);
        bool Button(SharpButton button);
        void End();
        void Render(IDeviceContext immediateContext);
        bool Slider(SharpSlider slider, ref int value);
        void Text(int x, int y, Color color, string text);
        IntSize2 MeasureButton(SharpButton sharpButton);
    }
}