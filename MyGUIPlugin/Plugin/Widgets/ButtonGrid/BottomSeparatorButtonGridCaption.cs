using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGUIPlugin
{
    class BottomSeparatorButtonGridCaption : ButtonGridCaption
    {
        private TextBox captionText;
        private Widget separator;
        private BottomSeparatorButtonGridCaptionFactory factory;

        public BottomSeparatorButtonGridCaption(TextBox captionText, Widget separator, BottomSeparatorButtonGridCaptionFactory factory)
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

            captionText.setPosition(x, y);

            separator.setPosition(x, captionText.Bottom + factory.SeparatorTextSpace);
            separator.setSize(width, separator.Height);
        }

        public int Height
        {
            get
            {
                return separator.Bottom - captionText.Top + factory.SeparatorAdditionalPaddingTop + factory.SeparatorAdditionalPaddingBottom;
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
