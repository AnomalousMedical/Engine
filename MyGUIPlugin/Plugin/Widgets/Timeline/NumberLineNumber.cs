﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace MyGUIPlugin
{
    class NumberLineNumber
    {
        private TextBox text;
        private Widget hashMark;
        private float time;
        private NumberLine numberLine;

        public NumberLineNumber(TextBox text, Widget hashMark, NumberLine numberLine)
        {
            this.text = text;
            text.MouseButtonClick += new MyGUIEvent(text_MouseButtonClick);
            text.NeedKeyFocus = false;
            this.hashMark = hashMark;
            hashMark.NeedKeyFocus = false;
            hashMark.NeedMouseFocus = false;
            this.numberLine = numberLine;
        }

        void text_MouseButtonClick(Widget source, EventArgs e)
        {
            numberLine.moveMarker(time);
        }

        public int Left
        {
            get
            {
                return text.Left;
            }
        }

        public int Right
        {
            get
            {
                return text.Right;
            }
        }

        public float Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
                int min = (int)(time / 60);
                int sec = (int)(time % 60);
                if (sec < 10)
                {
                    text.Caption = String.Format("{0}:0{1}", min, sec);
                }
                else
                {
                    text.Caption = String.Format("{0}:{1}", min, sec);
                }
                Size2 textSize = text.getTextSize();
                text.setSize((int)textSize.Width, (int)textSize.Height);
                text.setPosition((int)((numberLine.PixelsPerSecond * time) - text.Width / 2), text.Top);
                hashMark.setPosition((int)(numberLine.PixelsPerSecond * time), hashMark.Top);
            }
        }

        public bool Visible
        {
            get
            {
                return text.Visible;
            }
            set
            {
                text.Visible = value;
                hashMark.Visible = value;
            }
        }
    }
}
