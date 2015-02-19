using Engine;
using MyGUIPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.GuiFramework
{
    class MissingTaskbarItem : TaskbarItem
    {
        public event EventDelegate<MissingTaskbarItem> RemoveFromTaskbar;

        public MissingTaskbarItem(String name, String iconName)
            :base(name, iconName)
        {

        }

        public override void clicked(Widget source, EventArgs e)
        {
            
        }

        public override void rightClicked(Widget source, EventArgs e)
        {
            var popupMenu = (PopupMenu)Gui.Instance.createWidgetT("PopupMenu", "PopupMenu", 0, 0, 10, 10, Align.Default, "Info", "");
            popupMenu.Visible = false;
            MenuItem item = popupMenu.addItem("Remove");
            IntVector2 position = findGoodPosition(popupMenu.Width, popupMenu.Height);
            popupMenu.setPosition(position.x, position.y);
            popupMenu.Closed += new MyGUIEvent(popupMenu_Closed);
            popupMenu.ItemAccept += new MyGUIEvent(popupMenu_ItemAccept);
            LayerManager.Instance.upLayerItem(popupMenu);
            popupMenu.ensureVisible();
            popupMenu.setVisibleSmooth(true);
        }

        void popupMenu_ItemAccept(Widget source, EventArgs e)
        {
            //These only have remove tasks
            if (RemoveFromTaskbar != null)
            {
                RemoveFromTaskbar.Invoke(this);
            }
        }

        void popupMenu_Closed(Widget source, EventArgs e)
        {
            Gui.Instance.destroyWidget(source);
        }

        internal override void addToPinnedTasksList(PinnedTaskSerializer pinnedTaskSerializer)
        {
            pinnedTaskSerializer.addPinnedTask(Name);
        }
    }
}
