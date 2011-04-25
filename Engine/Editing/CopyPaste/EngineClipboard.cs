using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Saving;

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
            IEnumerable<Type> supportedTypes = destination.SupportedTypes;
            if (supportedTypes != null)
            {
                foreach (ClipboardEntry copyPaste in clipboard)
                {
                    if (supportedTypes.Contains<Type>(copyPaste.ObjectType))
                    {
                        destination.paste(copyPaste.copy());
                    }
                }
            }
            else
            {
                foreach(ClipboardEntry copyPaste in clipboard)
                {
                    destination.paste(copyPaste.copy());
                }
            }
        }

        private static void cut(ClipboardEntry destination)
        {
            IEnumerable<Type> supportedTypes = destination.SupportedTypes;
            if (supportedTypes != null)
            {
                foreach (ClipboardEntry copyPaste in clipboard)
                {
                    if (supportedTypes.Contains<Type>(copyPaste.ObjectType))
                    {
                        destination.paste(copyPaste.cut());
                    }
                }
            }
            else
            {
                foreach (ClipboardEntry copyPaste in clipboard)
                {
                    destination.paste(copyPaste.cut());
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
