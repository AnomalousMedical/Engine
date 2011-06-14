using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Engine.Editing;
using Engine;

namespace Editor
{
    /// <summary>
    /// The delegate for EditInterfaceViews.
    /// </summary>
    /// <param name="evt">The EditInterfaceViewEvent.</param>
    public delegate void EditInterfaceEvent(EditInterfaceViewEvent evt);

    public partial class EditInterfaceView : UserControl, EditUICallback
    {
        static OpenFileDialog openDialog = new OpenFileDialog();
        static SaveFileDialog saveDialog = new SaveFileDialog();
        static FolderBrowserDialog folderBrowser = new FolderBrowserDialog();

        #region Fields

        private Dictionary<ToolStripItem, EditInterfaceCommand> currentMenuCommands = new Dictionary<ToolStripItem, EditInterfaceCommand>();
        private ContextMenuStrip menu = new ContextMenuStrip();
        private EditInterface currentMenuInterface;
        private EditInterfaceTreeNode parentNode;
        private TreeDblClickPrevent preventDblClick;
        private BrowserWindow browserWindow = new BrowserWindow();

        #endregion Fields

        #region Events

        /// <summary>
        /// Called when the selected EditInterface has changed. Cannot be canceled.
        /// </summary>
        public event EditInterfaceEvent OnEditInterfaceSelectionChanged;

        /// <summary>
        /// Called when the selected EditInterface is about to change. Can be canceled.
        /// </summary>
        public event EditInterfaceEvent OnEditInterfaceSelectionChanging;

        /// <summary>
        /// Called when the interface has requested to further edit an object.
        /// Can be ignored if not applicable.
        /// </summary>
        public event EditInterfaceEvent OnEditInterfaceSelectionEdit;

        /// <summary>
        /// Called when an EditInterface has been chosen in some way.
        /// </summary>
        public event EditInterfaceEvent OnEditInterfaceChosen;

        /// <summary>
        /// Called when an EditInterface is added.
        /// </summary>
        public event EditInterfaceEvent OnEditInterfaceAdded;

        /// <summary>
        /// Called when an EditInterface is removed.
        /// </summary>
        public event EditInterfaceEvent OnEditInterfaceRemoved;

        #endregion Events

        #region Constructors

        public EditInterfaceView()
        {
            InitializeComponent();
            objectsTree.NodeMouseClick += new TreeNodeMouseClickEventHandler(objectsTree_NodeMouseClick);
            objectsTree.AfterSelect += new TreeViewEventHandler(objectsTree_AfterSelect);
            objectsTree.BeforeSelect += new TreeViewCancelEventHandler(objectsTree_BeforeSelect);
            objectsTree.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(objectsTree_NodeMouseDoubleClick);
            menu.ItemClicked += new ToolStripItemClickedEventHandler(menu_ItemClicked);
            this.Disposed += new EventHandler(EditInterfaceView_Disposed);
            preventDblClick = new TreeDblClickPrevent(objectsTree);
            EditInterfaceIconCollection.setupTreeIcons(objectsTree);
            AutoExpand = false;
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
                parentNode = new EditInterfaceTreeNode(editor, this);
                this.objectsTree.Nodes.Add(parentNode);
                if (AutoExpand)
                {
                    objectsTree.ExpandAll();
                }
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
                if (!Disposing)
                {
                    this.objectsTree.Nodes.Clear();
                }
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
        public void getInputString(string prompt, SendResult<String> sendResult)
        {
            InputResult inRes = InputBox.GetInput("Input value", prompt, objectsTree.FindForm(), delegate(String input, out String newPrompt)
            {
                newPrompt = "";
                return sendResult.Invoke(input, ref newPrompt);
            });
        }

        /// <summary>
        /// Get the EditInterface that is currently selected on the UI.
        /// </summary>
        /// <returns>The EditInterface that is currently selected.</returns>
        public EditInterface getSelectedEditInterface()
        {
            if (objectsTree.SelectedNode != null)
            {
                return ((EditInterfaceTreeNode)objectsTree.SelectedNode).EditInterface;
            }
            else if (objectsTree.Nodes.Count > 0)
            {
                return ((EditInterfaceTreeNode)objectsTree.Nodes[0]).EditInterface;
            }
            else
            {
                return null;
            }
        }

        public void showBrowser(Browser browser, SendResult<Object> sendResult)
        {
            browserWindow.setBrowser(browser);
            DialogResult accept = browserWindow.ShowDialog(this.FindForm());
            if (accept == DialogResult.OK)
            {
                String error = null;
                sendResult(browserWindow.SelectedValue, ref error);
            }
        }

        /// <summary>
        /// Call back to the UI to open a open file browser.
        /// </summary>
        /// <param name="filename">The filename chosen by the file browser.</param>
        /// <returns>True if the user entered input, false if they canceled it.</returns>
        public void showOpenFileDialog(String filterString, SendResult<String> resultCallback)
        {
            openDialog.Filter = filterString;
            if (openDialog.ShowDialog(this) == DialogResult.OK)
            {
                String errorMessage = null;
                resultCallback(openDialog.FileName, ref errorMessage);
            }
        }

        /// <summary>
        /// Call back to the UI to open a save file browser.
        /// </summary>
        /// <param name="filename">The filename chosen by the file browser.</param>
        /// <returns>True if the user entered input, false if they canceled it.</returns>
        public void showSaveFileDialog(String filterString, SendResult<String> resultCallback)
        {
            saveDialog.Filter = filterString;
            if (saveDialog.ShowDialog(this) == DialogResult.OK)
            {
                String errorMessage = null;
                resultCallback(saveDialog.FileName, ref errorMessage);
            }
        }

        /// <summary>
        /// Call back to the UI to open a folder browser dialog.
        /// </summary>
        /// <param name="folderName">The folder chosen by the folder browser.</param>
        /// <returns>True if the user entered input, false if they canceled it.</returns>
        public void showFolderBrowserDialog(SendResult<String> resultCallback)
        {
            if (folderBrowser.ShowDialog(this) == DialogResult.OK)
            {
                String errorMessage = null;
                resultCallback(folderBrowser.SelectedPath, ref errorMessage);
            }
        }

        /// <summary>
        /// This method allows the interface to run a custom query on the
        /// UICallback. This can do anything and is not defined here.
        /// </summary>
        /// <param name="queryKey">The key for the query to run.</param>
        /// <param name="resultCallback">The callback with the results.</param>
        public void runCustomQuery(Object queryKey, SendResult<Object> resultCallback, params Object[] args)
        {

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

        public bool AutoExpand { get; set; }

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

        internal void nodeAdded(EditInterfaceTreeNode editInterfaceTreeNode)
        {
            if (OnEditInterfaceAdded != null)
            {
                OnEditInterfaceAdded.Invoke(new EditInterfaceViewEvent(editInterfaceTreeNode.EditInterface));
            }
        }

        internal void nodeRemoved(EditInterfaceTreeNode editInterfaceTreeNode)
        {
            if (OnEditInterfaceRemoved != null)
            {
                OnEditInterfaceRemoved.Invoke(new EditInterfaceViewEvent(editInterfaceTreeNode.EditInterface));
            }
        }

        #endregion Functions

        #region Helper Functions

        void EditInterfaceView_Disposed(object sender, EventArgs e)
        {
            clearEditInterface();
            browserWindow.Dispose();
        }

        void objectsTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (OnEditInterfaceChosen != null)
            {
                EditInterfaceViewEvent evt = new EditInterfaceViewEvent((e.Node as EditInterfaceTreeNode).EditInterface);
                OnEditInterfaceChosen.Invoke(evt);
            }
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
            if (OnEditInterfaceSelectionChanging != null && e.Node != null)
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

        void objectsTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (OnEditInterfaceSelectionEdit != null)
            {
                EditInterfaceViewEvent evt = new EditInterfaceViewEvent((e.Node as EditInterfaceTreeNode).EditInterface);
                OnEditInterfaceSelectionEdit.Invoke(evt);
            }
        }

        #endregion Helper Functions
    }
}
