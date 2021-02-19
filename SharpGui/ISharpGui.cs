using DiligentEngine;
using Engine;
using Engine.Platform;
using System;

namespace SharpGui
{
    public interface ISharpGui
    {
        void Begin(Clock clock);
        SharpButtonResult Button(SharpButton button);
        void End();
        void Render(IDeviceContext immediateContext);
        bool Slider(SharpSlider slider, ref int value);
        void Text(int x, int y, Color color, string text);
        IntSize2 MeasureButton(SharpButton sharpButton);

        /// <summary>
        /// This should be used carefully ideally before begin or after end is called.
        /// This will change the current focused item to the new id. Useful
        /// for navigation.
        /// </summary>
        /// <param name="id"></param>
        void StealFocus(Guid id);
    }
}