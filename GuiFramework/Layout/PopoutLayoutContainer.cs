using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using Engine;

namespace Anomalous.GuiFramework
{
    public class PopoutLayoutContainer : AnimatedLayoutContainer, UpdateListener
    {
        public delegate IntSize2 ComputeWorkingSizeDelegate(IntSize2 desiredSize);

        private UpdateTimer mainTimer;
        private LayoutContainer childContainer;
        private LayoutContainer oldChildContainer;

        private float animationLength;
        private float currentTime;
        private bool animating = false;
        private float alpha = 1.0f;
        private IntSize2 oldSize;
        private IntSize2 newSize;
        private IntSize2 sizeDelta;
        private IntSize2 currentSize;
        private EasingFunction currentEasing = EasingFunction.None;
        private OrientationStrategy orientationStrategy;
        private LayoutType orientation;
        private ComputeWorkingSizeDelegate computeWorkingSize;

        public PopoutLayoutContainer(UpdateTimer mainTimer, LayoutType orientation, ComputeWorkingSizeDelegate computeWorkingSize = null)
        {
            this.mainTimer = mainTimer;
            this.orientation = orientation;
            this.computeWorkingSize = computeWorkingSize;
            createOrientationStrategy();
        }

        public override void Dispose()
        {
            if (animating)
            {
                finishAnimation();
            }
        }

        public override void setAlpha(float alpha)
        {

        }

        public override void layout()
        {
            if (animating)
            {
                if (childContainer != null)
                {
                    childContainer.Location = Location;
                    childContainer.WorkingSize = currentSize;
                    childContainer.layout();
                }
                if (oldChildContainer != null)
                {
                    oldChildContainer.Location = Location;
                    oldChildContainer.WorkingSize = currentSize;
                    oldChildContainer.layout();
                }
            }
            else
            {
                if (childContainer != null)
                {
                    childContainer.Location = Location;
                    childContainer.WorkingSize = WorkingSize;
                    childContainer.layout();
                }
            }
        }

        /// <summary>
        /// Set the panel that will be displayed when this container is first shown
        /// </summary>
        /// <param name="childContainer"></param>
        public void setInitialPanel(LayoutContainer childContainer)
        {
            currentSize = childContainer.DesiredSize;
            this.childContainer = childContainer;
            invalidate();
        }

        public override void changePanel(LayoutContainer childContainer, float animDuration)
        {
            //If we were animating when a new request comes in clear the old animation first.
            if (animating)
            {
                if (this.childContainer != null)
                {
                    this.childContainer.setAlpha(1.0f);
                    this.childContainer.WorkingSize = newSize;
                    this.childContainer.layout();
                    finishAnimation();
                }
                else
                {
                    //If we were transitioning to null, but now there is another container use the child that was being transitioned
                    this.childContainer = oldChildContainer;
                    unsubscribeFromUpdates();
                }
            }

            currentTime = 0.0f;
            animationLength = animDuration;

            oldChildContainer = this.childContainer;
            if (oldChildContainer != null)
            {
                oldSize = oldChildContainer.DesiredSize;
                oldChildContainer.animatedResizeStarted(orientationStrategy.getOrientedSize(oldSize, WorkingSize));
            }
            else
            {
                oldSize = new IntSize2(0, 0);
            }

            this.childContainer = childContainer;
            if (childContainer != null)
            {
                childContainer._setParent(this);

                //Compute the final working size of this container
                newSize = childContainer.DesiredSize;
                IntSize2 finalWorkingSize = WorkingSize;
                if(computeWorkingSize != null)
                {
                    finalWorkingSize = computeWorkingSize(newSize);
                }
                childContainer.animatedResizeStarted(finalWorkingSize);

                //Force the child container to fit in the current alloted space
                childContainer.Location = Location;
                childContainer.WorkingSize = orientationStrategy.getOrientedSize(oldSize, WorkingSize);
                childContainer.layout();
            }
            else
            {
                newSize = new IntSize2(0, 0);
            }

            sizeDelta = newSize - oldSize;

            if (orientationStrategy.isMajorAxisZero(oldSize))
            {
                currentEasing = EasingFunction.EaseOutQuadratic;
            }
            else if (orientationStrategy.isMajorAxisZero(newSize))
            {
                currentEasing = EasingFunction.EaseInQuadratic;
            }
            else
            {
                currentEasing = EasingFunction.EaseInOutQuadratic;
            }

            //Make sure we start with no alpha if blending
            if (childContainer != null && oldChildContainer != null)
            {
                childContainer.setAlpha(0.0f);
            }

            subscribeToUpdates();
        }

        public override IntSize2 DesiredSize
        {
            get
            {
                if (animating)
                {
                    return currentSize;
                }
                else
                {
                    if (childContainer != null)
                    {
                        return childContainer.DesiredSize;
                    }
                    else
                    {
                        return new IntSize2();
                    }
                }
            }
        }

        public override IntSize2 RigidDesiredSize
        {
            get
            {
                if (childContainer != null)
                {
                    return childContainer.DesiredSize;
                }
                else
                {
                    return new IntSize2();
                }
            }
        }

        public void exceededMaxDelta()
        {

        }

        public void loopStarting()
        {

        }

        public void sendUpdate(Clock clock)
        {
            if (animating)
            {
                currentTime += clock.DeltaSeconds;
                if (currentTime < animationLength)
                {
                    alpha = EasingFunctions.Ease(currentEasing, 0, 1.0f, currentTime, animationLength);
                    currentSize = orientationStrategy.getOrientedResize(alpha);
                    invalidate();
                }
                else
                {
                    currentTime = animationLength;
                    alpha = 1.0f;
                    currentSize = orientationStrategy.getOrientedFinalSize();
                    invalidate();
                    finishAnimation();
                    oldChildContainer = null;
                }
                if (childContainer != null)
                {
                    childContainer.setAlpha(alpha);
                }
            }
        }

        private void finishAnimation()
        {
            //reset the old child
            if (oldChildContainer != null)
            {
                oldChildContainer._setParent(null);
                oldChildContainer.setAlpha(1.0f);
                oldChildContainer.WorkingSize = oldSize;
                oldChildContainer.animatedResizeCompleted(oldSize);
                oldChildContainer.layout();
            }
            fireAnimationComplete(oldChildContainer);
            if (childContainer != null)
            {
                childContainer.animatedResizeCompleted(currentSize);
                childContainer.WorkingSize = WorkingSize;
                childContainer.layout();
            }
            unsubscribeFromUpdates();
        }

        public override void bringToFront()
        {
            if (childContainer != null)
            {
                childContainer.bringToFront();
            }
        }

        public override bool Visible
        {
            get
            {
                if (childContainer != null)
                {
                    return childContainer.Visible;
                }
                return false;
            }
            set
            {
                if (childContainer != null)
                {
                    childContainer.Visible = value;
                }
            }
        }

        public override LayoutContainer CurrentContainer
        {
            get
            {
                return childContainer;
            }
        }

        public override LayoutType Orientation
        {
            get
            {
                return orientation;
            }
            set
            {
                if(orientation != value)
                {
                    orientation = value;
                    createOrientationStrategy();
                }
            }
        }

        private void subscribeToUpdates()
        {
            animating = true;
            mainTimer.addUpdateListener(this);
        }

        private void unsubscribeFromUpdates()
        {
            animating = false;
            mainTimer.removeUpdateListener(this);
        }

        private void createOrientationStrategy()
        {
            switch (orientation)
            {
                case LayoutType.Horizontal:
                    this.orientationStrategy = new HorizontalPopoutStrategy(this);
                    break;
                case LayoutType.Vertical:
                    this.orientationStrategy = new VerticalPopoutStrategy(this);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        interface OrientationStrategy
        {
            IntSize2 getOrientedSize(IntSize2 size, IntSize2 workingSize);

            bool isMajorAxisZero(IntSize2 size);

            IntSize2 getOrientedResize(float alpha);

            IntSize2 getOrientedFinalSize();
        }

        class HorizontalPopoutStrategy : OrientationStrategy
        {
            PopoutLayoutContainer container;

            public HorizontalPopoutStrategy(PopoutLayoutContainer container)
            {
                this.container = container;
            }

            public IntSize2 getOrientedSize(IntSize2 size, IntSize2 workingSize)
            {
                return new IntSize2(size.Width, workingSize.Height);
            }

            public bool isMajorAxisZero(IntSize2 size)
            {
                return size.Width == 0;
            }

            public IntSize2 getOrientedResize(float alpha)
            {
                return new IntSize2((int)(container.oldSize.Width + container.sizeDelta.Width * alpha), container.WorkingSize.Height);
            }

            public IntSize2 getOrientedFinalSize()
            {
                return new IntSize2(container.oldSize.Width + container.sizeDelta.Width, container.WorkingSize.Height);
            }
        }

        class VerticalPopoutStrategy : OrientationStrategy
        {
            PopoutLayoutContainer container;

            public VerticalPopoutStrategy(PopoutLayoutContainer container)
            {
                this.container = container;
            }

            public IntSize2 getOrientedSize(IntSize2 size, IntSize2 workingSize)
            {
                return new IntSize2(workingSize.Width, size.Height);
            }

            public bool isMajorAxisZero(IntSize2 size)
            {
                return size.Height == 0;
            }

            public IntSize2 getOrientedResize(float alpha)
            {
                return new IntSize2(container.WorkingSize.Width, (int)(container.oldSize.Height + container.sizeDelta.Height * alpha));
            }

            public IntSize2 getOrientedFinalSize()
            {
                return new IntSize2(container.WorkingSize.Width, container.oldSize.Height + container.sizeDelta.Height);
            }
        }
    }
}
