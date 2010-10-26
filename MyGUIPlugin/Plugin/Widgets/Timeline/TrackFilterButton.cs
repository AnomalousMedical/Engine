using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyGUIPlugin;

namespace MyGUIPlugin
{
    class TrackFilterButton : IDisposable
    {
        private Button button;
        private CheckButton checkButton;

        public TrackFilterButton(Button button, String actionType)
        {
            this.button = button;
            button.Caption = actionType;
            checkButton = new CheckButton(button);
            checkButton.Checked = true;
            checkButton.CheckedChanged += new MyGUIEvent(checkButton_CheckedChanged);
        }

        public void Dispose()
        {
            Gui.Instance.destroyWidget(button);
        }

        public void moveButtonTop(int newPosition)
        {
            button.setPosition(button.Left, newPosition);
        }

        void checkButton_CheckedChanged(Widget source, EventArgs e)
        {
            
        }
    }
}
