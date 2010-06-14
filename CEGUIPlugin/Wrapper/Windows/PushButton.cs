using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEGUIPlugin
{
    public class PushButton : Window
    {
        internal PushButton(IntPtr pushButton)
            :base(pushButton)
        {

        }

        public override void Dispose()
        {
            if(clickedTranslator != null) clickedTranslator.Dispose();
            base.Dispose();
        }

        private CEGUIEventTranslator clickedTranslator;

        public event CEGUIEvent Clicked
        {
            add
            {
                if(clickedTranslator == null)
                {
                    clickedTranslator = new WindowEventTranslator("Clicked", this);
                }
                clickedTranslator.BoundEvent += value;
            }
            remove
            {
                clickedTranslator.BoundEvent -= value;
            }
        }        
    }
}
