using Engine;
using Engine.Platform;
using Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anomalous.GuiFramework
{
    public class BorderLayoutChainLink : LayoutChainLink, IDisposable
    {
        private BorderLayoutContainer borderLayout;
        private Dictionary<LayoutElementName, AnimatedLayoutContainer> panels = new Dictionary<LayoutElementName, AnimatedLayoutContainer>();
        private ActiveContainerTracker activePanels = new ActiveContainerTracker();

        public BorderLayoutChainLink(String name, UpdateTimer tempTimer)
            : base(name)
        {
            borderLayout = new BorderLayoutContainer(name);

            AnimatedLayoutContainer animatedContainer = new PopoutLayoutContainer(tempTimer, LayoutType.Horizontal, borderLayout.calculateFinalLeftSize);
            borderLayout.Left = animatedContainer;
            panels.Add(borderLayout.LeftElementName, animatedContainer);
            animatedContainer.AnimationComplete += animatedContainer_AnimationComplete;

            animatedContainer = new PopoutLayoutContainer(tempTimer, LayoutType.Horizontal, borderLayout.calculateFinalRightSize);
            borderLayout.Right = animatedContainer;
            panels.Add(borderLayout.RightElementName, animatedContainer);
            animatedContainer.AnimationComplete += animatedContainer_AnimationComplete;

            animatedContainer = new PopoutLayoutContainer(tempTimer, LayoutType.Vertical, borderLayout.calculateFinalTopSize);
            borderLayout.Top = animatedContainer;
            panels.Add(borderLayout.TopElementName, animatedContainer);
            animatedContainer.AnimationComplete += animatedContainer_AnimationComplete;

            animatedContainer = new PopoutLayoutContainer(tempTimer, LayoutType.Vertical, borderLayout.calculateFinalBottomSize);
            borderLayout.Bottom = animatedContainer;
            panels.Add(borderLayout.BottomElementName, animatedContainer);
            animatedContainer.AnimationComplete += animatedContainer_AnimationComplete;
        }

        public void Dispose()
        {
            foreach (AnimatedLayoutContainer panel in panels.Values)
            {
                if (panel.CurrentContainer != null)
                {
                    activePanels.remove(panel.CurrentContainer);
                }
                panel.Dispose();
            }
            panels.Clear();
        }

        public override void setLayoutItem(LayoutElementName elementName, LayoutContainer container, Action removedCallback)
        {
            AnimatedLayoutContainer panel;
            if (panels.TryGetValue(elementName, out panel))
            {
                if (panel.CurrentContainer != container)
                {
                    panel.changePanel(container, 0.25f);
                    activePanels.add(container, removedCallback);
                }
            }
            else
            {
                Log.Warning("Cannot add container to border layout position '{0}' because it cannot be found. Container will not be visible on UI.", elementName.Name);
                if (removedCallback != null)
                {
                    removedCallback.Invoke();
                }
            }
        }

        public override void removeLayoutItem(LayoutElementName elementName, LayoutContainer container)
        {
            AnimatedLayoutContainer panel;
            if (panels.TryGetValue(elementName, out panel))
            {
                if (panel.CurrentContainer != null)
                {
                    panel.changePanel(null, 0.25f);
                }
            }
            else
            {
                Log.Warning("Cannot remove container from border layout position '{0}' because it cannot be found. No changes made.", elementName.Name);
            }
        }

        protected internal override void _setChildContainer(LayoutContainer layoutContainer)
        {
            borderLayout.Center = layoutContainer;
        }

        public override LayoutContainer Container
        {
            get
            {
                return borderLayout;
            }
        }

        public override IEnumerable<LayoutElementName> ElementNames
        {
            get
            {
                return panels.Keys;
            }
        }

        public override bool CompactMode
        {
            get
            {
                return borderLayout.CompactMode;
            }

            set
            {
                borderLayout.CompactMode = value;
            }
        }

        void animatedContainer_AnimationComplete(LayoutContainer oldChild)
        {
            activePanels.remove(oldChild);
        }
    }
}
