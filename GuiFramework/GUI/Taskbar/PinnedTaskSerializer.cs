using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace Anomalous.GuiFramework
{
    public class PinnedTaskSerializer
    {
        private const String SAVED_TASKBAR_ITEM_BASE = "TaskbarItem";

        private ConfigSection configSection;
        private int currentIndex = 0;

        public PinnedTaskSerializer(ConfigSection configSection)
        {
            this.configSection = configSection;
        }

        public void addPinnedTask(String uniqueName)
        {
            configSection.setValue(SAVED_TASKBAR_ITEM_BASE + currentIndex++, uniqueName);
        }

        public ConfigIterator Tasks
        {
            get
            {
                return new ConfigIterator(configSection, SAVED_TASKBAR_ITEM_BASE);
            }
        }
    }
}
