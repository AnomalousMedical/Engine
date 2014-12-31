using Anomalous.GuiFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anomalous.libRocketWidget
{
    /// <summary>
    /// A task to show the lib rocket debugger.
    /// </summary>
    public class ShowLibRocketDebugger : Task, IDisposable
    {
        private RocketDebuggerWindow rocketDebugger;
        private GUIManager guiManager;

        public ShowLibRocketDebugger(GUIManager guiManager, string uniqueName, string name, string iconName, string category)
            :base(uniqueName, name, iconName, category)
        {
            this.guiManager = guiManager;
        }

        public void Dispose()
        {
            if (rocketDebugger != null)
            {
                guiManager.removeManagedDialog(rocketDebugger);
                rocketDebugger.Dispose();
            }
        }

        public override void clicked(TaskPositioner taskPositioner)
        {
            if (rocketDebugger == null)
            {
                rocketDebugger = new RocketDebuggerWindow();
                guiManager.addManagedDialog(rocketDebugger);
            }
            rocketDebugger.Position = taskPositioner.findGoodWindowPosition(rocketDebugger.Width, rocketDebugger.Height);
            rocketDebugger.Visible = true;
            rocketDebugger.Closed += rocketDebugger_Closed;
        }

        public override bool Active
        {
            get
            {
                return rocketDebugger != null && rocketDebugger.Visible;
            }
        }

        void rocketDebugger_Closed(object sender, EventArgs e)
        {
            fireItemClosed();
        }
    }
}
