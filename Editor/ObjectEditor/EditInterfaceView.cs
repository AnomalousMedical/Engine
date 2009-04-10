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
    /// <param name="editInterface">The EditInterfaceViewEvent.</param>
    public delegate void EditInterfaceSelectionChanged(EditInterfaceViewEvent evt);

    /// <summary>
    /// This delegate is called when the selected EditInterface is about to
    /// change.
    /// </summary>
    /// <param name="editInterface">The EditInterfaceViewEvent.</param>
    public delegate void EditInterfaceSelectionChanging(EditInterfaceViewEvent evt);

    public partial class EditInterfaceView : UserControl, EditUICallback
    {
        #region Fields

        private Dictionary<ToolStripItem, CreateEditInterfaceCommand> currentMenuCommands = new Dictionary<ToolStripItem, CreateEditInterfaceCommand>();
        private DestroyEditInterfaceCommand currentDestroyCommand = null;
        private ContextMenuStrip menu = new ContextMenuStrip();
        private EditInterface currentMenuInterface;

        #endregion Fields

        #region Events

        /// <summary>
        /// Called when the selected EditInterface has changed. Cannot be canceled.
        /// </summary>
        public event EditInterfaceSelectionChanged OnEditInterfaceSelectionChanged;

        /// <summary>
        /// Called when the selected EditInterface is about to change. Can be canceled.
        /// </summary>
        public event EditInterfaceSelectionChanging OnEditInterfaceSelectionChanging;

        #endregion Events

        #region Constructors

        public EditInterfaceView()
        {
            InitializeComponent();
            objectsTree.NodeMouseClick += new TreeNodeMouseClickEventHandler(objectsTree_NodeMouseClick);
            objectsTree.AfterSelect += new TreeViewEventHandler(objectsTree_AfterSelect);
            objectsTree.BeforeSelect += new TreeViewCancelEventHandler(objectsTree_BeforeSelect);
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

        /// <summary>
        /// Validate all EditInterfaces in this view. If one has an error it
        /// will be highlighted and an errorMessage will be returned.
        /// </summary>
        /// <param name="errorMessage">An error message on an error.</param>
        /// <returns>True if all interfaces are valid.</returns>
        public bool validateAllInterfaces(out String errorMessage)
        {
            //Start with the selected node. This way if it has an error the change will be less jarring.
            EditInterface currentInterface = ((EditInterfaceTreeNode)objectsTree.SelectedNode).EditInterface;
            if (currentInterface.validate(out errorMessage))
            {
                return scanForErrors(out errorMessage, objectsTree.Nodes);
            }
            //There is an error with the selected node.
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Recursive helper function to scan the whole tree.
        /// </summary>
        /// <param name="errorMessage">An error message on an error.</param>
        /// <returns>True if all interfaces are valid.</returns>
        private bool scanForErrors(out String errorMessage, TreeNodeCollection parent)
        {
            foreach (EditInterfaceTreeNode node in parent)
            {
                if (!node.EditInterface.validate(out errorMessage))
                {
                    objectsTree.SelectedNode = node;
                    return false;
                }
                if (!scanForErrors(out errorMessage, node.Nodes))
                {
                    return false;
                }
            }
            errorMessage = null;
            return true;
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
                if (currentMenuInterface.hasDestroyObjectCommand())
                {
                    DestroyEditInterfaceCommand command = currentMenuInterface.getDestroyObjectCommand();
                    ToolStripItem entry = new ToolStripMenuItem(command.PrettyName);
                    currentDestroyCommand = command;
                    menu.Items.Add(entry);
                }
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
            if (currentMenuCommands.ContainsKey(e.ClickedItem))
            {
                CreateEditInterfaceCommand command = currentMenuCommands[e.ClickedItem];
                EditInterface newInterface = command.execute(this);
                if (newInterface != null)
                {
                    objectsTree.SelectedNode.Nodes.Add(new EditInterfaceTreeNode(newInterface));
                }
            }
            else
            {
                EditInterfaceTreeNode node = (EditInterfaceTreeNode)objectsTree.SelectedNode;
                currentDestroyCommand.execute(node.EditInterface, this);
                objectsTree.Nodes.Remove(node);
            }
        }

        void objectsTree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (OnEditInterfaceSelectionChanging != null)
            {
                EditInterfaceViewEvent evt = new EditInterfaceViewEvent((e.Node as EditInterfaceTreeNode).EditInterface);
                OnEditInterfaceSelectionChanging.Invoke(evt);
                e.Cancel = evt.Cancel;
            }
        }

        void objectsTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (OnEditInterfaceSelectionChanged != null)
            {
                EditInterfaceViewEvent evt = new EditInterfaceViewEvent((e.Node as EditInterfaceTreeNode).EditInterface);
                OnEditInterfaceSelectionChanged.Invoke(evt);
            }
        }

        #endregion Helper Functions
    }
}
