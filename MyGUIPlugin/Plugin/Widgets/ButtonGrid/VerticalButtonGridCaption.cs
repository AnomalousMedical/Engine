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
        private VerticalButtonGridCaptionFactory factory;

        public VerticalButtonGridCaption(TextBox captionText, Widget separator, VerticalButtonGridCaptionFactory factory)
        {
            this.captionText = captionText;
            this.separator = separator;
            this.factory = factory;
        }

        public void Dispose()
        {
            Gui.Instance.destroyWidget(separator);
            Gui.Instance.destroyWidget(captionText);
        }

        public void align(int x, int y, int width)
        {
            y += factory.SeparatorAdditionalPaddingTop;

            separator.setPosition(x, y);
            separator.setSize(width, separator.Height);
            captionText.setPosition(x, y + factory.SeparatorTextSpace);
        }

        public int Height
        {
            get
            {
                return captionText.Bottom - separator.Top + factory.SeparatorAdditionalPaddingTop + factory.SeparatorAdditionalPaddingBottom;
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
