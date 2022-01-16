using Engine;
using SharpGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest.Exploration.Menu
{
    interface IContextMenu
    {
        void ClearContext(Action activatedCallback);
        void HandleContext(String title, Action activatedCallback);

        void Update();
    }

    class ContextMenu : IContextMenu
    {
        private readonly ISharpGui sharpGui;
        private readonly IScaleHelper scaleHelper;
        private readonly IScreenPositioner screenPositioner;
        SharpButton contextButton = new SharpButton() { Text = "Debug" };
        private Action activatedCallback;

        public ContextMenu(
            ISharpGui sharpGui,
            IScaleHelper scaleHelper,
            IScreenPositioner screenPositioner)
        {
            this.sharpGui = sharpGui;
            this.scaleHelper = scaleHelper;
            this.screenPositioner = screenPositioner;
        }

        public void HandleContext(String title, Action activatedCallback)
        {
            contextButton.Text = title;
            this.activatedCallback = activatedCallback;
        }

        public void ClearContext(Action activatedCallback)
        {
            if (this.activatedCallback == activatedCallback)
            {
                this.activatedCallback = null;
            }
        }

        public void Update()
        {
            if (activatedCallback == null)
            {
                return;
            }

            var layout =
               new MarginLayout(new IntPad(scaleHelper.Scaled(10)),
               new MaxWidthLayout(scaleHelper.Scaled(300),
               new ColumnLayout(contextButton) { Margin = new IntPad(10) }
            ));

            var desiredSize = layout.GetDesiredSize(sharpGui);
            layout.SetRect(screenPositioner.GetBottomRightRect(desiredSize));

            if (sharpGui.Button(contextButton))
            {
                activatedCallback();
            }
        }
    }
}
