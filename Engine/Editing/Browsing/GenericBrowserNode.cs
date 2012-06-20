using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Editing
{
    public class GenericBrowserNode<T> : BrowserNode
    {
        public GenericBrowserNode(String text, T value, String defaultName = null)
            : base(text, value, defaultName)
        {

        }
    }
}