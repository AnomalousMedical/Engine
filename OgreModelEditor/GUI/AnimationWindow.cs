using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using OgreWrapper;
using Engine.Platform;

namespace OgreModelEditor
{
    public partial class AnimationWindow : DockContent, UpdateListener
    {
        class AnimationWindowListItem
        {
            private AnimationState state;
            private String desc;

            public AnimationWindowListItem(AnimationState state)
            {
                this.state = state;
                desc = String.Format("{0} - {1}", state.getAnimationName(), state.getLength());
            }

            public override string ToString()
            {
                return desc;
            }

            public bool Playing
            {
                get
                {
                    return state.getEnabled();
                }
                set
                {
                    state.setEnabled(value);
                }
            }

            public void addTime(float time)
            {
                state.addTime(time);
            }
        }

        private Entity currentEntity;
        private AnimationWindowListItem playingAnimationState = null;

        public AnimationWindow()
        {
            InitializeComponent();
            animationList.MouseDoubleClick += new MouseEventHandler(animationList_MouseDoubleClick);
        }

        public void findAnimations(Entity entity)
        {
            if (playingAnimationState != null)
            {
                playingAnimationState.Playing = false;
                playingAnimationState = null;
            }
            this.currentEntity = entity;
            animationList.Items.Clear();
            AnimationStateSet animationStateSet = entity.getAllAnimationStates();
            if (animationStateSet != null)
            {
                foreach (AnimationState state in animationStateSet.AnimationStates)
                {
                    animationList.Items.Add(new AnimationWindowListItem(state));
                }
            }
        }

        void animationList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (playingAnimationState != null)
            {
                playingAnimationState.Playing = false;
            }
            if (animationList.SelectedIndex >= 0)
            {
                playingAnimationState = (AnimationWindowListItem)animationList.SelectedItem;
                if (playingAnimationState != null)
                {
                    playingAnimationState.Playing = true;
                }
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            if (playingAnimationState != null)
            {
                playingAnimationState.Playing = false;
                playingAnimationState = null;
            }
        }

        #region UpdateListener Members

        public void sendUpdate(Clock clock)
        {
            if (playingAnimationState != null)
            {
                playingAnimationState.addTime(clock.fSeconds);
            }
        }

        public void loopStarting()
        {
            
        }

        public void exceededMaxDelta()
        {
            
        }

        #endregion
    }
}
