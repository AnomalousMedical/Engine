using DiligentEngine;
using Engine;
using Engine.Platform;
using System;

namespace SharpGui
{
    public interface ISharpGui
    {
        /// <summary>
        /// Start a gui frame.
        /// </summary>
        /// <param name="clock"></param>
        void Begin(Clock clock);
        
        /// <summary>
        /// End a gui frame. Call after begin and before render.
        /// </summary>
        void End();
        
        /// <summary>
        /// Render the gui frame to the context.
        /// </summary>
        /// <param name="immediateContext"></param>
        void Render(IDeviceContext immediateContext);

        /// <summary>
        /// Draw a slider. Returns true if the passed in value changed.
        /// </summary>
        /// <param name="slider">The slider to draw.</param>
        /// <param name="value">The current value of the slider.</param>
        /// <returns>True if value changed.</returns>
        public bool Slider(SharpSliderHorizontal slider, ref int value, Guid? navUp = null, Guid? navDown = null);

        /// <summary>
        /// Draw a slider. Returns true if the passed in value changed.
        /// </summary>
        /// <param name="slider">The slider to draw.</param>
        /// <param name="value">The current value of the slider.</param>
        /// <returns>True if value changed.</returns>
        public bool Slider(SharpSliderVertical slider, ref int value, Guid? navLeft = null, Guid? navRight = null);

        /// <summary>
        /// Draw text
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        /// <param name="text"></param>
        void Text(int x, int y, Color color, string text);

        /// <summary>
        /// Draw a button.
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        bool Button(SharpButton button, Guid? navUp = null, Guid? navDown = null, Guid? navLeft = null, Guid? navRight = null);

        /// <summary>
        /// Draw a text input.
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        bool Input(SharpInput input, Guid? navUp = null, Guid? navDown = null, Guid? navLeft = null, Guid? navRight = null);

        /// <summary>
        /// Measure the size of a button.
        /// </summary>
        /// <param name="sharpButton"></param>
        /// <returns></returns>
        IntSize2 MeasureButton(SharpButton sharpButton);

        /// <summary>
        /// Measure the size of an input text.
        /// </summary>
        /// <param name="sharpInput"></param>
        /// <returns></returns>
        IntSize2 MeasureInput(SharpInput sharpInput);

        /// <summary>
        /// This should be used carefully ideally before begin or after end is called.
        /// This will change the current focused item to the new id. Useful
        /// for navigation.
        /// </summary>
        /// <param name="id"></param>
        void StealFocus(Guid id);
    }
}