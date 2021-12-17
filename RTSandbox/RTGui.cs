using Engine;
using Engine.Platform;
using SharpGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTSandbox
{
    internal class RTGui
    {
        const float LightRange = 10f;
        const float LightConversion = 30;

        private readonly IScaleHelper scaleHelper;
        private readonly ISharpGui sharpGui;
        private readonly OSWindow window;
        private Vector4 lightPos = new Vector4(0, -5, -1, 0);

        private SharpButton nextScene = new SharpButton() { Text = "Next Scene" };

        SharpSliderHorizontal lightPosX;
        SharpSliderHorizontal lightPosY;
        SharpSliderHorizontal lightPosZ;

        public RTGui(IScaleHelper scaleHelper, ISharpGui sharpGui, OSWindow window)
        {
            this.scaleHelper = scaleHelper;
            this.sharpGui = sharpGui;
            this.window = window;

            lightPosX = new SharpSliderHorizontal() { Rect = scaleHelper.Scaled(new IntRect(100, 10, 500, 35)), Max = ToSlider(LightRange * 2) };
            lightPosY = new SharpSliderHorizontal() { Rect = scaleHelper.Scaled(new IntRect(100, 50, 500, 35)), Max = ToSlider(LightRange * 2) };
            lightPosZ = new SharpSliderHorizontal() { Rect = scaleHelper.Scaled(new IntRect(100, 90, 500, 35)), Max = ToSlider(LightRange * 2) };
        }

        public void Update(Clock clock, in Constants constants)
        {
            sharpGui.Begin(clock);


            var layout =
                new MarginLayout(new IntPad(scaleHelper.Scaled(10)),
                new MaxWidthLayout(scaleHelper.Scaled(300),
                new ColumnLayout(nextScene) { Margin = new IntPad(10) }
                ));
            var desiredSize = layout.GetDesiredSize(sharpGui);
            layout.SetRect(new IntRect(window.WindowWidth - desiredSize.Width, window.WindowHeight - desiredSize.Height, desiredSize.Width, desiredSize.Height));

            //Buttons
            if (sharpGui.Button(nextScene))
            {

            }

            int light = ToSlider(constants.LightPos_0.x);
            if (sharpGui.Slider(lightPosX, ref light) || sharpGui.ActiveItem == lightPosX.Id)
            {
                lightPos.x = FromSlider(light);
            }

            light = ToSlider(constants.LightPos_0.y);
            if (sharpGui.Slider(lightPosY, ref light) || sharpGui.ActiveItem == lightPosY.Id)
            {
                lightPos.y = FromSlider(light);
            }

            light = ToSlider(constants.LightPos_0.z);
            if (sharpGui.Slider(lightPosZ, ref light) || sharpGui.ActiveItem == lightPosZ.Id)
            {
                lightPos.z = FromSlider(light);
            }

            sharpGui.End();
        }

        private int ToSlider(float pos)
        {
            return (int)((pos + LightRange) * LightConversion);
        }

        private float FromSlider(int pos)
        {
            return pos / LightConversion - LightRange;
        }

        public Vector4 LightPos => lightPos;
    }
}
