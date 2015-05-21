using Engine;
using Engine.Threads;
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
        private bool missingMode = false;

        public MissingTaskbarItem(String uniqueName)
            : base("Loading", "Anomalous.GuiFramework.Loading")
        {
            this.UniqueName = uniqueName;
        }

        public void setMissingMode(String iconName)
        {
            this.IconName = iconName;
            DisplayName = String.Format("Missing {0}", UniqueName);
            missingMode = true;
        }

        public override void clicked(Widget source, EventArgs e)
        {
            if(missingMode)
            {
                MessageBox.show(String.Format("The task {0} is missing. Would you like to unpin this task?", UniqueName), "Task Missing", MessageBoxStyle.IconQuest | MessageBoxStyle.Yes | MessageBoxStyle.No, result =>
                {
                    if(result == MessageBoxStyle.Yes)
                    {
                        fireRemoveFromTaskbar();
                    }
                });
            }
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

        public String UniqueName { get; private set; }

        void popupMenu_ItemAccept(Widget source, EventArgs e)
        {
            //These only have remove tasks
            fireRemoveFromTaskbar();
        }

        void popupMenu_Closed(Widget source, EventArgs e)
        {
            ThreadManager.invoke(() => Gui.Instance.destroyWidget(source));
        }

        internal override void addToPinnedTasksList(PinnedTaskSerializer pinnedTaskSerializer)
        {
            pinnedTaskSerializer.addPinnedTask(UniqueName);
        }

        private void fireRemoveFromTaskbar()
        {
            if (RemoveFromTaskbar != null)
            {
                RemoveFromTaskbar.Invoke(this);
            }
        }
    }
}
