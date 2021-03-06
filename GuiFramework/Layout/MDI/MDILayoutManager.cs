﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Platform;
using MyGUIPlugin;

namespace Anomalous.GuiFramework
{
    public enum DockLocation
    {
        None = 0,
        Left = 1 << 0,
        Right = 1 << 1,
        Top = 1 << 2,
        Bottom = 1 << 3,
        Center = 1 << 4,
        Floating = 1 << 5,
        All = Left | Top | Right | Bottom | Center | Floating,
    }

    /// <summary>
    /// This class provides a MDI-like interface for hosting multiple windows at once.
    /// </summary>
    public class MDILayoutManager : LayoutContainer, IDisposable
    {
        public event EventHandler ActiveWindowChanged;

        private List<MDIWindow> windows = new List<MDIWindow>();
        private MDIBorderContainer rootContainer;
        private MDIWindow activeWindow = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public MDILayoutManager()
            :this(ScaleHelper.Scaled(5))
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="padding">The amount of padding between elements.</param>
        public MDILayoutManager(int padding)
        {
            rootContainer = new MDIBorderContainer(padding);
            rootContainer._setParent(this);
            AllowActiveWindowChanges = true;
        }

        /// <summary>
        /// Dispose method.
        /// </summary>
        public void Dispose()
        {
            rootContainer.Dispose();
        }

        /// <summary>
        /// Show a window where the location does not matter.
        /// </summary>
        /// <param name="window">The window to add.</param>
        public void showWindow(MDIWindow window)
        {
            setWindowProperties(window);
            rootContainer.addChild(window);
            invalidate();
        }

        /// <summary>
        /// Show a window relative to another window.
        /// </summary>
        /// <param name="window">The window to show.</param>
        /// <param name="previous">The window to show window relative to.</param>
        /// <param name="alignment">The alignemnt of window to previous.</param>
        public void showWindow(MDIWindow window, MDIWindow previous, WindowAlignment alignment)
        {
            if (previous == null)
            {
                throw new MDIException("Previous window cannot be null.");
            }
            setWindowProperties(window);
            previous._ParentContainer.addChild(window, previous, alignment);
            invalidate();
        }

        /// <summary>
        /// Close a window.
        /// </summary>
        /// <param name="window"></param>
        public void closeWindow(MDIWindow window)
        {
            //if (!windows.Contains(window))
            //{
            //    throw new MDIException("Attempted to close a window that is not part of this MDILayoutManager.");
            //}
            if (windows.Contains(window))
            {
                windows.Remove(window);
                //Check to see if this window was the active window.
                if (window == ActiveWindow)
                {
                    if (windows.Count > 0)
                    {

                        ActiveWindow = windows[0];
                    }
                    else
                    {
                        ActiveWindow = null;
                    }
                }
                window._ParentContainer.removeChild(window);
            }
        }

        public LayoutContainer Center
        {
            get
            {
                return rootContainer.Center;
            }
            set
            {
                rootContainer.Center = value;
            }
        }

        public MDILayoutContainer DocumentArea
        {
            get
            {
                return rootContainer.DocumentArea;
            }
        }

        /// <summary>
        /// LayoutContainer function
        /// </summary>
        public override void bringToFront()
        {
            rootContainer.bringToFront();
        }

        /// <summary>
        /// LayoutContainer function
        /// </summary>
        public override void setAlpha(float alpha)
        {
            rootContainer.setAlpha(alpha);
        }

        /// <summary>
        /// LayoutContainer function
        /// </summary>
        public override void layout()
        {
            rootContainer.WorkingSize = WorkingSize;
            rootContainer.Location = Location;
            rootContainer.layout();
        }

        /// <summary>
        /// Determine is a control widget like a separator bar is at the given coordinates.
        /// </summary>
        /// <param name="x">The x coord</param>
        /// <param name="y">The y coord</param>
        /// <returns>True if there is a control widget at the given coords and false if not.</returns>
        public bool isControlWidgetAtPosition(int x, int y)
        {
            return rootContainer.isControlWidgetAtPosition(x, y);
        }

        public StoredMDILayout storeCurrentLayout()
        {
            StoredMDILayout storedLayout = new StoredMDILayout();
            storedLayout.BorderContainer = rootContainer.storeCurrentLayout();
            return storedLayout;
        }

        public void restoreLayout(StoredMDILayout storedLayout)
        {
            rootContainer.restoreLayout(storedLayout.BorderContainer);
            this.invalidate();
        }

        /// <summary>
        /// LayoutContainer function
        /// </summary>
        public override IntSize2 DesiredSize
        {
            get 
            {
                return rootContainer.DesiredSize;
            }
        }

        /// <summary>
        /// LayoutContainer function
        /// </summary>
        public override bool Visible
        {
            get
            {
                return rootContainer.Visible;
            }
            set
            {
                rootContainer.Visible = value;
            }
        }

        /// <summary>
        /// Get or set the "Active" window in the MDI interface. This is basicly which window is focused.
        /// </summary>
        public MDIWindow ActiveWindow
        {
            get
            {
                return activeWindow;
            }
            set
            {
                if (activeWindow != value && AllowActiveWindowChanges)
                {
                    if (activeWindow != null)
                    {
                        activeWindow._doSetActive(false);
                        activeWindow.MouseDragStarted -= activeWindow_MouseDragStarted;
                        activeWindow.MouseDrag -= activeWindow_MouseDrag;
                        activeWindow.MouseDragFinished -= activeWindow_MouseDragFinished;
                    }
                    activeWindow = value;
                    if (activeWindow != null)
                    {
                        activeWindow._doSetActive(true);
                        activeWindow.MouseDragStarted += activeWindow_MouseDragStarted;
                        activeWindow.MouseDrag += activeWindow_MouseDrag;
                        activeWindow.MouseDragFinished += activeWindow_MouseDragFinished;
                    }
                    if (ActiveWindowChanged != null)
                    {
                        ActiveWindowChanged.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// True if the ActiveWindow can change, false to block changes. False
        /// will lock the active window and it will not be able to be changed by
        /// the user or code.
        /// </summary>
        public bool AllowActiveWindowChanges { get; set; }

        void activeWindow_MouseDragStarted(MDIWindow source, float mouseX, float mouseY)
        {
            rootContainer.windowDragStarted(source, mouseX, mouseY);
        }

        void activeWindow_MouseDrag(MDIWindow source, float mouseX, float mouseY)
        {
            rootContainer.windowDragged(source, mouseX, mouseY);
        }

        void activeWindow_MouseDragFinished(MDIWindow source, float mouseX, float mouseY)
        {
            rootContainer.windowDragEnded(source, mouseX, mouseY);
        }

        /// <summary>
        /// Helper function to intialize windows that are added to the MDILayoutManager.
        /// </summary>
        /// <param name="window">The window to initialize.</param>
        private void setWindowProperties(MDIWindow window)
        {
            if (windows.Count == 0)
            {
                ActiveWindow = window;
            }
            window._setMDILayoutManager(this);
            windows.Add(window);
        }
    }
}
