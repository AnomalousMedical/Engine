using Engine;
using MyGUIPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.GuiFramework
{
    public class MyGUISingleChildLayoutContainer : SingleChildLayoutContainer
    {
        private Widget widget;
        private LayoutContainer child;
        private TaskbarAlignment alignment = TaskbarAlignment.Top;

        public MyGUISingleChildLayoutContainer(Widget widget)
        {
            this.widget = widget;
        }

        public override void bringToFront()
        {
            LayerManager.Instance.upLayerItem(widget);
        }

        public override void setAlpha(float alpha)
        {
            widget.Alpha = alpha;
        }

        public override void layout()
        {
            if (Visible)
            {
                if (Alignment == TaskbarAlignment.Left)
                {
                    widget.setCoord(Location.x, Location.y, widget.Width, WorkingSize.Height);

                    if (Child != null)
                    {
                        Child.Location = new IntVector2(Location.x + widget.Width, Location.y);
                        Child.WorkingSize = new IntSize2(WorkingSize.Width - widget.Width, WorkingSize.Height);
                        Child.layout();
                    }
                }
                else if (Alignment == TaskbarAlignment.Right)
                {
                    widget.setCoord(WorkingSize.Width - widget.Width, Location.y, widget.Width, WorkingSize.Height);

                    if (Child != null)
                    {
                        Child.Location = Location;
                        Child.WorkingSize = new IntSize2(WorkingSize.Width - widget.Width, WorkingSize.Height);
                        Child.layout();
                    }
                }
                else if (Alignment == TaskbarAlignment.Top)
                {
                    widget.setCoord(Location.x, Location.y, WorkingSize.Width, widget.Height);

                    if (Child != null)
                    {
                        Child.Location = new IntVector2(Location.x, Location.y + widget.Height);
                        Child.WorkingSize = new IntSize2(WorkingSize.Width, WorkingSize.Height - widget.Height);
                        Child.layout();
                    }
                }
                else if (Alignment == TaskbarAlignment.Bottom)
                {
                    widget.setCoord(Location.x, WorkingSize.Height - widget.Height, WorkingSize.Width, widget.Height);

                    if (Child != null)
                    {
                        Child.Location = Location;
                        Child.WorkingSize = new IntSize2(WorkingSize.Width, WorkingSize.Height - widget.Height);
                        Child.layout();
                    }
                }
            }
            else
            {
                if (Child != null)
                {
                    Child.Location = Location;
                    Child.WorkingSize = WorkingSize;
                    Child.layout();
                }
            }
        }

        public override IntSize2 DesiredSize
        {
            get
            {
                return new IntSize2(widget.Width, widget.Height);
            }
        }

        public override bool Visible
        {
            get
            {
                return widget.Visible;
            }
            set
            {
                widget.Visible = value;
                invalidate();
            }
        }

        public override LayoutContainer Child
        {
            get
            {
                return child;
            }
            set
            {
                if (child != null)
                {
                    child._setParent(null);
                }
                child = value;
                if (child != null)
                {
                    child._setParent(this);
                }
                invalidate();
            }
        }

        public TaskbarAlignment Alignment
        {
            get
            {
                return alignment;
            }
            set
            {
                if (alignment != value)
                {
                    alignment = value;
                    layout();
                }
            }
        }
    }
}
