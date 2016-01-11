using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.GuiFramework
{
    static class AnimatedLayoutContainerManager
    {
        private static List<AnimatedLayoutContainer> animatedContainers = new List<AnimatedLayoutContainer>();
        private static UpdateTimer timer;
        private static Updater updater = new Updater();
        private static int updateIndex = 0;

        internal static UpdateTimer Timer
        {
            get
            {
                return timer;
            }
            set
            {
                if (timer != null && animatedContainers.Count > 0)
                {
                    timer.removeUpdateListener(updater);
                }
                timer = value;
                if(timer != null && animatedContainers.Count > 0)
                {
                    timer.addUpdateListener(updater);
                }
            }
        }

        public static void AddAnimatedContainer(AnimatedLayoutContainer container)
        {
            animatedContainers.Add(container);
            if(animatedContainers.Count == 1)
            {
                timer.addUpdateListener(updater);
            }
        }

        public static void RemoveAnimatedContainer(AnimatedLayoutContainer container)
        {
            int index = animatedContainers.IndexOf(container);
            if (index != -1)
            {
                animatedContainers.RemoveAt(index);
                //Adjust the iteration index backwards if the element being removed is before or on the index.
                //This way nothing gets skipped.
                if (index <= updateIndex)
                {
                    --updateIndex;
                }
            }

            if (animatedContainers.Count == 0)
            {
                timer.removeUpdateListener(updater);
            }
        }

        private static void Update(Clock clock)
        {
            for (updateIndex = 0; updateIndex < animatedContainers.Count; updateIndex++)
            {
                animatedContainers[updateIndex].update(clock);
            }
            if (animatedContainers.Count > 0)
            {
                animatedContainers[0].invalidate();
            }
            for (updateIndex = 0; updateIndex < animatedContainers.Count; updateIndex++)
            {
                animatedContainers[updateIndex].updateAfterLayout(clock);
            }
        }

        class Updater : UpdateListener
        {
            public void exceededMaxDelta()
            {

            }

            public void loopStarting()
            {

            }

            public void sendUpdate(Clock clock)
            {
                AnimatedLayoutContainerManager.Update(clock);
            }
        }
    }
}
