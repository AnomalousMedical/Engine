using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Engine.Platform;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;

namespace Editor
{
    public partial class EditorMainForm : Form, OSWindow
    {
        private List<OSWindowListener> listeners = new List<OSWindowListener>();
        private String windowDefaultText;
        private const String TITLE_FORMAT = "{0} - {1}";
        private DockPanel dockPanel;

        public EditorMainForm()
        {
            InitializeComponent();
        }

        protected void setDockPanel(DockPanel dockPanel)
        {
            this.dockPanel = dockPanel;
            windowDefaultText = this.Text;
        }

        public void saveWindows(String filename)
        {
            if (dockPanel == null)
            {
                throw new Exception("You must call setDockPanel before calling saveWindows.");
            }
            else
            {
                dockPanel.SaveAsXml(filename);
            }
        }

        public bool restoreWindows(String filename, DeserializeDockContent callback)
        {
            if (dockPanel == null)
            {
                throw new Exception("You must call restoreWindows before calling saveWindows.");
            }
            else
            {
                bool restore = File.Exists(filename);
                if (restore)
                {
                    dockPanel.LoadFromXml(filename, callback);
                }
                return restore;
            }
        }

        #region OSWindow Members

        public IntPtr WindowHandle
        {
            get
            {
                return this.Handle;
            }
        }

        public int WindowWidth
        {
            get
            {
                return this.Width;
            }
        }

        public int WindowHeight
        {
            get
            {
                return this.Height;
            }
        }

        public void addListener(OSWindowListener listener)
        {
            listeners.Add(listener);
        }

        public void removeListener(OSWindowListener listener)
        {
            listeners.Remove(listener);
        }

        #endregion

        /// <summary>
        /// Update the title of the window to reflect a current filename or other info.
        /// </summary>
        /// <param name="subName">A name to place as a secondary name in the title.</param>
        protected void updateWindowTitle(String subName)
        {
            Text = String.Format(TITLE_FORMAT, windowDefaultText, subName);
        }

        /// <summary>
        /// Clear the window title back to the default text.
        /// </summary>
        protected void clearWindowTitle()
        {
            Text = windowDefaultText;
        }

        protected override void OnResize(EventArgs e)
        {
            foreach (OSWindowListener listener in listeners)
            {
                listener.resized(this);
            }
            base.OnResize(e);
        }

        protected override void OnMove(EventArgs e)
        {
            foreach (OSWindowListener listener in listeners)
            {
                listener.moved(this);
            }
            base.OnMove(e);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            foreach (OSWindowListener listener in listeners)
            {
                listener.closing(this);
            }
            base.OnHandleDestroyed(e);
        }
    }
}
