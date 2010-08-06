using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void SetConfigInfo(IntPtr name, IntPtr currentValue, bool immutable);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void AddPossibleValue(IntPtr possibleValue);

    public class ConfigOption
    {
        private List<String> possibleValues = new List<string>();

        internal ConfigOption()
        {
            SetInfo = new SetConfigInfo(_setDetails);
            AddValue = new AddPossibleValue(_addPossibleValue);
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

        internal SetConfigInfo SetInfo { get; private set; }

        internal AddPossibleValue AddValue { get; private set; }

        private void _setDetails(IntPtr name, IntPtr currentValue, bool immutable)
        {
            this.Name = Marshal.PtrToStringAnsi(name);
            this.CurrentValue = Marshal.PtrToStringAnsi(currentValue);
            Immutable = immutable;
        }

        private void _addPossibleValue(IntPtr possibleValue)
        {
            possibleValues.Add(Marshal.PtrToStringAnsi(possibleValue));
        }
    }
}
