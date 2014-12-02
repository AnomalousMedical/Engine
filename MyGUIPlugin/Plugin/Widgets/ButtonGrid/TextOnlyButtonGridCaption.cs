using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGUIPlugin
{
    class TextOnlyButtonGridCaption : ButtonGridCaption
    {
        private TextBox captionText;
        private TextOnlyButtonGridCaptionFactory factory;

        public TextOnlyButtonGridCaption(TextBox captionText, TextOnlyButtonGridCaptionFactory factory)
        {
            this.captionText = captionText;
            this.factory = factory;
        }

        public void Dispose()
        {
            Gui.Instance.destroyWidget(captionText);
        }

        public void align(int x, int y, int width)
        {
            y += factory.SeparatorAdditionalPaddingTop;

            captionText.setPosition(x, y);
        }

        public int Height
        {
            get
            {
                return captionText.Height + factory.SeparatorAdditionalPaddingTop + factory.SeparatorAdditionalPaddingBottom;
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
            }
        }
    }
}
