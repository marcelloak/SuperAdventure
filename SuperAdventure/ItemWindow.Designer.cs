namespace SuperAdventure
{
    partial class ItemWindow
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
            lblName = new Label();
            rtbInfo = new RichTextBox();
            SuspendLayout();
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(30, 25);
            lblName.Name = "lblName";
            lblName.Size = new Size(0, 15);
            lblName.TabIndex = 0;
            lblName.TextAlign = ContentAlignment.TopCenter;
            // 
            // rtbInfo
            // 
            rtbInfo.Location = new Point(30, 80);
            rtbInfo.Name = "rtbInfo";
            rtbInfo.ReadOnly = true;
            rtbInfo.Size = new Size(321, 256);
            rtbInfo.TabIndex = 1;
            rtbInfo.Text = "";
            // 
            // ItemWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(384, 361);
            Controls.Add(rtbInfo);
            Controls.Add(lblName);
            KeyPreview = true;
            Name = "ItemWindow";
            Text = "ItemWindow";
            KeyDown += ItemWindow_KeyDown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblName;
        private RichTextBox rtbInfo;
    }
}