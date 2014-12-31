using System;

namespace Anomalous.GuiFramework
{
    interface DialogEntry
    {
        void deserialize(Engine.ConfigFile file);
        void ensureVisible();
        void openMainGUIDialog();
        void serialize(Engine.ConfigFile file);
        void closeMainGUIDialog();
        void disposeDialog();
    }
}
