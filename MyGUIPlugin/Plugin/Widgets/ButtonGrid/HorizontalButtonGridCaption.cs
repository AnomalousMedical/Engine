using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGUIPlugin
{
    public class HorizontalButtonGridCaption : ButtonGridCaption
    {
        private TextBox captionText;
        private Widget separator;

        public HorizontalButtonGridCaption(TextBox captionText, Widget separator)
        {
            this.captionText = captionText;
            this.separator = separator;
        }

        public void Dispose()
        {
            Gui.Instance.destroyWidget(separator);
            Gui.Instance.destroyWidget(captionText);
        }

        public void align(int x, int y, int width)
        {
            captionText.setPosition(x, y);
            separator.setPosition(x + captionText.Width, y + captionText.Height / 2);
            separator.setSize(width - captionText.Width - HorizontalButtonGridCaptionFactory.BorderWidthOffset, 1);
        }

        public int Height
        {
            get
            {
                return captionText.FontHeight;
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
