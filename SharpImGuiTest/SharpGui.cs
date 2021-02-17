﻿using DiligentEngine;
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

        public Guid HotItem { get; internal set; }
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
            HotItem = Guid.Empty;
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

        private Color ButtonHotAndActive = Color.FromARGB(0xffff0000);
        private Color ButtonHot = Color.FromARGB(0xffffffff);
        private Color ButtonActive = Color.FromARGB(0xffaaaaaa);
        private Color ButtonShadowColor = Color.FromARGB(0xff000000);

        internal bool DrawButton(Guid id, int x, int y, int width, int height)
        {
            // Check whether the button should be hot
            bool regionHit = RegionHit(x, y, width, height);
            if (regionHit)
            {
                HotItem = id;
                if (ActiveItem == Guid.Empty && MouseDown)
                {
                    ActiveItem = id;
                }
            }

            //Draw shadow
            buffer.DrawQuad(x + 8, y + 8, width, height, ButtonShadowColor);

            if (HotItem == id)
            {
                if (ActiveItem == id)
                {
                    // Button is both 'hot' and 'active'
                    buffer.DrawQuad(x, y, width, height, ButtonHotAndActive);
                }
                else
                {
                    // Button is merely 'hot'
                    buffer.DrawQuad(x, y, width, height, ButtonHot);
                }
            }
            else
            {
                // button is not hot, but it may be active    
                buffer.DrawQuad(x, y, width, height, ButtonActive);
            }

            bool clicked = false;
            if (regionHit && !MouseDown && ActiveItem == id)
            {
                clicked = true;
            }
            return clicked;
        }

        internal void Render(IDeviceContext immediateContext)
        {
            renderer.Render(buffer, immediateContext);
        }
    }
}
