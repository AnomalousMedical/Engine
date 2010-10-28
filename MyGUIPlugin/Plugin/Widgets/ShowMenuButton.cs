﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    /// <summary>
    /// This class takes a PopupMenu and a Button and will show the PopupMenu
    /// when the button is clicked. It also will keep the menu on the screen as
    /// best as it can.
    /// </summary>
    public class ShowMenuButton
    {
        private Button button;
        private PopupMenu menu;

        public ShowMenuButton(Button button, PopupMenu menu)
        {
            this.button = button;
            this.menu = menu;

            button.MouseButtonClick += new MyGUIEvent(button_MouseButtonClick);
        }

        void button_MouseButtonClick(Widget source, EventArgs e)
        {
            int left = source.AbsoluteLeft;
            int top = source.AbsoluteTop + source.Height;
            int guiWidth = Gui.Instance.getViewWidth();
            int guiHeight = Gui.Instance.getViewHeight();

            int right = left + menu.Width;
            int bottom = top + menu.Height;

            if (right > guiWidth)
            {
                left -= right - guiWidth;
                if (left < 0)
                {
                    left = 0;
                }
            }

            if (bottom > guiHeight)
            {
                top -= bottom - guiHeight;
                if (top < 0)
                {
                    top = 0;
                }
            }

            menu.setPosition(left, top);            
            LayerManager.Instance.upLayerItem(menu);
            menu.setVisibleSmooth(true);
        }
    }
}
