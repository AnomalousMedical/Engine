using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Medical.Controller
{
    public class StoredFloatingWindows
    {
        private List<MDIWindow> floatingWindows = new List<MDIWindow>();

        public StoredFloatingWindows()
        {

        }

        public void addFloatingWindow(MDIWindow floatingWindow)
        {
            floatingWindows.Add(floatingWindow);
        }

        public void restoreWindows()
        {
            foreach (MDIWindow window in floatingWindows)
            {
                window.Visible = true;
            }
        }
    }
}
