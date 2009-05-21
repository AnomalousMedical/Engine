using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Engine.Editing;
using Engine.Attributes;

namespace Editor
{
    public partial class PropertiesTable : UserControl, EditUICallback
    {
        #region Fields

        private bool allowValidation = true;
        private bool dataErrorOnClose = false;
        private bool changesMade = false;
        private EditablePropertyInfo currentPropInfo;
        private EditInterface currentEditInterface;
        private DataGridViewColumn editColumn = new DataGridViewTextBoxColumn();
        private int editColumnIndex = -1;
        private PropertyAdded propertyAddedCallback;
        private PropertyRemoved propertyRemovedCallback;
        private BrowserWindow browserWindow = new BrowserWindow();

        #endregion Fields

        #region Constructors

        public PropertiesTable()
        {
            InitializeComponent();
            propGridView.AutoGenerateColumns = false;
            propGridView.AllowUserToAddRows = false;
            propGridView.AllowUserToDeleteRows = false;
            propGridView.AllowDrop = false;
            propGridView.AllowUserToOrderColumns = false;
            propGridView.EditMode = DataGridViewEditMode.EditOnKeystroke;
            propGridView.CellParsing += new DataGridViewCellParsingEventHandler(propGridView_CellParsing);
            propGridView.CellValidating += new DataGridViewCellValidatingEventHandler(propGridView_CellValidating);
            propGridView.CellValueChanged += new DataGridViewCellEventHandler(propGridView_CellValueChanged);

            editColumn.Visible = false;
            editColumn.ReadOnly = true;
            editColumn.HeaderText = "Edit";

            propertyAddedCallback = new PropertyAdded(editInterface_OnPropertyAdded);
            propertyRemovedCallback = new PropertyRemoved(editInterface_OnPropertyRemoved);

            this.Disposed += new EventHandler(PropertiesTable_Disposed);
        }

        void PropertiesTable_Disposed(object sender, EventArgs e)
        {
            browserWindow.Dispose();
        }

        #endregion Constructors

        /// <summary>
        /// Show the properties for the given EditInterface.
        /// </summary>
        /// <param name="editInterface"></param>
        public void showEditableProperties(EditInterface editInterface)
        {
            allowValidation = false;

            if (currentEditInterface != null)
            {
                currentEditInterface.OnPropertyAdded -= propertyAddedCallback;
                currentEditInterface.OnPropertyRemoved -= propertyRemovedCallback;
            }

            this.currentEditInterface = editInterface;

            editInterface.OnPropertyAdded += propertyAddedCallback;
            editInterface.OnPropertyRemoved += propertyRemovedCallback;

            propGridView.DataSource = null;
            propGridView.Rows.Clear();
            propGridView.Columns.Clear();
            addRemovePanel.Visible = editInterface.canAddRemoveProperties();
            currentPropInfo = editInterface.getPropertyInfo();
            if (currentPropInfo != null)
            {
                foreach (EditablePropertyColumn column in currentPropInfo.getColumns())
                {
                    DataGridViewColumn dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = column.Header;
                    dgvColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvColumn.ReadOnly = column.ReadOnly;
                    propGridView.Columns.Add(dgvColumn);
                }
                editColumnIndex = propGridView.Columns.Add(editColumn);
            }
            if (editInterface.hasEditableProperties())
            {
                foreach (EditableProperty editProp in editInterface.getEditableProperties())
                {
                    addProperty(editProp);
                }
            }

            allowValidation = true;
        }

        #region Helper Functions

        /// <summary>
        /// Callback for cell validating.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void propGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (!currentPropInfo.getColumn(e.ColumnIndex).ReadOnly)
            {
                e.Cancel = !validateEditCell(e.RowIndex, e.ColumnIndex, e.FormattedValue.ToString());
            }
        }

        /// <summary>
        /// Callback for cell parsing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void propGridView_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (!currentPropInfo.getColumn(e.ColumnIndex).ReadOnly)
            {
                dataErrorOnClose = !validateEditCell(e.RowIndex, e.ColumnIndex, propGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString());
            }
        }

        /// <summary>
        /// Callback for cell value changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void propGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = propGridView.Rows[e.RowIndex];
            EditableProperty var = (EditableProperty)row.Cells[editColumnIndex].Value;
            if (var != null)
            {
                var.setValueStr(e.ColumnIndex, propGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString());
                changesMade = true;
            }
        }

        /// <summary>
        /// Validate the edit cell in a row.
        /// </summary>
        /// <param name="rowIndex">The row index to validate.</param>
        /// <param name="value">The value to validate.</param>
        /// <returns>True if the cell was valid false if not.</returns>
        bool validateEditCell(int rowIndex, int colIndex, String value)
        {
            bool valid = true;
            if (allowValidation)
            {
                DataGridViewRow row = propGridView.Rows[rowIndex];
                EditableProperty var = (EditableProperty)row.Cells[editColumnIndex].Value;
                if (var != null)
                {
                    String errorText;
                    if (!var.canParseString(colIndex, value, out errorText))
                    {
                        valid = false;
                        propGridView.Rows[rowIndex].ErrorText = errorText;
                    }
                    else
                    {
                        propGridView.Rows[rowIndex].ErrorText = "";
                    }
                }
            }
            return valid;
        }

        /// <summary>
        /// Add an EditableProperty to this table.
        /// </summary>
        /// <param name="property">The property to add.</param>
        private void addProperty(EditableProperty property)
        {
            DataGridViewRow newRow = new DataGridViewRow();

            for (int i = 0; i < currentPropInfo.getNumColumns(); i++)
            {
                DataGridViewCell newCell = createCell(property.getPropertyType(i));
                newCell.Value = property.getValue(i);
                newRow.Cells.Add(newCell);
            }
            DataGridViewTextBoxCell editCell = new DataGridViewTextBoxCell();
            editCell.Value = property;
            newRow.Cells.Add(editCell);
            propGridView.Rows.Add(newRow);
        }

        private DataGridViewCell createCell(Type propType)
        {
            if (propType.IsEnum)
            {
                if (propType.GetCustomAttributes(typeof(SingleEnumAttribute), true).Length > 0)
                {
                    SingleEnumEditorCell editorCell = new SingleEnumEditorCell();
                    editorCell.populateCombo(propType);
                    return editorCell;
                }
                else if (propType.GetCustomAttributes(typeof(MultiEnumAttribute), true).Length > 0)
                {
                    MultiEnumEditorCell editorCell = new MultiEnumEditorCell();
                    editorCell.EnumType = propType;
                    return editorCell;
                }
                else
                {
                    DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
                    return cell;
                }
            }
            if (propType == typeof(bool))
            {
                return new DataGridViewCheckBoxCell();
            }
            return new DataGridViewTextBoxCell();
        }

        private void removeProperty(EditableProperty property)
        {
            foreach (DataGridViewRow row in propGridView.Rows)
            {
                if (row.Cells[editColumnIndex].Value == property)
                {
                    propGridView.Rows.Remove(row);
                    break;
                }
            }
        }

        /// <summary>
        /// Remove a row from the table and fire the destroy event for the
        /// EditableProperty on that row.
        /// </summary>
        /// <param name="rowIndex">The rowIndex to remove.</param>
        /// <returns>The EditableProperty that was on the given row.</returns>
        private void removeRow(int rowIndex)
        {
            DataGridViewRow row = propGridView.Rows[rowIndex];
            EditableProperty var = (EditableProperty)row.Cells[editColumnIndex].Value;
            currentEditInterface.getRemovePropertyCallback().Invoke(this, var);
        }

        /// <summary>
        /// Callback for when the add button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addButton_Click(object sender, EventArgs e)
        {
            currentEditInterface.getAddPropertyCallback().Invoke(this);
        }

        /// <summary>
        /// Callback for when the remove button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeButton_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in propGridView.SelectedRows)
            {
                removeRow(row.Index);
            }
        }

        /// <summary>
        /// Validate the settings that are currently being displayed on the table.
        /// </summary>
        /// <param name="errorMessage">An error message to fill out.</param>
        /// <returns>True if the settings are valid. False if they are not.</returns>
        public bool validateCurrentSettings(out String errorMessage)
        {
            return currentEditInterface.validate(out errorMessage);
        }

        #endregion Helper Functions

        #region EditUICallback Members

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
            InputResult inRes = InputBox.GetInput("Input value", prompt, propGridView.FindForm());
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
            InputResult inRes = InputBox.GetInput("Input value", prompt, propGridView.FindForm(), preloadValue);
            result = inRes.text;
            return inRes.ok;
        }

        /// <summary>
        /// Get the EditInterface that is currently selected on the UI.
        /// </summary>
        /// <returns>The EditInterface that is currently selected.</returns>
        public EditInterface getSelectedEditInterface()
        {
            return currentEditInterface;
        }

        void editInterface_OnPropertyRemoved(EditableProperty property)
        {
            removeProperty(property);
        }

        void editInterface_OnPropertyAdded(EditableProperty property)
        {
            addProperty(property);
        }

        public bool showBrowser(Browser browser, out object result)
        {
            DialogResult accept = browserWindow.ShowDialog(this.FindForm());
            result = browserWindow.SelectedValue;
            return accept == DialogResult.OK;
        }

        #endregion
    }
}
