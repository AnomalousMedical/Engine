using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Editor
{
    class TreeDblClickPrevent
    {
        private TreeView treeView;
        private long mouseDownTime;
        private bool doubleClick = false;
        public TreeDblClickPrevent(TreeView tv)
        {
            treeView = tv;
            tv.BeforeExpand += new TreeViewCancelEventHandler(tv_BeforeExpand);
            tv.MouseDown += new MouseEventHandler(tv_MouseDown);
            tv.BeforeCollapse += new TreeViewCancelEventHandler(tv_BeforeCollapse);
        }

        private void tv_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (doubleClick == true && e.Action == TreeViewAction.Expand)
            {
                e.Cancel = true;
            }
            else
            {
                mouseDownTime = 0;
            }
        }

        private void tv_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (doubleClick == true && e.Action == TreeViewAction.Collapse)
            {
                e.Cancel = true;
            }
            else
            {
                mouseDownTime = 0;
            }
        }

        private void tv_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (System.DateTime.Now.Ticks - mouseDownTime <= (SystemInformation.DoubleClickTime * TimeSpan.TicksPerMillisecond))
            {
                doubleClick = true;
            }
            else
            {
                doubleClick = false;
            }
            mouseDownTime = System.DateTime.Now.Ticks;
        }
    }
}
