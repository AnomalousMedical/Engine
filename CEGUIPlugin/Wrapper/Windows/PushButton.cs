using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEGUIPlugin
{
    public class PushButton : Window
    {
        private CEGUIEventTranslator clickedTranslator;

        internal PushButton(IntPtr pushButton)
            :base(pushButton)
        {
            clickedTranslator = new WindowEventTranslator("Clicked", this);
        }

        public override void Dispose()
        {
            clickedTranslator.Dispose();
            base.Dispose();
        }

        public event CEGUIEvent Clicked
        {
            add
            {
                clickedTranslator.BoundEvent += value;
            }
            remove
            {
                clickedTranslator.BoundEvent -= value;
            }
        }        
    }
}
