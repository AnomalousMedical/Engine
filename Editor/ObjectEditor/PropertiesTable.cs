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
        #region Static

        private const int NAME_COL = 0;
        private const int VALUE_COL = 1;
        private const int PROP_COL = 2;

        #endregion Static

        #region Fields

        private bool allowValidation = true;
        private bool dataErrorOnClose = false;
        private bool changesMade = false;

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
            if (editInterface.hasEditableProperties())
            {
                foreach (EditableProperty editProp in editInterface.getEditableProperties())
                {
                    DataGridViewRow newRow = new DataGridViewRow();
                    DataGridViewTextBoxCell nameCell = new DataGridViewTextBoxCell();
                    nameCell.Value = editProp.getName();
                    newRow.Cells.Add(nameCell);

                    DataGridViewTextBoxCell valueCell = new DataGridViewTextBoxCell();
                    valueCell.Value = editProp.getValue();
                    newRow.Cells.Add(valueCell);

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
            if (e.ColumnIndex == VALUE_COL)
            {
                e.Cancel = !validateEditCell(e.RowIndex, e.FormattedValue.ToString());
            }
        }

        /// <summary>
        /// Callback for cell parsing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void propGridView_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (e.ColumnIndex == VALUE_COL)
            {
                dataErrorOnClose = !validateEditCell(e.RowIndex, propGridView.Rows[e.RowIndex].Cells[VALUE_COL].EditedFormattedValue.ToString());
            }
        }

        /// <summary>
        /// Callback for cell value changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void propGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == VALUE_COL)
            {
                DataGridViewRow row = propGridView.Rows[e.RowIndex];
                EditableProperty var = (EditableProperty)row.Cells[PROP_COL].Value;
                var.setValueStr(propGridView.Rows[e.RowIndex].Cells[VALUE_COL].EditedFormattedValue.ToString());
                changesMade = true;
            }
        }

        /// <summary>
        /// Validate the edit cell in a row.
        /// </summary>
        /// <param name="rowIndex">The row index to validate.</param>
        /// <param name="value">The value to validate.</param>
        /// <returns>True if the cell was valid false if not.</returns>
        bool validateEditCell(int rowIndex, String value)
        {
            bool valid = true;
            if (allowValidation)
            {
                DataGridViewRow row = propGridView.Rows[rowIndex];
                EditableProperty var = (EditableProperty)row.Cells[PROP_COL].Value;
                String errorText;
                if (!var.canParseString(value, out errorText))
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
