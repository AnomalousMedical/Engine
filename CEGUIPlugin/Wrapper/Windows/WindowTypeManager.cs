using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CEGUIPlugin
{
    /// <summary>
    /// This class can determine the type of a CEGUI window from an IntPtr. This
    /// is done using a native class that looks almost the same as this one. The
    /// layout of the class hierarchy is defined in the WindowManager.
    /// </summary>
    class WindowTypeManager : IDisposable
    {
        private IntPtr nativeTypeManager;
        private List<Type> types = new List<Type>();

        public WindowTypeManager()
        {
            types.Add(typeof(Window));
            nativeTypeManager = WindowTypeManager_Create(typeof(Window).Name, 0);

            //Buttons
            addLeafType(typeof(PushButton));

            //Tooltips
            addLeafType(typeof(Tooltip));

            //ItemListBase
            pushType(typeof(ItemListBase));

            popType(); //ItemListBase

            //ItemEntry
            pushType(typeof(ItemEntry));

            popType(); //ItemEntry
        }

        public void Dispose()
        {
            WindowTypeManager_Delete(nativeTypeManager);
        }

        /// <summary>
        /// Push a new type into the hierarchy. The resolver will search based
        /// off of the class hierarchy so calling this pushes a new type into
        /// the forefront any new pushes will be added below this object. So to
        /// add a class at one level push it then call popType.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="windowType"></param>
        public void pushType(Type windowType)
        {
            types.Add(windowType);
            WindowTypeManager_pushType(nativeTypeManager, windowType.Name, types.Count - 1);
        }

        /// <summary>
        /// Pop the current type so all new types are added to the parent.
        /// </summary>
        public void popType()
        {
            WindowTypeManager_popType(nativeTypeManager);
        }

        /// <summary>
        /// Add a type that is going to be a leaf. You will not have to call pop
        /// after this it will automatically still be set to the parent type.
        /// </summary>
        /// <param name="windowType"></param>
        public void addLeafType(Type windowType)
        {
            pushType(windowType);
            popType();
        }

        public Type searchType(IntPtr window)
        {
            return types[WindowTypeManager_searchType(nativeTypeManager, window)];
        }

#region PInvoke

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr WindowTypeManager_Create(String baseName, int baseWindowType);

        [DllImport("CEGUIWrapper")]
        private static extern void WindowTypeManager_Delete(IntPtr manager);

        [DllImport("CEGUIWrapper")]
        private static extern void WindowTypeManager_pushType(IntPtr manager, String name, int windowType);

        [DllImport("CEGUIWrapper")]
        private static extern void WindowTypeManager_popType(IntPtr manager);

        [DllImport("CEGUIWrapper")]
        private static extern int WindowTypeManager_searchType(IntPtr manager, IntPtr window);

#endregion
    }
}
