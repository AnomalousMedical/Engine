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
    public partial class ObjectEditorPanel : UserControl
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

        public ObjectEditorPanel()
        {
            InitializeComponent();

            propertiesTable.AutoGenerateColumns = false;
            propertiesTable.AllowUserToAddRows = false;
            propertiesTable.AllowUserToDeleteRows = false;
            propertiesTable.AllowDrop = false;
            propertiesTable.EditMode = DataGridViewEditMode.EditOnEnter;
            propertiesTable.CellParsing += new DataGridViewCellParsingEventHandler(propertiesTable_CellParsing);
            propertiesTable.CellValidating += new DataGridViewCellValidatingEventHandler(propertiesTable_CellValidating);
            propertiesTable.CellValueChanged += new DataGridViewCellEventHandler(propertiesTable_CellValueChanged);
        }

        #endregion Constructors

        #region Functions

        public void setEditInterface(EditInterface editor)
        {
            this.objectsTree.Nodes.Clear();
            EditInterfaceTreeNode parentNode = new EditInterfaceTreeNode(editor);
            this.objectsTree.Nodes.Add(parentNode);
            showEditableProperties(parentNode);
        }

        private void showEditableProperties(EditInterfaceTreeNode node)
        {
            allowValidation = false;

            propertiesTable.Rows.Clear();
            foreach (EditableProperty editProp in node.EditInterface.getEditableProperties())
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
                propertiesTable.Rows.Add(newRow);
            }

            allowValidation = true;
        }

        #endregion Functions

        #region Helper Functions

        private void propertiesTable_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == VALUE_COL)
            {
                e.Cancel = !validateEditCell(e.RowIndex, e.FormattedValue.ToString());
            }
        }

        private void propertiesTable_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (e.ColumnIndex == VALUE_COL)
            {
                dataErrorOnClose = !validateEditCell(e.RowIndex, propertiesTable.Rows[e.RowIndex].Cells[VALUE_COL].EditedFormattedValue.ToString());
            }
        }

        void propertiesTable_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == VALUE_COL)
            {
                DataGridViewRow row = propertiesTable.Rows[e.RowIndex];
                EditableProperty var = (EditableProperty)row.Cells[PROP_COL].Value;
                var.setValueStr(propertiesTable.Rows[e.RowIndex].Cells[VALUE_COL].EditedFormattedValue.ToString());
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
                DataGridViewRow row = propertiesTable.Rows[rowIndex];
                EditableProperty var = (EditableProperty)row.Cells[PROP_COL].Value;
                if (!var.canParseString(value))
                {
                    valid = false;
                    propertiesTable.Rows[rowIndex].ErrorText = "Invalid value";
                }
                else
                {
                    propertiesTable.Rows[rowIndex].ErrorText = "";
                }
            }
            return valid;
        }

        #endregion Helper Functions
    }
}
