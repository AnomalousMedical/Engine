using OgrePlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.SidescrollerCore
{
    class BlendedPhaseAnimation
    {
        private AnimationState startAnim;
        private AnimationState loopAnim;
        private float totalTime;
        private float startAnimLength;

        public BlendedPhaseAnimation(AnimationState startAnim, AnimationState loopAnim)
        {
            this.startAnim = startAnim;
            this.loopAnim = loopAnim;

            startAnimLength = startAnim.getLength();
        }

        public void start()
        {
            totalTime = 0;
            startAnim.setWeight(1.0f);
            startAnim.setTimePosition(0.0f);
            loopAnim.setWeight(0.0f);
            loopAnim.setTimePosition(0.0f);

            startAnim.setEnabled(true);
            loopAnim.setEnabled(true);
        }

        public void stop()
        {
            startAnim.setWeight(1.0f);
            loopAnim.setWeight(1.0f);

            startAnim.setEnabled(false);
            loopAnim.setEnabled(false);
        }

        public void update(float time)
        {
            totalTime += time;
            if(totalTime > startAnimLength)
            {
                startAnim.setEnabled(false);
                loopAnim.setWeight(1.0f);
                loopAnim.addTime(time);
            }
            else
            {
                float weightDistrib = totalTime / startAnimLength;
                startAnim.setWeight(1 - weightDistrib);
                loopAnim.setWeight(weightDistrib);
                startAnim.addTime(time);
                loopAnim.addTime(time);
            }
        }
    }
}
