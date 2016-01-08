using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anomalous.GuiFramework
{
    public delegate void AnimationCompletedDelegate(LayoutContainer oldChild);

    public abstract class AnimatedLayoutContainer : LayoutContainer, IDisposable
    {
        public event AnimationCompletedDelegate AnimationComplete;
        protected bool animating = false;

        public AnimatedLayoutContainer()
        {
            Rigid = false;
        }

        public abstract void Dispose();

        public abstract void changePanel(LayoutContainer childContainer, float animDuration);

        public abstract void update(Clock clock);

        public abstract LayoutContainer CurrentContainer { get; }

        protected void fireAnimationComplete(LayoutContainer oldChild)
        {
            if (AnimationComplete != null)
            {
                AnimationComplete.Invoke(oldChild);
            }
        }

        protected void subscribeToUpdates()
        {
            animating = true;
            AnimatedLayoutContainerManager.AddAnimatedContainer(this);
        }

        protected void unsubscribeFromUpdates()
        {
            animating = false;
            AnimatedLayoutContainerManager.RemoveAnimatedContainer(this);
        }
    }
}
