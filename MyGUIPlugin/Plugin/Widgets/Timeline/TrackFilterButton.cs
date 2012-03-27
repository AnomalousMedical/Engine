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
        private TextBox text;

        public event EventHandler CreateButtonClicked;

        public TrackFilterButton(Button button, TextBox text, String actionType)
        {
            this.button = button;
            button.MouseButtonClick += new MyGUIEvent(button_MouseButtonClick);
            this.text = text;
            text.Caption = actionType;
        }

        public void Dispose()
        {
            Gui.Instance.destroyWidget(button);
            Gui.Instance.destroyWidget(text);
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

        public bool Enabled
        {
            get
            {
                return button.Enabled;
            }
            set
            {
                button.Enabled = value;
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
