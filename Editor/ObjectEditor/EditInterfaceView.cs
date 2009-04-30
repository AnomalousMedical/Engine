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

        private Dictionary<ToolStripItem, EditInterfaceCommand> currentMenuCommands = new Dictionary<ToolStripItem, EditInterfaceCommand>();
        private ContextMenuStrip menu = new ContextMenuStrip();
        private EditInterface currentMenuInterface;
        private EditInterfaceTreeNode parentNode;

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
            this.Disposed += new EventHandler(EditInterfaceView_Disposed);
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Set the current interface being edited by this EditInterfaceView.
        /// You must call clearEditInterface before this function can be called
        /// a second time.
        /// </summary>
        /// <param name="editor"></param>
        public void setEditInterface(EditInterface editor)
        {
            if (parentNode == null)
            {
                parentNode = new EditInterfaceTreeNode(editor);
                this.objectsTree.Nodes.Add(parentNode);
            }
            else
            {
                throw new Exception("Attempted to use an EditInterfaceView with a new EditInterface without first calling clearEditInterface. You must call that function first to clear the old information before setting new information.");
            }
        }

        /// <summary>
        /// Clear the EditInterface from this view. This will disconnect all the
        /// editors from the parent node. This should be called as soon as you
        /// are finished with an EditInterface.
        /// </summary>
        public void clearEditInterface()
        {
            if (parentNode != null)
            {
                parentNode.removeCallbacks();
                this.objectsTree.Nodes.Clear();
                parentNode = null;
            }
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
        /// Get the EditInterface that is currently selected on the UI.
        /// </summary>
        /// <returns>The EditInterface that is currently selected.</returns>
        public EditInterface getSelectedEditInterface()
        {
            return ((EditInterfaceTreeNode)objectsTree.SelectedNode).EditInterface;
        }

        /// <summary>
        /// Validate all EditInterfaces in this view. If one has an error it
        /// will be highlighted and an errorMessage will be returned.
        /// </summary>
        /// <param name="errorMessage">An error message on an error.</param>
        /// <returns>True if all interfaces are valid.</returns>
        public bool validateAllInterfaces(out String errorMessage)
        {
            errorMessage = "";
            bool selectedOk = true;
            //Start with the selected node. This way if it has an error the change will be less jarring.
            if (objectsTree.SelectedNode != null)
            {
                EditInterface currentInterface = ((EditInterfaceTreeNode)objectsTree.SelectedNode).EditInterface;
                selectedOk = currentInterface.validate(out errorMessage);
            }
            if (selectedOk)
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

        void EditInterfaceView_Disposed(object sender, EventArgs e)
        {
            clearEditInterface();
        }

        void objectsTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                currentMenuCommands.Clear();
                menu.Items.Clear();
                EditInterfaceTreeNode node = e.Node as EditInterfaceTreeNode;
                node.TreeView.SelectedNode = node;
                currentMenuInterface = node.EditInterface;
                if (currentMenuInterface.hasCommands())
                {
                    foreach (EditInterfaceCommand command in currentMenuInterface.getCommands())
                    {
                        ToolStripItem entry = new ToolStripMenuItem(command.Name);
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
            EditInterfaceCommand command = currentMenuCommands[e.ClickedItem];
            command.execute(this);
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
