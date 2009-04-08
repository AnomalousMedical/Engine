using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Engine.Editing;

namespace Editor
{
    public partial class PropertiesTable : UserControl
    {
        #region Fields

        private bool allowValidation = true;
        private bool dataErrorOnClose = false;
        private bool changesMade = false;
        private EditablePropertyInfo currentPropInfo;
        private DataGridViewColumn editColumn = new DataGridViewColumn();

        #endregion Fields

        #region Constructors

        public PropertiesTable()
        {
            InitializeComponent();
            propGridView.AutoGenerateColumns = false;
            propGridView.AllowUserToAddRows = false;
            propGridView.AllowUserToDeleteRows = false;
            propGridView.AllowDrop = false;
            propGridView.EditMode = DataGridViewEditMode.EditOnEnter;
            propGridView.CellParsing += new DataGridViewCellParsingEventHandler(propGridView_CellParsing);
            propGridView.CellValidating += new DataGridViewCellValidatingEventHandler(propGridView_CellValidating);
            propGridView.CellValueChanged += new DataGridViewCellEventHandler(propGridView_CellValueChanged);

            editColumn.Visible = false;
            editColumn.ReadOnly = true;
            editColumn.HeaderText = "Edit";
        }

        #endregion Constructors

        /// <summary>
        /// Show the properties for the given EditInterface.
        /// </summary>
        /// <param name="editInterface"></param>
        public void showEditableProperties(EditInterface editInterface)
        {
            allowValidation = false;

            propGridView.Rows.Clear();
            propGridView.Columns.Clear();
            if (editInterface.hasEditableProperties())
            {
                currentPropInfo = editInterface.getPropertyInfo();
                foreach (EditablePropertyColumn column in currentPropInfo.getColumns())
                {
                    DataGridViewColumn dgvColumn = new DataGridViewColumn();
                    dgvColumn.HeaderText = column.Header;
                    dgvColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvColumn.ReadOnly = column.ReadOnly;
                    propGridView.Columns.Add(dgvColumn);
                }
                propGridView.Columns.Add(editColumn);
                foreach (EditableProperty editProp in editInterface.getEditableProperties())
                {
                    DataGridViewRow newRow = new DataGridViewRow();

                    for (int i = 0; i < currentPropInfo.getNumColumns(); i++)
                    {
                        DataGridViewTextBoxCell newCell = new DataGridViewTextBoxCell();
                        newCell.Value = editProp.getValue(i);
                        newRow.Cells.Add(newCell);
                    }
                    
                    DataGridViewTextBoxCell editCell = new DataGridViewTextBoxCell();
                    editCell.Value = editProp;
                    newRow.Cells.Add(editCell);
                    propGridView.Rows.Add(newRow);
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
            EditableProperty var = (EditableProperty)row.Cells[currentPropInfo.getNumColumns()].Value;
            var.setValueStr(e.ColumnIndex, propGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString());
            changesMade = true;
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
                EditableProperty var = (EditableProperty)row.Cells[currentPropInfo.getNumColumns()].Value;
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
            return valid;
        }

        #endregion Helper Functions
    }
}
