using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    /// <summary>
    /// This class adds wrapper functions to make a Min and Max value for a
    /// VScroll (HScroll is ok too). This makes it a better drop in for the
    /// Windows.Forms scrollbar.
    /// </summary>
    public class MinMaxScroll
    {
        private VScroll scroll;
        private int max;
        private int min;

        public MinMaxScroll(VScroll scroll)
        {
            this.scroll = scroll;
        }

        public int Minimum
        {
            get
            {
                return min;
            }
            set
            {
                min = value;
                uint range = (uint)(max - min);
                if (scroll.ScrollPosition < min)
                {
                    scroll.ScrollPosition = 0;
                }
                scroll.ScrollRange = range;
            }
        }

        public int Maximum
        {
            get
            {
                return max;
            }
            set
            {
                max = value;
                uint range = (uint)(max - min);
                if (scroll.ScrollPosition > range)
                {
                    scroll.ScrollPosition = range;
                }
                scroll.ScrollRange = range;
            }
        }

        public int Value
        {
            get
            {
                return (int)scroll.ScrollPosition + min;
            }
            set
            {
                scroll.ScrollPosition = (uint)(value - min);
            }
        }

        public event MyGUIEvent ScrollChangePosition
        {
            add
            {
                scroll.ScrollChangePosition += value;
            }
            remove
            {
                scroll.ScrollChangePosition -= value;
            }
        }

        /// <summary>
        /// Get the real scroll used by this class.
        /// </summary>
        public VScroll Scroll
        {
            get
            {
                return scroll;
            }
        }
    }
}
