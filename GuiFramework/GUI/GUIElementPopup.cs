using MyGUIPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.GuiFramework
{
    /// <summary>
    /// This is a popup that will popup on a named location on the screen.
    /// </summary>
    public class GUIElementPopup : PopupContainer
    {
        private GUIManager guiManager;
        private MyGUILayoutContainer layoutContainer;
        private LayoutElementName elementName;

        public GUIElementPopup(String layout, GUIManager guiManager, LayoutElementName elementName)
            : base(layout)
        {
            this.guiManager = guiManager;
            this.elementName = elementName;
            layoutContainer = new MyGUILayoutContainer(widget);
            layoutContainer.LayoutChanged += new Action(layoutUpdated);

            this.Showing += new EventHandler(ChooseSceneDialog_Showing);
            this.Hidden += new EventHandler(ChooseSceneDialog_Hidden);
        }

        protected virtual void layoutUpdated()
        {

        }

        void ChooseSceneDialog_Hidden(object sender, EventArgs e)
        {
            guiManager.closeElement(elementName, layoutContainer);
        }

        void ChooseSceneDialog_Showing(object sender, EventArgs e)
        {
            guiManager.changeElement(elementName, layoutContainer, null);
        }
    }
}
