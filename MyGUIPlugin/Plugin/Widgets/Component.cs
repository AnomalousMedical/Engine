using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    public abstract class Component : IDisposable
    {
        private Layout componentLayout;
        protected Widget widget;

        protected Component(String layoutFile)
        {
            componentLayout = LayoutManager.Instance.loadLayout(layoutFile);
            widget = componentLayout.getWidget(0);
        }

        protected Component(String layoutFile, Widget parent)
            :this(layoutFile)
        {
            Widget topLevelParent = parent;
            while (topLevelParent.Parent != null)
            {
                topLevelParent = topLevelParent.Parent;
            }
            bool topVisible = topLevelParent.Visible;
            topLevelParent.Visible = true;
            widget.attachToWidget(parent);
            topLevelParent.Visible = topVisible;
        }

        public virtual void Dispose()
        {
            LayoutManager.Instance.unloadLayout(componentLayout);
        }
    }
}
