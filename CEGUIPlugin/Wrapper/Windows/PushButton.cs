using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEGUIPlugin.Wrapper.Windows
{
    public class PushButton : Window
    {
        

        protected PushButton(IntPtr pushButton)
            :base(pushButton)
        {
            
        }

        public override void Dispose()
        {
            
            base.Dispose();
        }

        
    }
}
