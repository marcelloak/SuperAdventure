namespace SuperAdventure
{
    partial class TradingScreen
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
            descInventory = new Label();
            descVendorInventory = new Label();
            dgvMyItems = new DataGridView();
            dgvVendorItems = new DataGridView();
            btnClose = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvMyItems).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvVendorItems).BeginInit();
            SuspendLayout();
            // 
            // descInventory
            // 
            descInventory.AutoSize = true;
            descInventory.Location = new Point(99, 13);
            descInventory.Name = "descInventory";
            descInventory.Size = new Size(77, 15);
            descInventory.TabIndex = 0;
            descInventory.Text = "My Inventory";
            // 
            // descVendorInventory
            // 
            descVendorInventory.AutoSize = true;
            descVendorInventory.Location = new Point(349, 13);
            descVendorInventory.Name = "descVendorInventory";
            descVendorInventory.Size = new Size(105, 15);
            descVendorInventory.TabIndex = 1;
            descVendorInventory.Text = "Vendor's Inventory";
            // 
            // dgvMyItems
            // 
            dgvMyItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMyItems.Location = new Point(13, 43);
            dgvMyItems.Name = "dgvMyItems";
            dgvMyItems.Size = new Size(240, 216);
            dgvMyItems.TabIndex = 2;
            // 
            // dgvVendorItems
            // 
            dgvVendorItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvVendorItems.Location = new Point(276, 43);
            dgvVendorItems.Name = "dgvVendorItems";
            dgvVendorItems.Size = new Size(240, 216);
            dgvVendorItems.TabIndex = 3;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(441, 274);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(75, 23);
            btnClose.TabIndex = 4;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // TradingScreen
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(528, 310);
            Controls.Add(btnClose);
            Controls.Add(dgvVendorItems);
            Controls.Add(dgvMyItems);
            Controls.Add(descVendorInventory);
            Controls.Add(descInventory);
            KeyPreview = true;
            Name = "TradingScreen";
            Text = "Trade";
            KeyDown += TradingScreen_KeyDown;
            ((System.ComponentModel.ISupportInitialize)dgvMyItems).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvVendorItems).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label descInventory;
        private Label descVendorInventory;
        private DataGridView dgvMyItems;
        private DataGridView dgvVendorItems;
        private Button btnClose;
    }
}