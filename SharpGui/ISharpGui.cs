using DiligentEngine;
using Engine;

namespace SharpGui
{
    public interface ISharpGui
    {
        void Begin();
        bool Button(SharpButton button);
        void End();
        void Render(IDeviceContext immediateContext);
        bool Slider(SharpSlider slider, ref int value);
        void Text(int x, int y, Color color, string text);
        IntSize2 MeasureButton(SharpButton sharpButton);
    }
}