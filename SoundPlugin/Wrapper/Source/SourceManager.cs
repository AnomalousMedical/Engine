using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace SoundPlugin
{
    class SourceManager : IDisposable
    {
        private WrapperCollection<Source> sources = new WrapperCollection<Source>(createWrapper, destroyWrapper);

        public Source getSource(IntPtr source)
        {
            if (source != IntPtr.Zero)
            {
                return sources.getObject(source);
            }
            return null;
        }

        public void Dispose()
        {
            sources.clearObjects();
        }

        public IntPtr deleteWrapper(IntPtr widget)
        {
            return sources.destroyObject(widget);
        }

        private static Source createWrapper(IntPtr source, object[] args)
        {
            return new Source(source);
        }

        private static void destroyWrapper(Source source)
        {
            source.delete();
        }
    }
}
