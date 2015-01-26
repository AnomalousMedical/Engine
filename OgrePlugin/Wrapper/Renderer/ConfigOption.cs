using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace OgrePlugin
{
    public class ConfigOption
    {
        private List<String> possibleValues = new List<string>();

        internal ConfigOption()
        {
            
        }

        public String Name { get; internal set; }

        public String CurrentValue { get; set; }

        public IEnumerable<String> PossibleValues
        {
            get
            {
                return possibleValues;
            }
        }

        public bool Immutable { get; internal set; }

        internal void _setDetails(IntPtr name, IntPtr currentValue, bool immutable)
        {
            this.Name = Marshal.PtrToStringAnsi(name);
            this.CurrentValue = Marshal.PtrToStringAnsi(currentValue);
            Immutable = immutable;
        }

        internal void _addPossibleValue(IntPtr possibleValue)
        {
            possibleValues.Add(Marshal.PtrToStringAnsi(possibleValue));
        }
    }
}
