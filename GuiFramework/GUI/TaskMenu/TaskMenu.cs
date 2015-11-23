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
                return NaturalSortAlgorithm.CompareFunc(xItem.Name, yItem.Name);
            }
        }

        private static readonly int AdPadding = ScaleHelper.Scaled(10);
        public const String SearchResultsCategory = "Search Results";

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

        private EditBox searchBox;
        private List<ButtonGridItem> searchResults = new List<ButtonGridItem>(20);
        private List<ButtonGridItem> realTaskItems = new List<ButtonGridItem>(20);

        private String highlightSectionTitle = null;
        private List<ButtonGridItem> highlightCategoryItems = new List<ButtonGridItem>(20);

        private TaskMenuPositioner taskMenuPositioner = new TaskMenuPositioner();

        private int scrollerBorderLeft;
        private int scrollerBorderHeight;
        private bool addGroupsToSortList = true;

        private IntVector2 dragAreaSize;
        bool allowIconDrag = false;

        public TaskMenu(DocumentController documentController, TaskController taskController, GUIManager guiManager, LayoutElementName elementName)
            : base("Anomalous.GuiFramework.GUI.TaskMenu.TaskMenu.layout", guiManager, elementName)
        {
            this.taskController = taskController;
            taskController.TaskAdded += new TaskDelegate(taskController_TaskAdded);
            taskController.TaskRemoved += new TaskRemovedDelegate(taskController_TaskRemoved);

            iconScroller = (ScrollView)widget.findWidget("IconScroller");
            iconGrid = new NoSelectButtonGrid(iconScroller, new ButtonGridGridLayout(), new TaskMenuItemComparer());

            recentDocuments = new TaskMenuRecentDocuments(widget, documentController);
            recentDocuments.DocumentClicked += new EventDelegate(recentDocuments_DocumentClicked);

            searchBox = widget.findWidget("Search") as EditBox;
            searchBox.EventEditTextChange += SearchBox_EventEditTextChange;
            searchBox.EventEditSelectAccept += SearchBox_EventEditSelectAccept;

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

            scrollerBorderLeft = iconScroller.Left;
            scrollerBorderHeight = widget.Height - iconScroller.Bottom;

            dragAreaSize = new IntVector2(ScaleHelper.Scaled(40), iconGrid.ItemHeight);
        }

        public override void Dispose()
        {
            taskController.TaskAdded -= taskController_TaskAdded;
            taskController.TaskRemoved -= taskController_TaskRemoved;
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

                if (adProvider != null)
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
                if (this.groupSorter != null)
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
                updateAdPosition();
                IntCoord viewCoord = iconScroller.ViewCoord;
                iconGrid.resizeAndLayout(viewCoord.width);
                recentDocuments.resizeAndLayout();
            }
        }

        void taskController_TaskRemoved(Task task, bool willReload)
        {
            //Unlink task
            task.IconChanged -= task_IconChanged;

            //Remove from regular items
            ButtonGridItem item = realTaskItems.Find(i => i.UserObject == task);
            realTaskItems.Remove(item);
            iconGrid.removeItem(item);

            //Remove from search results
            item = searchResults.Find(i => i.UserObject == task);
            if (item != null)
            {
                searchResults.Remove(item);
                iconGrid.removeItem(item);
            }
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
                realTaskItems.Add(item);

                //Does this task match search results
                String searchText = searchBox.OnlyText;

                if (!String.IsNullOrWhiteSpace(searchText))
                {
                    searchText = searchText.ToLowerInvariant();
                    if(task.Name.ToLowerInvariant().Contains(searchText))
                    {
                        createSearchTaskItem(task);
                    }
                }
            }
        }

        void task_IconChanged(Task task)
        {
            ButtonGridItem item = realTaskItems.Find(i => i.UserObject == task);
            if (item != null)
            {
                item.setImage(task.IconName);
            }

            item = searchResults.Find(i => i.UserObject == task);
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
            searchBox.Visible = iconScroller.Visible = viewButtonGroup.SelectedButton == tasksButton;
            recentDocuments.Visible = viewButtonGroup.SelectedButton == documentsButton;
        }

        void recentDocuments_DocumentClicked()
        {
            this.hide();
        }

        void TaskMenu_Hidden(object sender, EventArgs e)
        {
            viewButtonGroup.SelectedButton = tasksButton;
            searchBox.OnlyText = "";
            doTaskSearch();
            clearHighlightTasks();
        }

        void item_MouseButtonPressed(ButtonGridItem source, MouseEventArgs arg)
        {
            var localPosition = arg.Position;
            localPosition.x -= source.AbsoluteLeft;
            localPosition.y -= source.AbsoluteTop;

            allowIconDrag = localPosition.x < dragAreaSize.x && localPosition.y < dragAreaSize.y;

            dragMouseStartPosition = arg.Position;
        }

        void item_MouseDrag(ButtonGridItem source, MouseEventArgs arg)
        {
            if (allowIconDrag)
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
            updateIconScrollerPosition(new IntCoord(ScaleHelper.Scaled(2), iconScroller.Top, widget.Width, iconScroller.Height));
        }

        private void updateAdPosition()
        {
            if (adProvider != null && adProvider.IsAdCreated)
            {
                int right = adProvider.Right + AdPadding;
                int iconScrollerHeight = widget.Height - scrollerBorderHeight - iconScroller.Top;
                if ((float)right / widget.Width < .43f)
                {
                    adProvider.AdAlignment = TaskMenuAdProvider.Alignment.Vertical;
                    adProvider.AdRect = new IntRect(scrollerBorderLeft, iconScroller.Top, right, iconScroller.Height);
                    updateIconScrollerPosition(new IntCoord(right, iconScroller.Top, widget.Width - right, iconScrollerHeight));
                }
                else
                {
                    int top = adProvider.Top + AdPadding;
                    if ((float)top / iconScrollerHeight < .5f)
                    {
                        updateIconScrollerPosition(new IntCoord(scrollerBorderLeft, iconScroller.Top, widget.Width, iconScrollerHeight - top));
                        adProvider.AdAlignment = TaskMenuAdProvider.Alignment.Horizontal;
                        adProvider.AdRect = new IntRect(scrollerBorderLeft, iconScroller.Bottom + AdPadding, iconScroller.Width, top);
                    }
                    else
                    {
                        adProvider.AdAlignment = TaskMenuAdProvider.Alignment.Hidden;
                        updateIconScrollerPosition(new IntCoord(scrollerBorderLeft, iconScroller.Top, widget.Width, iconScrollerHeight));
                    }
                }

                adProvider.fireLayoutChanged();
            }
        }

        private void updateIconScrollerPosition(IntCoord coord)
        {
            iconScroller.setPosition(coord.left, coord.top);
            iconScroller.setSize(coord.width, coord.height);
            iconGrid.resizeAndLayout(iconScroller.ViewCoord.width);
            recentDocuments.moveAndResize(coord);
        }

        void adProvider_AdCreated(TaskMenuAdProvider adProvider)
        {
            updateAdPosition();
        }

        void iconGrid_GroupAdded(ButtonGrid arg1, string arg2)
        {
            if (addGroupsToSortList)
            {
                groupSorter.groupAdded(arg2);
            }
            iconGrid.sortGroups(groupSorter.compareGroups);
        }

        private void SearchBox_EventEditTextChange(Widget source, EventArgs e)
        {
            doTaskSearch();
        }

        private void doTaskSearch()
        {
            iconGrid.SuppressLayout = true;
            foreach (var item in searchResults)
            {
                iconGrid.removeItem(item);
            }

            searchResults.Clear();
            String searchText = searchBox.OnlyText;

            if (!String.IsNullOrWhiteSpace(searchText))
            {
                iconScroller.CanvasPosition = new IntVector2(0, 0);
                searchText = searchText.ToLowerInvariant();
                foreach (var task in taskController.Tasks.Where(t => t.ShowOnTaskMenu && t.Name.ToLowerInvariant().Contains(searchText)))
                {
                    createSearchTaskItem(task);
                }
            }

            iconGrid.SuppressLayout = false;
            iconGrid.resizeAndLayout();
        }

        public void setHighlightTasks(String highlightSectionTitle, IEnumerable<Task> tasks)
        {
            iconGrid.SuppressLayout = true;
            addGroupsToSortList = false;

            foreach (var item in highlightCategoryItems)
            {
                iconGrid.removeItem(item);
            }

            highlightCategoryItems.Clear();

            this.highlightSectionTitle = highlightSectionTitle;

            if (tasks != null)
            {
                foreach (var task in tasks)
                {
                    ButtonGridItem item = iconGrid.addItem(highlightSectionTitle, task.Name, task.IconName);
                    item.UserObject = task;
                    item.ItemClicked += new EventHandler(item_ItemClicked);
                    item.MouseButtonPressed += new EventDelegate<ButtonGridItem, MouseEventArgs>(item_MouseButtonPressed);
                    item.MouseDrag += new EventDelegate<ButtonGridItem, MouseEventArgs>(item_MouseDrag);
                    item.MouseButtonReleased += new EventDelegate<ButtonGridItem, MouseEventArgs>(item_MouseButtonReleased);
                    highlightCategoryItems.Add(item);
                }
            }

            addGroupsToSortList = true;
            iconGrid.SuppressLayout = false;
            iconGrid.resizeAndLayout();
        }

        public void clearHighlightTasks()
        {
            setHighlightTasks(null, null);
        }

        private void createSearchTaskItem(Task task)
        {
            ButtonGridItem item = iconGrid.addItem(SearchResultsCategory, task.Name, task.IconName);
            item.UserObject = task;
            item.ItemClicked += new EventHandler(item_ItemClicked);
            item.MouseButtonPressed += new EventDelegate<ButtonGridItem, MouseEventArgs>(item_MouseButtonPressed);
            item.MouseDrag += new EventDelegate<ButtonGridItem, MouseEventArgs>(item_MouseDrag);
            item.MouseButtonReleased += new EventDelegate<ButtonGridItem, MouseEventArgs>(item_MouseButtonReleased);
            searchResults.Add(item);
        }

        private void SearchBox_EventEditSelectAccept(Widget source, EventArgs e)
        {
            if(searchResults.Count > 0)
            {
                item_ItemClicked(searchResults.First(), null);
            }
        }
    }
}
