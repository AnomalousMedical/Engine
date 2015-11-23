using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Editing
{
    public class StringListlikeEditInterface : ListlikeEditInterface<String>
    {
        public StringListlikeEditInterface(IList<String> list, String name, Validate validateCallback = null, Func<EditUICallback, Browser> getBrowser = null)
            :base(list, name, validateCallback, getBrowser)
        {

        }

        public StringListlikeEditInterface(LinkedList<String> list, String name, Validate validateCallback = null, Func<EditUICallback, Browser> getBrowser = null)
            : base(list, name, validateCallback, getBrowser)
        {

        }

        protected internal override string createNew()
        {
            return "";
        }

        protected internal override void removed(string value)
        {
            
        }

        protected internal override string parseString(string value)
        {
            return value;
        }

        protected internal override bool canParseString(string value, out string errorMessage)
        {
            errorMessage = null;
            return true;
        }
    }
}
