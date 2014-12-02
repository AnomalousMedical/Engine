using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGUIPlugin
{
    public interface ButtonGridCaption : IDisposable
    {
        void align(int x, int y, int width);

        int Height { get; }

        bool Visible { get; set; }
    }
}
