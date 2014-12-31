using libRocketPlugin;
using Medical.GUI;
using MyGUIPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anomalous.libRocketWidget
{
    public class RocketDebuggerWindow : MDIDialog
    {
        private RocketWidget rocketWidget;

        public RocketDebuggerWindow()
            : base("Anomalous.libRocketWidget.RocketGui.RocketDebuggerWindow.layout")
        {
            rocketWidget = new RocketWidget((ImageBox)window.findWidget("RocketImage"), false);
            Debugger.Initialise(rocketWidget.Context);
            Debugger.SetVisible(true);
            this.Resized += RocketDebuggerWindow_Resized;
        }

        public override void Dispose()
        {
            rocketWidget.Dispose();
            base.Dispose();
        }

        void RocketDebuggerWindow_Resized(object sender, EventArgs e)
        {
            rocketWidget.resized();
        }
    }
}
