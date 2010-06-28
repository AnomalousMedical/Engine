using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    public class FontManager
    {
        static FontManager instance;

        public static FontManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FontManager();
                }
                return instance;
            }
        }

        private IntPtr fontManager;

        private FontManager()
        {
            fontManager = FontManager_getInstancePtr();
        }

        /// <summary>
        /// Measure the width of the longest line of text in measureString with the font defined by fontName.
        /// </summary>
        /// <param name="fontName">The name of the font to measure with.</param>
        /// <param name="measureString">The string to measure.</param>
        /// <returns>The width of the longest line.</returns>
        public uint measureStringWidth(String fontName, String measureString)
        {
            String[] splitString = measureString.Split('\n');
            String longestLine = "";
            foreach (String split in splitString)
            {
                if (split.Length > longestLine.Length)
                {
                    longestLine = split;
                }
            }
            return FontManager_measureStringWidth(fontManager, fontName, longestLine, new UIntPtr((uint)longestLine.Length));
        }

#region PInvoke

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr FontManager_getInstancePtr();

        [DllImport("MyGUIWrapper")]
        private static extern uint FontManager_measureStringWidth(IntPtr fontManager, String fontName, String measureString, UIntPtr measureStringLength);

#endregion
    }
}
