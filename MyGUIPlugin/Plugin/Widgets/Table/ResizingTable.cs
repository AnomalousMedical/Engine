using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace MyGUIPlugin
{
    public class ResizingTable : Table
    {
        private ScrollView scrollView;

        public ResizingTable(ScrollView scrollView)
            :base(scrollView)
        {
            this.scrollView = scrollView;
        }

        public override void layout()
        {
            int totalWidth = 0;
            if (Columns.Count > 0)
            {
                int size = (int)(1.0f / Columns.Count * scrollView.ViewCoord.width);
                foreach (TableColumn column in Columns)
                {
                    column.Width = size;
                }
                totalWidth = size * Columns.Count;
            }

            base.layout();

            scrollView.CanvasSize = new Size2(totalWidth, Height);
        }
    }
}
