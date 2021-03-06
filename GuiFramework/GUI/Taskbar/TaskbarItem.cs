﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyGUIPlugin;
using Engine;

namespace Anomalous.GuiFramework
{
    public abstract class TaskbarItem : IDisposable
    {
        protected Button taskbarButton;
        protected Taskbar taskbar;
        private String iconName;

        public TaskbarItem(String displayName, String iconName)
        {
            this.DisplayName = displayName;
            this.iconName = iconName;
        }

        public virtual void Dispose()
        {
            //Don't have to destroy the button because MyGUI will do it for us.
        }

        public abstract void clicked(Widget source, EventArgs e);

        public virtual void rightClicked(Widget source, EventArgs e)
        {
            
        }

        /// <summary>
        /// The name of the icon for this item.
        /// </summary>
        public String IconName
        {
            get
            {
                return iconName;
            }
            set
            {
                iconName = value;
                taskbarButton.ImageBox.setItemResource(value);
            }
        }

        /// <summary>
        /// The name that is displayed on the gui for this item, not used to track the item in any way.
        /// </summary>
        public String DisplayName { get; set; }

        internal void _configureForTaskbar(Taskbar taskbar, Button taskbarButton)
        {
            if (this.taskbarButton != null)
            {
                throw new Exception("This item has already been configured. Only add a TaskbarItem to one taskbar.");
            }
            this.taskbar = taskbar;
            this.taskbarButton = taskbarButton;
            taskbarButton.ImageBox.setItemResource(IconName);
            taskbarButton.MouseButtonClick += clicked;
            taskbarButton.MouseButtonReleased += new MyGUIEvent(taskbarButton_MouseButtonReleased);
            taskbarButton.NeedToolTip = true;
            taskbarButton.EventToolTip += new MyGUIEvent(taskbarButton_EventToolTip);
        }

        internal void _deconfigureForTaskbar()
        {
            if (taskbarButton != null)
            {
                Gui.Instance.destroyWidget(taskbarButton);
                taskbar = null;
                taskbarButton = null;
            }
        }

        void taskbarButton_EventToolTip(Widget source, EventArgs e)
        {
            TooltipManager.Instance.processTooltip(source, DisplayName, (ToolTipEventArgs)e);
        }

        internal void setCoord(int x, int y, int width, int height)
        {
            taskbarButton.setCoord(x, y, width, height);
        }

        internal virtual void addToPinnedTasksList(PinnedTaskSerializer pinnedTaskSerializer)
        {
            
        }

        public int AbsoluteLeft
        {
            get
            {
                return taskbarButton.AbsoluteLeft;
            }
        }

        public int AbsoluteTop
        {
            get
            {
                return taskbarButton.AbsoluteTop;
            }
        }

        public int Width
        {
            get
            {
                return taskbarButton.Width;
            }
        }

        public int Height
        {
            get
            {
                return taskbarButton.Height;
            }
        }

        protected IntVector2 findGoodPosition(int width, int height)
        {
            switch (taskbar.Alignment)
            {
                case TaskbarAlignment.Left:
                    return new IntVector2(taskbarButton.AbsoluteLeft, taskbarButton.AbsoluteTop + taskbarButton.Height);
                case TaskbarAlignment.Right:
                    return new IntVector2(taskbarButton.AbsoluteLeft - width + taskbarButton.Width, taskbarButton.AbsoluteTop + taskbarButton.Height);
                case TaskbarAlignment.Top:
                    return new IntVector2(taskbarButton.AbsoluteLeft, taskbarButton.AbsoluteTop + taskbarButton.Height);
                case TaskbarAlignment.Bottom:
                    return new IntVector2(taskbarButton.AbsoluteLeft, taskbarButton.AbsoluteTop - height);
                default:
                    return new IntVector2(taskbarButton.AbsoluteLeft, taskbarButton.AbsoluteTop - height);
            }
        }

        void taskbarButton_MouseButtonReleased(Widget source, EventArgs e)
        {
            MouseEventArgs me = e as MouseEventArgs;
            if (me.Button == Engine.Platform.MouseButtonCode.MB_BUTTON1)
            {
                rightClicked(source, e);
            }
        }
    }
}
