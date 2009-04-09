using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Engine.Editing;
using Engine;
using CommonControls;

namespace Editor
{
    /// <summary>
    /// This delegate is called when the selected EditInterface changes.
    /// </summary>
    /// <param name="editInterface">The EditInterface that has been selected.</param>
    public delegate void EditInterfaceSelectionChanged(EditInterface editInterface);

    public partial class EditInterfaceView : UserControl, EditUICallback
    {
        #region Fields

        private Dictionary<ToolStripItem, CreateEditInterfaceCommand> currentMenuCommands = new Dictionary<ToolStripItem, CreateEditInterfaceCommand>();
        private ContextMenuStrip menu = new ContextMenuStrip();
        private EditInterface currentMenuInterface;

        #endregion Fields

        #region Events

        public event EditInterfaceSelectionChanged OnEditInterfaceSelectionChanged;

        #endregion Events

        #region Constructors

        public EditInterfaceView()
        {
            InitializeComponent();
            objectsTree.NodeMouseClick += new TreeNodeMouseClickEventHandler(objectsTree_NodeMouseClick);
            objectsTree.AfterSelect += new TreeViewEventHandler(objectsTree_AfterSelect);
            menu.ItemClicked += new ToolStripItemClickedEventHandler(menu_ItemClicked);
        }

        #endregion Constructors

        #region Functions

        public void setEditInterface(EditInterface editor)
        {
            this.objectsTree.Nodes.Clear();
            EditInterfaceTreeNode parentNode = new EditInterfaceTreeNode(editor);
            this.objectsTree.Nodes.Add(parentNode);
        }

        /// <summary>
        /// Call back to the UI to get an input string for a given prompt. This
        /// function will return true if the user entered valid input or false
        /// if they canceled or did not enter valid input. If false is returned
        /// the operation in progress should be stopped and any changes
        /// reverted.
        /// </summary>
        /// <param name="prompt">The propmpt to show the user.</param>
        /// <param name="result">The result of the user input.</param>
        /// <returns>True if the user entered input, false if they canceled it.</returns>
        public bool getInputString(string prompt, out string result)
        {
            InputResult inRes = InputBox.GetInput("Input value", prompt, objectsTree.FindForm());
            result = inRes.text;
            return inRes.ok;
        }

        /// <summary>
        /// Call back to the UI to get an input string for a given prompt. This
        /// function will return true if the user entered valid input or false
        /// if they canceled or did not enter valid input. If false is returned
        /// the operation in progress should be stopped and any changes
        /// reverted.
        /// </summary>
        /// <param name="prompt">The propmpt to show the user.</param>
        /// <param name="preloadValue">A value to preload the input with.</param>
        /// <param name="result">The result of the user input.</param>
        /// <returns>True if the user entered input, false if they canceled it.</returns>
        public bool getInputString(string prompt, string preloadValue, out string result)
        {
            InputResult inRes = InputBox.GetInput("Input value", prompt, objectsTree.FindForm(), preloadValue);
            result = inRes.text;
            return inRes.ok;
        }

        #endregion Functions

        #region Helper Functions

        void objectsTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                currentMenuCommands.Clear();
                menu.Items.Clear();
                EditInterfaceTreeNode node = e.Node as EditInterfaceTreeNode;
                node.TreeView.SelectedNode = node;
                currentMenuInterface = node.EditInterface;
                if (currentMenuInterface.hasCreateSubObjectCommands())
                {
                    foreach (CreateEditInterfaceCommand command in currentMenuInterface.getCreateSubObjectCommands())
                    {
                        ToolStripItem entry = new ToolStripMenuItem(command.PrettyName);
                        currentMenuCommands.Add(entry, command);
                        menu.Items.Add(entry);
                    }
                }
                if (menu.Items.Count > 0)
                {
                    menu.Show(node.TreeView.PointToScreen(e.Location));
                }
            }
        }

        void menu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            CreateEditInterfaceCommand command = currentMenuCommands[e.ClickedItem];
            EditInterface newInterface = command.execute(currentMenuInterface.getCommandTargetObject(), this);
            if (newInterface != null)
            {
                objectsTree.SelectedNode.Nodes.Add(new EditInterfaceTreeNode(newInterface));
            }
        }

        void objectsTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (OnEditInterfaceSelectionChanged != null)
            {
                OnEditInterfaceSelectionChanged.Invoke((e.Node as EditInterfaceTreeNode).EditInterface);
            }
        }

        #endregion Helper Functions
    }
}
