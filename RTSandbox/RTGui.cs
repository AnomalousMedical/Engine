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
        private readonly IScaleHelper scaleHelper;
        private readonly ISharpGui sharpGui;
        private readonly OSWindow window;

        private SharpButton nextScene = new SharpButton() { Text = "Next Scene" };

        public RTGui(IScaleHelper scaleHelper, ISharpGui sharpGui, OSWindow window)
        {
            this.scaleHelper = scaleHelper;
            this.sharpGui = sharpGui;
            this.window = window;
        }

        public void Update(Clock clock)
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

            sharpGui.End();
        }
    }
}
