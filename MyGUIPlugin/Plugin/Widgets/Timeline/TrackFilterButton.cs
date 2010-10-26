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
        private StaticText text;

        public event EventHandler CreateButtonClicked;

        public TrackFilterButton(Button button, StaticText text, String actionType)
        {
            this.button = button;
            button.MouseButtonClick += new MyGUIEvent(button_MouseButtonClick);
            this.text = text;
            text.Caption = actionType;
        }

        public void Dispose()
        {
            Gui.Instance.destroyWidget(button);
        }

        public void moveButtonTop(int newPosition)
        {
            button.setPosition(button.Left, newPosition);
            text.setPosition(text.Left, newPosition);
        }

        public String Caption
        {
            get
            {
                return text.Caption;
            }
        }

        void button_MouseButtonClick(Widget source, EventArgs e)
        {
            if (CreateButtonClicked != null)
            {
                CreateButtonClicked.Invoke(this, e);
            }
        }
    }
}
