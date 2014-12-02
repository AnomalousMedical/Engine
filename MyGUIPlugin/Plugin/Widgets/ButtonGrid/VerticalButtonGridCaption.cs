using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGUIPlugin
{
    class VerticalButtonGridCaption : ButtonGridCaption
    {
        private TextBox captionText;
        private Widget separator;
        private int separatorTextSpace;

        public VerticalButtonGridCaption(TextBox captionText, Widget separator, int separatorTextSpace)
        {
            this.captionText = captionText;
            this.separator = separator;
            this.separatorTextSpace = separatorTextSpace;
        }

        public void Dispose()
        {
            Gui.Instance.destroyWidget(separator);
            Gui.Instance.destroyWidget(captionText);
        }

        public void align(int x, int y, int width)
        {
            separator.setPosition(x, y);
            separator.setSize(width, separator.Height);
            captionText.setPosition(x, y + separatorTextSpace);
        }

        public int Height
        {
            get
            {
                return captionText.Bottom - separator.Top;
            }
        }

        public bool Visible
        {
            get
            {
                return captionText.Visible;
            }
            set
            {
                captionText.Visible = value;
                separator.Visible = value;
            }
        }
    }
}
