using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGUIPlugin
{
    public interface ButtonGridCaptionFactory
    {
        ButtonGridCaption createCaption(String name);

        void destroyCaption(ButtonGridCaption caption);
    }
}
