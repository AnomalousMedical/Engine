using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace MyGUIPlugin
{
    public class TooltipManager
    {
        private static TooltipManager instance;

        public static TooltipManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new TooltipManager();
                }
                return instance;
            }
        }

        private Dictionary<Widget, Tooltip> tooltips = new Dictionary<Widget, Tooltip>();

        protected TooltipManager()
        {

        }

        public void processTooltip(Widget hostWidget, String text, ToolTipEventArgs eventArgs)
        {
            if (eventArgs.Type == ToolTipType.Show)
            {
                showTooltip(hostWidget, text, eventArgs.Point);
            }
            else
            {
                destroyTooltip(hostWidget);
            }
        }

        public void showTooltip(Widget hostWidget, String text, Vector2 location)
        {
            Tooltip tooltip = new Tooltip(text, (int)location.x, (int)location.y + 25);
            tooltip.Visible = true;
            tooltips.Add(hostWidget, tooltip);
        }

        public void destroyTooltip(Widget hostWidget)
        {
            Tooltip tooltip;
            if (tooltips.TryGetValue(hostWidget, out tooltip))
            {
                tooltip.Dispose();
                tooltips.Remove(hostWidget);
            }
        }
    }
}
