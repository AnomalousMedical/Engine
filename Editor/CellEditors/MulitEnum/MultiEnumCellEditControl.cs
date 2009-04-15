using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Editor
{
    class MultiEnumCellEditControl : Button, IDataGridViewEditingControl
    {
        private static MultiEnumEditorCellPopup popup = new MultiEnumEditorCellPopup();
        private DataGridView dataGridView;
        private int index;
        private bool valueChanged;
        private object value;

        public Type EnumType { get; set; }

        public MultiEnumCellEditControl()
        {
            this.Click += new EventHandler(MultiEnumCellEditControl_Click);
        }

        void MultiEnumCellEditControl_Click(object sender, EventArgs e)
        {
            popup.populateList(EnumType);
            popup.Value = value;
            popup.ShowDialog(dataGridView.FindForm());
            if (popup.Value != value)
            {
                value = popup.Value;
                notifyDataGridViewOfValueChange();
                this.Text = value.ToString();
            }
        }

        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.ForeColor = dataGridViewCellStyle.ForeColor;
            this.BackColor = dataGridViewCellStyle.BackColor;
            this.Text = value.ToString();
        }

        public DataGridView EditingControlDataGridView
        {
            get
            {
                return dataGridView;
            }
            set
            {
                dataGridView = value;
            }
        }

        public object EditingControlFormattedValue
        {
            get
            {
                return value.ToString();
            }
            set
            {
                this.value = Enum.Parse(EnumType, value.ToString());
            }
        }

        public int EditingControlRowIndex
        {
            get
            {
                return index;
            }
            set
            {
                index = value;
            }
        }

        public bool EditingControlValueChanged
        {
            get
            {
                return valueChanged;
            }
            set
            {
                valueChanged = value;
            }
        }

        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            return !dataGridViewWantsInputKey;
        }

        public Cursor EditingPanelCursor
        {
            get { return base.Cursor; } 
        }

        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {
            
        }

        public bool RepositionEditingControlOnValueChange
        {
            get { return false; }
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            dataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        public object Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }

        private void notifyDataGridViewOfValueChange()
        {
            if (!this.valueChanged)
            {
                this.valueChanged = true;
                this.dataGridView.NotifyCurrentCellDirty(true);
            }
        }
    }
}
