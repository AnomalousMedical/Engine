using DiligentEngine;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpImGuiTest
{
    public class SharpGui
    {
        private Guid EmptySpace = Guid.NewGuid(); //A guid for when the user clicks on empty space. This gets considered to be active
        private readonly SharpGuiBuffer buffer;
        private readonly SharpGuiRenderer renderer;

        public SharpGui(SharpGuiBuffer buffer, SharpGuiRenderer renderer)
        {
            this.buffer = buffer;
            this.renderer = renderer;
        }

        public void SetMouseState(int x, int y, bool mouseDown)
        {
            MouseX = x;
            MouseY = y;
            MouseDown = mouseDown;
        }

        public Guid FocusItem { get; internal set; }
        public Guid ActiveItem { get; internal set; }
        public int MouseX { get; private set; }
        public int MouseY { get; private set; }
        public bool MouseDown { get; private set; }

        bool RegionHit(int x, int y, int w, int h)
        {
            return !(MouseX < x ||
                   MouseY < y ||
                   MouseX >= x + w ||
                   MouseY >= y + h);
        }

        public void Begin()
        {
            FocusItem = Guid.Empty;
            buffer.Begin();
        }

        public void End()
        {
            if (MouseDown)
            {
                //This needs to say nested, above check is just mouse up / down
                //If ActiveItem is empty at the end of the frame, consider empty space to be clicked.
                if (ActiveItem == Guid.Empty)
                {
                    ActiveItem = EmptySpace;
                }
            }
            else
            {
                ActiveItem = Guid.Empty;
            }
        }

        private Color ButtonFocusAndActive = Color.FromARGB(0xffff0000);
        private Color ButtonFocus = Color.FromARGB(0xffffffff);
        private Color ButtonNormal = Color.FromARGB(0xffaaaaaa);
        private Color ButtonShadowColor = Color.FromARGB(0xff000000);

        /// <summary>
        /// Draw a button. Returns true if the button was clicked.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public bool Button(Guid id, int x, int y, int width, int height)
        {
            // Check whether the button should be hot
            bool regionHit = RegionHit(x, y, width, height);
            if (regionHit)
            {
                FocusItem = id;
                if (ActiveItem == Guid.Empty && MouseDown)
                {
                    ActiveItem = id;
                }
            }

            //Draw shadow
            buffer.DrawQuad(x + 8, y + 8, width, height, ButtonShadowColor);

            //Draw button
            if (FocusItem == id)
            {
                if (ActiveItem == id)
                {
                    buffer.DrawQuad(x, y, width, height, ButtonFocusAndActive);
                }
                else
                {
                    buffer.DrawQuad(x, y, width, height, ButtonFocus);
                }
            }
            else
            {
                buffer.DrawQuad(x, y, width, height, ButtonNormal);
            }

            //Determine clicked
            bool clicked = false;
            if (regionHit && !MouseDown && ActiveItem == id)
            {
                clicked = true;
            }
            return clicked;
        }

        private int SliderBoxMargin = 8;
        private Color SliderBoxBackgroundColor = Color.FromARGB(0xff777777);
        private Color SliderFocusAndActive = Color.FromARGB(0xffff0000);
        private Color SliderFocus = Color.FromARGB(0xffffffff);
        private Color SliderNormal = Color.FromARGB(0xffaaaaaa);

        public bool Slider(Guid id, int x, int y, int width, int height, int max, ref int value)
        {
            var buttonX = x + SliderBoxMargin;
            var buttonY = y + SliderBoxMargin;
            int buttonWidth = width - SliderBoxMargin * 2;
            int buttonHeight = 16;

            // Calculate mouse cursor's relative y offset
            int ypos = ((height - buttonHeight - SliderBoxMargin * 2) * value) / max;
            buttonY += ypos;

            // Check for hotness
            if (RegionHit(x, y, width, height))
            {
                FocusItem = id;
                if (ActiveItem == Guid.Empty && MouseDown)
                {
                    ActiveItem = id;
                }
            }

            //if(ActiveItem == id)
            //{
            //    buttonY = MouseY;
            //    if (buttonY < y + SliderBoxMargin)
            //    {
            //        buttonY = y + buttonHeight;
            //    }
            //    else if (buttonY > y + height - SliderBoxMargin - buttonHeight)
            //    {
            //        buttonY = y + height - SliderBoxMargin - buttonHeight;
            //    }
            //}

            // Render the scrollbar
            buffer.DrawQuad(x, y, width, height, SliderBoxBackgroundColor);

            // Render scroll button
            var color = SliderNormal;
            if (FocusItem == id)
            {
                if (ActiveItem == id)
                {
                    color = SliderFocusAndActive;
                }
                else
                {
                    color = SliderFocus;
                }
            }
            buffer.DrawQuad(buttonX, buttonY, buttonWidth, buttonHeight, color);

            // Update widget value
            if (ActiveItem == id)
            {
                var scrollHeight = height - SliderBoxMargin * 2;
                int mousepos = MouseY - (y + SliderBoxMargin - buttonHeight / 2);
                if (mousepos < 0) { mousepos = 0; }
                if (mousepos > scrollHeight) { mousepos = scrollHeight; }
                Console.WriteLine(mousepos);
                int v = (mousepos * max) / scrollHeight;
                if (v != value)
                {
                    value = v;
                    return true;
                }
            }

            return false;
        }

        public void Render(IDeviceContext immediateContext)
        {
            renderer.Render(buffer, immediateContext);
        }
    }
}
