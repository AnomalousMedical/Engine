using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyGUIPlugin;
using Engine;

namespace Anomalous.GuiFramework
{
    public delegate void TaskDragDropEventDelegate(Task item, IntVector2 position);

    public class TaskMenu : GUIElementPopup
    {
        private class TaskMenuItemComparer : IComparer<ButtonGridItem>
        {
            public int Compare(ButtonGridItem x, ButtonGridItem y)
            {
                Task xItem = (Task)x.UserObject;
                Task yItem = (Task)y.UserObject;
                if(xItem != null && yItem != null)
                {
                    return xItem.Weight - yItem.Weight;
                }
                return 0;
            }
        }

        private static readonly int AdPadding = ScaleHelper.Scaled(10);

        private NoSelectButtonGrid iconGrid;
        private ScrollView iconScroller;

        private TaskMenuRecentDocuments recentDocuments;

        private ButtonGroup viewButtonGroup;
        private Button tasksButton;
        private Button documentsButton;
        private TaskController taskController;

        public event TaskDelegate TaskItemOpened;
        public event TaskDragDropEventDelegate TaskItemDropped;
        public event TaskDragDropEventDelegate TaskItemDragged;

        private ImageBox dragIconPreview;
        private IntVector2 dragMouseStartPosition;
        private TaskMenuAdProvider adProvider;
        private TaskMenuGroupSorter groupSorter;

        private TaskMenuPositioner taskMenuPositioner = new TaskMenuPositioner();

        public TaskMenu(DocumentController documentController, TaskController taskController, GUIManager guiManager, LayoutElementName elementName)
            : base("Anomalous.GuiFramework.GUI.TaskMenu.TaskMenu.layout", guiManager, elementName)
        {
            this.taskController = taskController;
            taskController.TaskAdded += new TaskDelegate(taskController_TaskAdded);
            taskController.TaskRemoved += new TaskRemovedDelegate(taskController_TaskRemoved);

            iconScroller = (ScrollView)widget.findWidget("IconScroller");
            iconGrid = new NoSelectButtonGrid(iconScroller, new ButtonGridTextAdjustedGridLayout(), new TaskMenuItemComparer());

            recentDocuments = new TaskMenuRecentDocuments(widget, documentController);
            recentDocuments.DocumentClicked += new EventDelegate(recentDocuments_DocumentClicked);

            viewButtonGroup = new ButtonGroup();
            viewButtonGroup.SelectedButtonChanged += new EventHandler(viewButtonGroup_SelectedButtonChanged);
            tasksButton = (Button)widget.findWidget("Tasks");
            viewButtonGroup.addButton(tasksButton);
            documentsButton = (Button)widget.findWidget("Documents");
            viewButtonGroup.addButton(documentsButton);

            this.Hidden += new EventHandler(TaskMenu_Hidden);

            dragIconPreview = (ImageBox)Gui.Instance.createWidgetT("ImageBox", "ImageBox", 0, 0, ScaleHelper.Scaled(32), ScaleHelper.Scaled(32), Align.Default, "Info", "TaskMenuDragIconPreview");
            dragIconPreview.Visible = false;

            Button closeButton = (Button)widget.findWidget("CloseButton");
            closeButton.MouseButtonClick += new MyGUIEvent(closeButton_MouseButtonClick);
        }

        public override void Dispose()
        {
            Gui.Instance.destroyWidget(dragIconPreview);
            base.Dispose();
        }

        public void defineGroup(String name)
        {
            iconGrid.defineGroup(name);
        }

        public void setSize(int width, int height)
        {
            widget.setSize(width, height);
            IntCoord viewCoord = iconScroller.ViewCoord;
            iconGrid.resizeAndLayout(viewCoord.width);
            recentDocuments.resizeAndLayout();
        }

        public bool SuppressLayout
        {
            get
            {
                return iconGrid.SuppressLayout;
            }
            set
            {
                iconGrid.SuppressLayout = value;
            }
        }

        public TaskMenuAdProvider AdProvider
        {
            get
            {
                return adProvider;
            }
            set
            {
                if (adProvider != null)
                {
                    adProvider.AdCreated -= adProvider_AdCreated;
                    adProvider.AdDestroyed -= adProvider_AdDestroyed;
                    adProvider.ParentWidget = null;
                }

                this.adProvider = value;

                if(adProvider != null)
                {
                    adProvider.AdCreated += adProvider_AdCreated;
                    adProvider.AdDestroyed += adProvider_AdDestroyed;
                    adProvider.ParentWidget = widget;
                }
            }
        }

        public int AdTop
        {
            get
            {
                return iconScroller.Top;
            }
        }

        /// <summary>
        /// Set a group comparison function, when groups are added this will be used to sort the
        /// groups in the menu.
        /// </summary>
        public TaskMenuGroupSorter GroupComparison
        {
            get
            {
                return groupSorter;
            }
            set
            {
                if (this.groupSorter != null)
                {
                    iconGrid.GroupAdded -= iconGrid_GroupAdded;
                }
                this.groupSorter = value;
                if(this.groupSorter != null)
                {
                    iconGrid.GroupAdded += iconGrid_GroupAdded;
                }
            }
        }

        protected override void layoutUpdated()
        {
            base.layoutUpdated();
            if (Visible)
            {
                IntCoord viewCoord = iconScroller.ViewCoord;
                iconGrid.resizeAndLayout(viewCoord.width);
                recentDocuments.resizeAndLayout();
            }
        }

        void taskController_TaskRemoved(Task task, bool willReload)
        {
            task.IconChanged -= task_IconChanged;
            ButtonGridItem item = iconGrid.findItemByUserObject(task);
            iconGrid.removeItem(item);
        }

        void taskController_TaskAdded(Task task)
        {
            if (task.ShowOnTaskMenu)
            {
                task.IconChanged += task_IconChanged;
                iconGrid.SuppressLayout = true;
                ButtonGridItem item = iconGrid.addItem(task.Category, task.Name, task.IconName);
                item.UserObject = task;
                item.ItemClicked += new EventHandler(item_ItemClicked);
                task.RequestShowInTaskbar += new TaskDelegate(taskItem_RequestShowInTaskbar);
                item.MouseButtonPressed += new EventDelegate<ButtonGridItem, MouseEventArgs>(item_MouseButtonPressed);
                item.MouseDrag += new EventDelegate<ButtonGridItem, MouseEventArgs>(item_MouseDrag);
                item.MouseButtonReleased += new EventDelegate<ButtonGridItem, MouseEventArgs>(item_MouseButtonReleased);
                iconGrid.SuppressLayout = false;
                iconGrid.resizeAndLayout();
            }
        }

        void task_IconChanged(Task task)
        {
            ButtonGridItem item = iconGrid.findItemByUserObject(task);
            if (item != null)
            {
                item.setImage(task.IconName);
            }
        }

        void taskItem_RequestShowInTaskbar(Task item)
        {
            if (TaskItemOpened != null)
            {
                TaskItemOpened.Invoke(item);
            }
        }

        void item_ItemClicked(object sender, EventArgs e)
        {
            ButtonGridItem bgSender = (ButtonGridItem)sender;
            Task item = (Task)bgSender.UserObject;
            taskMenuPositioner.CurrentItem = bgSender;
            item.clicked(taskMenuPositioner);
            hide();
            if (TaskItemOpened != null)
            {
                TaskItemOpened.Invoke(item);
            }
        }

        void viewButtonGroup_SelectedButtonChanged(object sender, EventArgs e)
        {
            iconScroller.Visible = viewButtonGroup.SelectedButton == tasksButton;
            recentDocuments.Visible = viewButtonGroup.SelectedButton == documentsButton;
        }

        void recentDocuments_DocumentClicked()
        {
            this.hide();
        }

        void TaskMenu_Hidden(object sender, EventArgs e)
        {
            viewButtonGroup.SelectedButton = tasksButton;
        }

        void item_MouseButtonPressed(ButtonGridItem source, MouseEventArgs arg)
        {
            dragMouseStartPosition = arg.Position;
        }

        void item_MouseDrag(ButtonGridItem source, MouseEventArgs arg)
        {
            dragIconPreview.setPosition(arg.Position.x - (dragIconPreview.Width / 2), arg.Position.y - (int)(dragIconPreview.Height * .75f));
            if (!dragIconPreview.Visible && (Math.Abs(dragMouseStartPosition.x - arg.Position.x) > 5 || Math.Abs(dragMouseStartPosition.y - arg.Position.y) > 5))
            {
                dragIconPreview.Visible = true;
                dragIconPreview.setItemResource(((Task)source.UserObject).IconName);
                LayerManager.Instance.upLayerItem(dragIconPreview);
            }
            if (TaskItemDragged != null)
            {
                TaskItemDragged.Invoke((Task)source.UserObject, arg.Position);
            }
        }

        void item_MouseButtonReleased(ButtonGridItem source, MouseEventArgs arg)
        {
            dragIconPreview.Visible = false;
            if (TaskItemDropped != null)
            {
                TaskItemDropped.Invoke((Task)source.UserObject, arg.Position);
            }
        }

        void closeButton_MouseButtonClick(Widget source, EventArgs e)
        {
            this.hide();
        }

        void adProvider_AdDestroyed(TaskMenuAdProvider adProvider)
        {
            IntCoord coord = new IntCoord(2, iconScroller.Top, widget.Width, iconScroller.Height);
            iconScroller.setPosition(coord.left, coord.top);
            iconScroller.setSize(coord.width, coord.height);
            iconGrid.resizeAndLayout(iconScroller.ViewCoord.width);
            recentDocuments.moveAndResize(coord);
        }

        void adProvider_AdCreated(TaskMenuAdProvider adProvider)
        {
            int right = adProvider.Right + AdPadding;
            IntCoord coord = new IntCoord(right, iconScroller.Top, widget.Width - right, iconScroller.Height);
            iconScroller.setPosition(coord.left, coord.top);
            iconScroller.setSize(coord.width, coord.height);
            iconGrid.resizeAndLayout(iconScroller.ViewCoord.width);
            recentDocuments.moveAndResize(coord);
        }

        void iconGrid_GroupAdded(ButtonGrid arg1, string arg2)
        {
            groupSorter.groupAdded(arg2);
            iconGrid.sortGroups(groupSorter.compareGroups);
        }
    }
}
