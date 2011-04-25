using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Saving;
using Logging;

namespace Engine.Editing
{
    public enum EngineClipboardMode
    {
        Cut,
        Copy,
    }

    public class EngineClipboard
    {
        private static List<ClipboardEntry> clipboard = new List<ClipboardEntry>();
        private static CopySaver copySaver = new CopySaver();

        static EngineClipboard()
        {
            Mode = EngineClipboardMode.Copy;
        }

        public static void add(ClipboardEntry copyPaste)
        {
            clipboard.Add(copyPaste);
        }

        public static void remove(ClipboardEntry copyPaste)
        {
            clipboard.Remove(copyPaste);
        }

        public static void clear()
        {
            clipboard.Clear();
        }

        public static void paste(ClipboardEntry destination)
        {
            switch (Mode)
            {
                case EngineClipboardMode.Copy:
                    copy(destination);
                    break;
                case EngineClipboardMode.Cut:
                    cut(destination);
                    break;
            }
        }

        private static void copy(ClipboardEntry destination)
        {
            foreach(ClipboardEntry copyPaste in clipboard)
            {
                if (copyPaste.SupportsCopy)
                {
                    if (destination.supportsPastingType(copyPaste.ObjectType))
                    {
                        destination.paste(copyPaste.copy());
                    }
                    else
                    {
                        Log.ImportantInfo("Type {0} cannot be pasted into {1}.", copyPaste.ObjectType.Name, destination.ObjectType.Name);
                    }
                }
                else
                {
                    Log.ImportantInfo("Type {0} does not support copy.", copyPaste.ObjectType.Name);
                }
            }
        }

        private static void cut(ClipboardEntry destination)
        {
            foreach (ClipboardEntry copyPaste in clipboard)
            {
                if (copyPaste.SupportsCut)
                {
                    if (destination.supportsPastingType(copyPaste.ObjectType))
                    {
                        destination.paste(copyPaste.cut());
                    }
                    else
                    {
                        Log.ImportantInfo("Type {0} cannot be pasted into {1}.", copyPaste.ObjectType.Name, destination.ObjectType.Name);
                    }
                }
                else
                {
                    Log.ImportantInfo("Type {0} does not support cut.", copyPaste.ObjectType.Name);
                }
            }
            clear();
        }

        public static EngineClipboardMode Mode { get; set; }

        public static object copyObject(Saveable source)
        {
            return copySaver.copyObject(source);
        }
    }
}
