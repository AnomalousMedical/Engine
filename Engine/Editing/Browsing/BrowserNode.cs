using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Editing
{
    public class BrowserNode
    {
        private String text;
        private Object value;
        private Dictionary<String, BrowserNode> children = new Dictionary<String, BrowserNode>();

        public BrowserNode(String text, Object value)
        {
            this.text = text;
            this.value = value;
        }

        public void addChild(BrowserNode child)
        {
            children.Add(child.text, child);
        }

        public void removeChild(BrowserNode child)
        {
            children.Remove(child.text);
        }

        public bool hasChild(String text)
        {
            return children.ContainsKey(text);
        }

        public BrowserNode getChild(String text)
        {
            return children[text];
        }

        public IEnumerable<BrowserNode> getChildIterator()
        {
            return children.Values;
        }

        public String Text
        {
            get
            {
                return text;
            }
        }

        public Object Value
        {
            get
            {
                return value;
            }
        }
    }
}
