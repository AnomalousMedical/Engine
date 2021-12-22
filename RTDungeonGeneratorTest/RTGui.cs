using Engine;
using Engine.CameraMovement;
using Engine.Platform;
using SharpGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTDungeonGeneratorTest
{
    internal class RTGui
    {
        const float LightRange = 80f;
        const float LightConversion = 1;

        private readonly IScaleHelper scaleHelper;
        private readonly ISharpGui sharpGui;
        private readonly OSWindow window;
        private readonly FirstPersonFlyCamera cameraControls;
        private Vector4 lightPos = new Vector4(0, 24, -40, 0);

        private SharpText lightPosText = new SharpText() { Text = "" };
        private SharpText cameraPosText = new SharpText() { Text = "" };

        SharpSliderHorizontal lightPosX;
        SharpSliderHorizontal lightPosY;
        SharpSliderHorizontal lightPosZ;

        public RTGui(IScaleHelper scaleHelper, ISharpGui sharpGui, OSWindow window, FirstPersonFlyCamera cameraControls)
        {
            this.scaleHelper = scaleHelper;
            this.sharpGui = sharpGui;
            this.window = window;
            this.cameraControls = cameraControls;
            lightPosX = new SharpSliderHorizontal() { Rect = scaleHelper.Scaled(new IntRect(100, 10, 500, 35)), Max = ToSlider(LightRange) };
            lightPosY = new SharpSliderHorizontal() { Rect = scaleHelper.Scaled(new IntRect(100, 50, 500, 35)), Max = ToSlider(LightRange) };
            lightPosZ = new SharpSliderHorizontal() { Rect = scaleHelper.Scaled(new IntRect(100, 90, 500, 35)), Max = ToSlider(LightRange) };
        }

        public void Update(Clock clock)
        {
            int light = ToSlider(lightPos.x);
            if (sharpGui.Slider(lightPosX, ref light) || sharpGui.ActiveItem == lightPosX.Id)
            {
                lightPos.x = FromSlider(light);
            }

            light = ToSlider(lightPos.y);
            if (sharpGui.Slider(lightPosY, ref light) || sharpGui.ActiveItem == lightPosY.Id)
            {
                lightPos.y = FromSlider(light);
            }

            light = ToSlider(lightPos.z);
            if (sharpGui.Slider(lightPosZ, ref light) || sharpGui.ActiveItem == lightPosZ.Id)
            {
                lightPos.z = FromSlider(light);
            }

            lightPosText.Text = lightPos.ToString();
            cameraPosText.Text = cameraControls.Position.ToString();

            var layout =
                new MarginLayout(new IntPad(scaleHelper.Scaled(10)),
                new MaxWidthLayout(scaleHelper.Scaled(700),
                new ColumnLayout(lightPosText, cameraPosText) { Margin = new IntPad(10) }
                ));
            var desiredSize = layout.GetDesiredSize(sharpGui);
            layout.SetRect(new IntRect(0, window.WindowHeight - desiredSize.Height, desiredSize.Width, desiredSize.Height));

            //Buttons
            sharpGui.Text(lightPosText);
            sharpGui.Text(cameraPosText);
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
