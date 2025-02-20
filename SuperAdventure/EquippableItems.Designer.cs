namespace SuperAdventure
{
    partial class EquippableItems
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dgvEquipment = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvEquipment).BeginInit();
            SuspendLayout();
            // 
            // dgvEquipment
            // 
            dgvEquipment.AllowUserToAddRows = false;
            dgvEquipment.AllowUserToDeleteRows = false;
            dgvEquipment.AllowUserToResizeRows = false;
            dgvEquipment.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEquipment.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvEquipment.Location = new Point(12, 15);
            dgvEquipment.MultiSelect = false;
            dgvEquipment.Name = "dgvEquipment";
            dgvEquipment.ReadOnly = true;
            dgvEquipment.RowHeadersVisible = false;
            dgvEquipment.Size = new Size(350, 423);
            dgvEquipment.TabIndex = 0;
            dgvEquipment.CellDoubleClick += dgvEquipment_CellDoubleClick;
            // 
            // EquippableItems
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(374, 450);
            Controls.Add(dgvEquipment);
            KeyPreview = true;
            Name = "EquippableItems";
            Text = "EquippableItems";
            KeyDown += EquippableItems_KeyDown;
            ((System.ComponentModel.ISupportInitialize)dgvEquipment).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvEquipment;
    }
}