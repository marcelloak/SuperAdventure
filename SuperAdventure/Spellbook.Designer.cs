namespace SuperAdventure
{
    partial class Spellbook
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
            dgvSpells = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvSpells).BeginInit();
            SuspendLayout();
            // 
            // dgvSpells
            // 
            dgvSpells.AllowUserToAddRows = false;
            dgvSpells.AllowUserToDeleteRows = false;
            dgvSpells.AllowUserToResizeRows = false;
            dgvSpells.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSpells.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvSpells.Location = new Point(7, 14);
            dgvSpells.MultiSelect = false;
            dgvSpells.Name = "dgvSpells";
            dgvSpells.ReadOnly = true;
            dgvSpells.RowHeadersVisible = false;
            dgvSpells.Size = new Size(203, 403);
            dgvSpells.TabIndex = 0;
            dgvSpells.CellDoubleClick += dgvSpells_CellDoubleClick;
            // 
            // Spellbook
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(214, 431);
            Controls.Add(dgvSpells);
            KeyPreview = true;
            Name = "Spellbook";
            Text = "Spellbook";
            KeyDown += Spellbook_KeyDown;
            ((System.ComponentModel.ISupportInitialize)dgvSpells).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvSpells;
    }
}