using OgreWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    public class ManualWindow : OgreWindow
    {
        private RenderTarget renderTarget;

        public ManualWindow(RenderTarget renderTarget)
        {
            this.renderTarget = renderTarget;
        }

        public override void Dispose()
        {
            
        }

        public override RenderTarget OgreRenderTarget
        {
            get
            {
                return renderTarget;
            }
        }
    }
}
