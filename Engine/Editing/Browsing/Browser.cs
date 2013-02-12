using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Editing
{
    public class Browser
    {
        public enum DisplayHint
        {
            Tree = 0,
            Images = 1,
        }

        protected BrowserNode rootNode;

        public Browser(String rootNodeName, String prompt)
        {
            rootNode = new BrowserNode(rootNodeName, null);
            this.Prompt = prompt;
        }

        /// <summary>
        /// Get the top level nodes that should go into the ObjectBrowser tree.
        /// </summary>
        /// <returns>An enumerable with all the top nodes.</returns>
        public BrowserNode getTopNode()
        {
            return rootNode;
        }

        /// <summary>
        /// Add a node to the tree.  This will automaticly create any folder nodes specified
        /// in the path.  The path values can be separated by the delimiter array.  So you could
        /// make a path such as Foo\Bar and send a "\" as the delimiter which would then put node
        /// into a folder called Bar which in turn is a subfolder of Foo, which would be a top 
        /// level node in the tree.
        /// </summary>
        /// <param name="path">The path to put the node in.  Pass null to place the object just under the root node.</param>
        /// <param name="delimeter">The delimiter to split up the path.  Can be null if path is null.</param>
        /// <param name="node">The node to add to path.</param>
        public void addNode(String path, String[] delimeter, BrowserNode node)
        {
            BrowserNode folder;
            if (path != null)
            {
                String[] expandedPath = path.Split(delimeter, StringSplitOptions.RemoveEmptyEntries);
                if (expandedPath.Length > 0)
                {
                    folder = expandRemainingPath(rootNode, expandedPath, 0);
                }
                else
                {
                    folder = rootNode;
                }
            }
            else
            {
                folder = rootNode;
            }
            folder.addChild(node);
        }

        /// <summary>
        /// Returns a node to select by default, can be null, which means select nothing.
        /// </summary>
        public BrowserNode DefaultSelection { get; set; }

        /// <summary>
        /// A string prompt for this browser.
        /// </summary>
        public String Prompt { get; set; }

        /// <summary>
        /// This is a hint to the ui of the contents of the browser.
        /// </summary>
        public DisplayHint Hint { get; set; }

        /// <summary>
        /// Recursive helper function to build the tree.
        /// </summary>
        /// <param name="parent">The parent node to add any subnodes to.</param>
        /// <param name="path">The path array.</param>
        /// <param name="index">The current index into the path array.</param>
        /// <returns></returns>
        private BrowserNode expandRemainingPath(BrowserNode parent, String[] path, int index)
        {
            if (index == path.Length)
            {
                return parent;
            }
            if (!parent.hasChild(path[index]))
            {
                parent.addChild(new BrowserNode(path[index], null));
            }
            return expandRemainingPath(parent.getChild(path[index]), path, index + 1);
        }
    }
}
