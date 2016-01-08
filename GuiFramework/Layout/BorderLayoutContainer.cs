using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace Anomalous.GuiFramework
{
    public enum BorderLayoutLocations
    {
        Left,
        Right,
        Top,
        Bottom,
        Center
    }

    /// <summary>
    /// Creates a border layout. The top and bottom will fill the width and
    /// height of the screen and left, right and center will be sandwiched in
    /// between.
    /// </summary>
    public class BorderLayoutContainer : LayoutContainer
    {
        private LayoutContainer left;
        private LayoutContainer right;
        private LayoutContainer top;
        private LayoutContainer bottom;
        private LayoutContainer center;

        private bool visible = true;
        private bool compactMode = false;

        public BorderLayoutContainer(String name)
        {
            LeftElementName = new BorderLayoutElementName(name, BorderLayoutLocations.Left);
            RightElementName = new BorderLayoutElementName(name, BorderLayoutLocations.Right);
            TopElementName = new BorderLayoutElementName(name, BorderLayoutLocations.Top);
            BottomElementName = new BorderLayoutElementName(name, BorderLayoutLocations.Bottom);
            CenterElementName = new BorderLayoutElementName(name, BorderLayoutLocations.Center);
        }

        public override void setAlpha(float alpha)
        {
            //Top
            if (top != null)
            {
                top.setAlpha(alpha);
            }

            //Bottom
            if (bottom != null)
            {
                bottom.setAlpha(alpha);
            }

            //Left
            if (left != null)
            {
                left.setAlpha(alpha);
            }

            //Center
            if (center != null)
            {
                center.setAlpha(alpha);
            }

            //Right
            if (right != null)
            {
                right.setAlpha(alpha);
            }
        }

        public override void layout()
        {
            IntSize2 leftDesired;
            IntSize2 rightDesired;
            IntSize2 topDesired;
            IntSize2 bottomDesired;
            IntSize2 centerSize;

            getDesiredSizes(out leftDesired, out rightDesired, out topDesired, out bottomDesired);
            doResponsiveLayout(ref leftDesired, ref rightDesired, ref topDesired, ref bottomDesired, out centerSize);

            if (compactMode)
            {
                IntVector2 loc = this.Location;
                //Top
                if (top != null)
                {
                    top.Location = loc;
                    top.WorkingSize = new IntSize2(WorkingSize.Width, topDesired.Height);
                    top.layout();

                    loc.y += topDesired.Height;
                }

                //Right
                if (right != null)
                {
                    right.Location = loc;
                    right.WorkingSize = new IntSize2(WorkingSize.Width, rightDesired.Height);
                    right.layout();

                    loc.y += rightDesired.Height;
                }

                //Center
                if (center != null)
                {
                    center.Location = loc;
                    center.WorkingSize = centerSize;
                    center.layout();

                    loc.y += centerSize.Height;
                }

                //Left
                if (left != null)
                {
                    left.Location = loc;
                    left.WorkingSize = new IntSize2(WorkingSize.Width, leftDesired.Height);
                    left.layout();

                    loc.y += leftDesired.Height;
                }

                //Bottom
                if (bottom != null)
                {
                    bottom.Location = loc;
                    bottom.WorkingSize = new IntSize2(WorkingSize.Width, bottomDesired.Height);
                    bottom.layout();
                }
            }
            else
            {
                //Top
                if (top != null)
                {
                    top.Location = this.Location;
                    top.WorkingSize = new IntSize2(WorkingSize.Width, topDesired.Height);
                    top.layout();
                }

                //Bottom
                if (bottom != null)
                {
                    bottom.Location = new IntVector2(this.Location.x, this.Location.y + topDesired.Height + centerSize.Height);
                    bottom.WorkingSize = new IntSize2(WorkingSize.Width, bottomDesired.Height);
                    bottom.layout();
                }

                //Left
                if (left != null)
                {
                    left.Location = new IntVector2(this.Location.x, this.Location.y + topDesired.Height);
                    left.WorkingSize = new IntSize2(leftDesired.Width, centerSize.Height);
                    left.layout();
                }

                //Center
                if (center != null)
                {
                    center.Location = new IntVector2(this.Location.x + leftDesired.Width, this.Location.y + topDesired.Height);
                    center.WorkingSize = centerSize;
                    center.layout();
                }

                //Right
                if (right != null)
                {
                    right.Location = new IntVector2(this.Location.x + leftDesired.Width + centerSize.Width, this.Location.y + topDesired.Height);
                    right.WorkingSize = new IntSize2(rightDesired.Width, centerSize.Height);
                    right.layout();
                }
            }
        }

        public override IntSize2 DesiredSize
        {
            get
            {
                IntSize2 desiredSize = new IntSize2();
                IntSize2 leftDesired;
                IntSize2 rightDesired;
                IntSize2 topDesired;
                IntSize2 bottomDesired;

                getDesiredSizes(out leftDesired, out rightDesired, out topDesired, out bottomDesired);

                if(leftDesired.Height > rightDesired.Height)
                {
                    desiredSize.Height = leftDesired.Height + topDesired.Height + bottomDesired.Height;
                }
                else
                {
                    desiredSize.Height = rightDesired.Height + topDesired.Height + bottomDesired.Height;
                }
                if(topDesired.Width > bottomDesired.Width)
                {
                    desiredSize.Width = topDesired.Width;
                }
                else
                {
                    desiredSize.Width = bottomDesired.Width;
                }
                return desiredSize;
            }
        }

        public bool CompactMode
        {
            get
            {
                return compactMode;
            }
            set
            {
                if(compactMode != value)
                {
                    compactMode = value;
                    if(left != null)
                    {
                        left.Orientation = compactMode ? LayoutType.Vertical : LayoutType.Horizontal;
                    }
                    if (right != null)
                    {
                        right.Orientation = compactMode ? LayoutType.Vertical : LayoutType.Horizontal;
                    }
                }
            }
        }

        private int getNumberOfPeers(LayoutElementName elementName)
        {
            int elementCount = 0;
            if(elementName == LeftElementName)
            {
                elementCount += countContainer(right);
                if(compactMode)
                {
                    elementCount += countContainer(top);
                    elementCount += countContainer(bottom);
                }
            }
            else if(elementName == RightElementName)
            {
                elementCount += countContainer(left);
                if (compactMode)
                {
                    elementCount += countContainer(top);
                    elementCount += countContainer(bottom);
                }
            }
            else if (elementName == TopElementName)
            {
                elementCount += countContainer(bottom);
                if (compactMode)
                {
                    elementCount += countContainer(left);
                    elementCount += countContainer(right);
                }
            }
            else if (elementName == BottomElementName)
            {
                elementCount += countContainer(top);
                if (compactMode)
                {
                    elementCount += countContainer(left);
                    elementCount += countContainer(right);
                }
            }
            return elementCount;
        }

        private int countContainer(LayoutContainer container)
        {
            return container != null && container.Visible ? 1 : 0;
        }

        public LayoutContainer Left
        {
            get
            {
                return left;
            }
            set
            {
                if (left != null)
                {
                    left._setParent(null);
                }
                left = value;
                if (left != null)
                {
                    left._setParent(this);
                }
                invalidate();
            }
        }

        public BorderLayoutElementName LeftElementName { get; private set; }

        public LayoutContainer Right
        {
            get
            {
                return right;
            }
            set
            {
                if (right != null)
                {
                    right._setParent(null);
                }
                right = value;
                if (right != null)
                {
                    right._setParent(this);
                }
                invalidate();
            }
        }

        public BorderLayoutElementName RightElementName { get; private set; }

        public LayoutContainer Top
        {
            get
            {
                return top;
            }
            set
            {
                if (top != null)
                {
                    top._setParent(null);
                }
                top = value;
                if (top != null)
                {
                    top._setParent(this);
                }
                invalidate();
            }
        }

        public BorderLayoutElementName TopElementName { get; private set; }

        public LayoutContainer Bottom
        {
            get
            {
                return bottom;
            }
            set
            {
                if (bottom != null)
                {
                    bottom._setParent(null);
                }
                bottom = value;
                if (bottom != null)
                {
                    bottom._setParent(this);
                }
                invalidate();
            }
        }

        public BorderLayoutElementName BottomElementName { get; private set; }

        public LayoutContainer Center
        {
            get
            {
                return center;
            }
            set
            {
                if (center != null)
                {
                    center._setParent(null);
                }
                center = value;
                if (center != null)
                {
                    center._setParent(this);
                }
                invalidate();
            }
        }

        public BorderLayoutElementName CenterElementName { get; private set; }

        public override void bringToFront()
        {
            if (center != null)
            {
                center.bringToFront();
            }
            if (left != null)
            {
                left.bringToFront();
            }
            if (right != null)
            {
                right.bringToFront();
            }
            if (top != null)
            {
                top.bringToFront();
            }
            if (bottom != null)
            {
                bottom.bringToFront();
            }
        }

        public override bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
                if (center != null)
                {
                    center.Visible = value;
                }
                if (left != null)
                {
                    left.Visible = value;
                }
                if (right != null)
                {
                    right.Visible = value;
                }
                if (top != null)
                {
                    top.Visible = value;
                }
                if (bottom != null)
                {
                    bottom.Visible = value;
                }
            }
        }

        public IntSize2 calculateFinalLeftSize(IntSize2 desiredSize)
        {
            IntSize2 leftDesired;
            IntSize2 rightDesired;
            IntSize2 topDesired;
            IntSize2 bottomDesired;
            IntSize2 centerSize;

            getDesiredRigidSizes(out leftDesired, out rightDesired, out topDesired, out bottomDesired);
            leftDesired = desiredSize;
            doResponsiveLayout(ref leftDesired, ref rightDesired, ref topDesired, ref bottomDesired, out centerSize);
            calculateFinalSizes(ref leftDesired, ref rightDesired, ref topDesired, ref bottomDesired, ref centerSize);
            return leftDesired;
        }

        public IntSize2 calculateFinalRightSize(IntSize2 desiredSize)
        {
            IntSize2 leftDesired;
            IntSize2 rightDesired;
            IntSize2 topDesired;
            IntSize2 bottomDesired;
            IntSize2 centerSize;

            getDesiredRigidSizes(out leftDesired, out rightDesired, out topDesired, out bottomDesired);
            rightDesired = desiredSize;
            doResponsiveLayout(ref leftDesired, ref rightDesired, ref topDesired, ref bottomDesired, out centerSize);
            calculateFinalSizes(ref leftDesired, ref rightDesired, ref topDesired, ref bottomDesired, ref centerSize);
            return rightDesired;
        }

        public IntSize2 calculateFinalTopSize(IntSize2 desiredSize)
        {
            IntSize2 leftDesired;
            IntSize2 rightDesired;
            IntSize2 topDesired;
            IntSize2 bottomDesired;
            IntSize2 centerSize;

            getDesiredRigidSizes(out leftDesired, out rightDesired, out topDesired, out bottomDesired);
            topDesired = desiredSize;
            doResponsiveLayout(ref leftDesired, ref rightDesired, ref topDesired, ref bottomDesired, out centerSize);
            calculateFinalSizes(ref leftDesired, ref rightDesired, ref topDesired, ref bottomDesired, ref centerSize);
            return topDesired;
        }

        public IntSize2 calculateFinalBottomSize(IntSize2 desiredSize)
        {
            IntSize2 leftDesired;
            IntSize2 rightDesired;
            IntSize2 topDesired;
            IntSize2 bottomDesired;
            IntSize2 centerSize;

            getDesiredRigidSizes(out leftDesired, out rightDesired, out topDesired, out bottomDesired);
            bottomDesired = desiredSize;
            doResponsiveLayout(ref leftDesired, ref rightDesired, ref topDesired, ref bottomDesired, out centerSize);
            calculateFinalSizes(ref leftDesired, ref rightDesired, ref topDesired, ref bottomDesired, ref centerSize);
            return bottomDesired;
        }

        private void getDesiredSizes(out IntSize2 leftDesired, out IntSize2 rightDesired, out IntSize2 topDesired, out IntSize2 bottomDesired)
        {
            leftDesired = left != null ? left.DesiredSize : new IntSize2();
            rightDesired = right != null ? right.DesiredSize : new IntSize2();
            topDesired = top != null ? top.DesiredSize : new IntSize2();
            bottomDesired = bottom != null ? bottom.DesiredSize : new IntSize2();
        }

        private void getDesiredRigidSizes(out IntSize2 leftDesired, out IntSize2 rightDesired, out IntSize2 topDesired, out IntSize2 bottomDesired)
        {
            leftDesired = left != null ? left.RigidDesiredSize : new IntSize2();
            rightDesired = right != null ? right.RigidDesiredSize : new IntSize2();
            topDesired = top != null ? top.RigidDesiredSize : new IntSize2();
            bottomDesired = bottom != null ? bottom.RigidDesiredSize : new IntSize2();
        }

        private void doResponsiveLayout(ref IntSize2 leftDesired, ref IntSize2 rightDesired, ref IntSize2 topDesired, ref IntSize2 bottomDesired, out IntSize2 centerSize)
        {
            int yPeers = 2 + getNumberOfPeers(TopElementName);

            if (compactMode)
            {
                if (leftDesired.Height > WorkingSize.Height / yPeers)
                {
                    leftDesired.Height = WorkingSize.Height / yPeers;
                }

                if (topDesired.Height > WorkingSize.Height / yPeers)
                {
                    topDesired.Height = WorkingSize.Height / yPeers;
                }

                if (rightDesired.Height > WorkingSize.Height / yPeers)
                {
                    rightDesired.Height = WorkingSize.Height / yPeers;
                }

                if (bottomDesired.Height > WorkingSize.Height / yPeers)
                {
                    bottomDesired.Height = WorkingSize.Height / yPeers;
                }

                //Determine center region size.
                centerSize = new IntSize2(WorkingSize.Width, WorkingSize.Height - topDesired.Height - bottomDesired.Height - leftDesired.Height - rightDesired.Height);
            }
            else
            {
                int xPeers = 2 + getNumberOfPeers(LeftElementName);

                if (leftDesired.Width > WorkingSize.Width / xPeers)
                {
                    leftDesired.Width = WorkingSize.Width / xPeers;
                }

                if (topDesired.Height > WorkingSize.Height / yPeers)
                {
                    topDesired.Height = WorkingSize.Height / yPeers;
                }

                if (rightDesired.Width > WorkingSize.Width / xPeers)
                {
                    rightDesired.Width = WorkingSize.Width / xPeers;
                }

                if (bottomDesired.Height > WorkingSize.Height / yPeers)
                {
                    bottomDesired.Height = WorkingSize.Height / yPeers;
                }

                //Determine center region size.
                centerSize = new IntSize2(WorkingSize.Width - leftDesired.Width - rightDesired.Width, WorkingSize.Height - topDesired.Height - bottomDesired.Height);
            }
        }

        private void calculateFinalSizes(ref IntSize2 leftSize, ref IntSize2 rightSize, ref IntSize2 topSize, ref IntSize2 bottomSize, ref IntSize2 centerSize)
        {
            if (compactMode)
            {
                //Top
                if (top != null)
                {
                    topSize = new IntSize2(WorkingSize.Width, topSize.Height);
                }

                //Right
                if (right != null)
                {
                    rightSize = new IntSize2(WorkingSize.Width, rightSize.Height);
                }

                //Left
                if (left != null)
                {
                    leftSize = new IntSize2(WorkingSize.Width, leftSize.Height);
                }

                //Bottom
                if (bottom != null)
                {
                    bottomSize = new IntSize2(WorkingSize.Width, bottomSize.Height);
                }
            }
            else
            {
                //Top
                if (top != null)
                {
                    topSize = new IntSize2(WorkingSize.Width, topSize.Height);
                }

                //Bottom
                if (bottom != null)
                {
                    bottomSize = new IntSize2(WorkingSize.Width, bottomSize.Height);
                }

                //Left
                if (left != null)
                {
                    leftSize = new IntSize2(leftSize.Width, centerSize.Height);
                }

                //Right
                if (right != null)
                {
                    rightSize = new IntSize2(rightSize.Width, centerSize.Height);
                }
            }
        }
    }
}
