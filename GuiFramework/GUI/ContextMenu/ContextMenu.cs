using Engine;
using Engine.Threads;
using MyGUIPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anomalous.GuiFramework
{
    public class ContextMenu
    {
        private List<ContextMenuItem> menuItems = new List<ContextMenuItem>();

        public void add(ContextMenuItem item)
        {
            menuItems.Add(item);
        }

        public void showMenu(IntVector2 loc)
        {
            PopupMenu popupMenu = (PopupMenu)Gui.Instance.createWidgetT("PopupMenu", "PopupMenu", 0, 0, 1, 1, Align.Default, "Overlapped", "");
            popupMenu.Visible = false;
            popupMenu.ItemAccept += new MyGUIEvent(popupMenu_ItemAccept);
            popupMenu.Closed += new MyGUIEvent(popupMenu_Closed);
            foreach (ContextMenuItem item in menuItems)
            {
                MenuItem menuItem = popupMenu.addItem(item.Text, MenuItemType.Normal, item.Text);
                menuItem.UserObject = item;
            }
            LayerManager.Instance.upLayerItem(popupMenu);
            popupMenu.setPosition(loc.x, loc.y);
            popupMenu.ensureVisible();
            popupMenu.setVisibleSmooth(true);
        }

        void popupMenu_ItemAccept(Widget source, EventArgs e)
        {
            MenuCtrlAcceptEventArgs mcae = (MenuCtrlAcceptEventArgs)e;
            ((ContextMenuItem)mcae.Item.UserObject).execute();
        }

        void popupMenu_Closed(Widget source, EventArgs e)
        {
            ThreadManager.invoke(() => Gui.Instance.destroyWidget(source));
        }
    }
}
