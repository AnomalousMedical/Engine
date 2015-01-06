using Anomalous.GuiFramework;
using Engine.Platform;
using MyGUIPlugin;
using OgrePlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgreModelEditor
{
    class AnimationWindow : MDIDialog, UpdateListener
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
        private MultiListBox listBox;

        public AnimationWindow()
            :base("OgreModelEditor.GUI.AnimationWindow.AnimationWindow.layout")
        {
            listBox = window.findWidget("List") as MultiListBox;
            listBox.ListSelectAccept += listBox_ListSelectAccept;
            listBox.addColumn("Name", listBox.Width);
            Button stopButton = window.findWidget("Stop") as Button;
            stopButton.MouseButtonClick += stopButton_MouseButtonClick;
        }

        public void findAnimations(Entity entity)
        {
            if (playingAnimationState != null)
            {
                playingAnimationState.Playing = false;
                playingAnimationState = null;
            }
            this.currentEntity = entity;
            listBox.removeAllItems();
            AnimationStateSet animationStateSet = entity.getAllAnimationStates();
            if (animationStateSet != null)
            {
                foreach (AnimationState state in animationStateSet.AnimationStates)
                {
                    listBox.addItem(state.getAnimationName(),new AnimationWindowListItem(state));
                }
            }
        }

        void listBox_ListSelectAccept(Widget source, EventArgs e)
        {
            if (playingAnimationState != null)
            {
                playingAnimationState.Playing = false;
            }
            if (listBox.getIndexSelected() >= 0)
            {
                playingAnimationState = (AnimationWindowListItem)listBox.getItemDataAt(listBox.getIndexSelected());
                if (playingAnimationState != null)
                {
                    playingAnimationState.Playing = true;
                }
            }
        }

        void stopButton_MouseButtonClick(Widget source, EventArgs e)
        {
            if (playingAnimationState != null)
            {
                playingAnimationState.Playing = false;
                playingAnimationState = null;
            }
        }

        public void sendUpdate(Clock clock)
        {
            if (playingAnimationState != null)
            {
                playingAnimationState.addTime(clock.DeltaSeconds);
            }
        }

        public void loopStarting()
        {

        }

        public void exceededMaxDelta()
        {

        }
    }
}
