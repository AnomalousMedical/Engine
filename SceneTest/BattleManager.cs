using Anomalous.OSPlatform;
using Engine;
using Engine.Platform;
using SharpGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class BattleManager
    {
        private readonly EventManager eventManager;
        private readonly ISharpGui sharpGui;
        private readonly IScaleHelper scaleHelper;
        private readonly NativeOSWindow window;
        private SharpButton endBattle = new SharpButton() { Text = "End Battle" };

        public BattleManager(EventManager eventManager,
            ISharpGui sharpGui,
            IScaleHelper scaleHelper,
            NativeOSWindow window)
        {
            this.eventManager = eventManager;
            this.sharpGui = sharpGui;
            this.scaleHelper = scaleHelper;
            this.window = window;
        }

        public void SetActive(bool active)
        {
            if (active != this.Active)
            {
                this.Active = active;
                if (active)
                {
                    eventManager[EventLayers.Battle].OnUpdate += eventManager_OnUpdate;
                }
                else
                {
                    eventManager[EventLayers.Battle].OnUpdate -= eventManager_OnUpdate;
                }
            }
        }

        public void UpdateGui()
        {
            var layout =
                new MarginLayout(new IntPad(scaleHelper.Scaled(10)),
                new MaxWidthLayout(scaleHelper.Scaled(300),
                new ColumnLayout(endBattle) { Margin = new IntPad(10) }
                ));
            var desiredSize = layout.GetDesiredSize(sharpGui);
            layout.SetRect(new IntRect(window.WindowWidth - desiredSize.Width, window.WindowHeight - desiredSize.Height, desiredSize.Width, desiredSize.Height));

            if (sharpGui.Button(endBattle))
            {
                this.SetActive(false);
            }
        }

        public bool Active { get; private set; }

        private void eventManager_OnUpdate(EventLayer eventLayer)
        {
            eventLayer.alertEventsHandled();
        }
    }
}
