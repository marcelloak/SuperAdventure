namespace SuperAdventure
{
    partial class SuperAdventure
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            descHitPoints = new Label();
            descGold = new Label();
            descLevel = new Label();
            lblHitPoints = new Label();
            lblGold = new Label();
            lblLevel = new Label();
            descSelectAction = new Label();
            cboWeapons = new ComboBox();
            cboUsableItems = new ComboBox();
            cboSpells = new ComboBox();
            btnUseWeapon = new Button();
            btnUseItem = new Button();
            btnUseSpell = new Button();
            btnNorth = new Button();
            btnEast = new Button();
            btnSouth = new Button();
            btnWest = new Button();
            rtbLocation = new RichTextBox();
            rtbMessages = new RichTextBox();
            dgvInventory = new DataGridView();
            dgvQuests = new DataGridView();
            btnLoad = new Button();
            btnSave = new Button();
            btnTrade = new Button();
            btnMap = new Button();
            btnStats = new Button();
            descMana = new Label();
            lblMana = new Label();
            btnWait = new Button();
            btnSpellbook = new Button();
            lblStatus = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvInventory).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvQuests).BeginInit();
            SuspendLayout();
            // 
            // descHitPoints
            // 
            descHitPoints.AutoSize = true;
            descHitPoints.Location = new Point(18, 20);
            descHitPoints.Name = "descHitPoints";
            descHitPoints.Size = new Size(62, 15);
            descHitPoints.TabIndex = 0;
            descHitPoints.Text = "Hit Points:";
            // 
            // descGold
            // 
            descGold.AutoSize = true;
            descGold.Location = new Point(18, 74);
            descGold.Name = "descGold";
            descGold.Size = new Size(35, 15);
            descGold.TabIndex = 1;
            descGold.Text = "Gold:";
            // 
            // descLevel
            // 
            descLevel.AutoSize = true;
            descLevel.Location = new Point(18, 100);
            descLevel.Name = "descLevel";
            descLevel.Size = new Size(37, 15);
            descLevel.TabIndex = 3;
            descLevel.Text = "Level:";
            // 
            // lblHitPoints
            // 
            lblHitPoints.AutoSize = true;
            lblHitPoints.Location = new Point(110, 19);
            lblHitPoints.Name = "lblHitPoints";
            lblHitPoints.Size = new Size(0, 15);
            lblHitPoints.TabIndex = 4;
            // 
            // lblGold
            // 
            lblGold.AutoSize = true;
            lblGold.Location = new Point(110, 73);
            lblGold.Name = "lblGold";
            lblGold.Size = new Size(0, 15);
            lblGold.TabIndex = 5;
            // 
            // lblLevel
            // 
            lblLevel.AutoSize = true;
            lblLevel.Location = new Point(110, 99);
            lblLevel.Name = "lblLevel";
            lblLevel.Size = new Size(0, 15);
            lblLevel.TabIndex = 7;
            // 
            // descSelectAction
            // 
            descSelectAction.AutoSize = true;
            descSelectAction.Location = new Point(620, 502);
            descSelectAction.Name = "descSelectAction";
            descSelectAction.Size = new Size(74, 15);
            descSelectAction.TabIndex = 8;
            descSelectAction.Text = "Select action";
            // 
            // cboWeapons
            // 
            cboWeapons.FormattingEnabled = true;
            cboWeapons.Location = new Point(369, 555);
            cboWeapons.Name = "cboWeapons";
            cboWeapons.Size = new Size(121, 23);
            cboWeapons.TabIndex = 9;
            // 
            // cboUsableItems
            // 
            cboUsableItems.FormattingEnabled = true;
            cboUsableItems.Location = new Point(369, 589);
            cboUsableItems.Name = "cboUsableItems";
            cboUsableItems.Size = new Size(121, 23);
            cboUsableItems.TabIndex = 10;
            // 
            // cboSpells
            // 
            cboSpells.FormattingEnabled = true;
            cboSpells.Location = new Point(369, 623);
            cboSpells.Name = "cboSpells";
            cboSpells.Size = new Size(121, 23);
            cboSpells.TabIndex = 9;
            // 
            // btnUseWeapon
            // 
            btnUseWeapon.Location = new Point(620, 555);
            btnUseWeapon.Name = "btnUseWeapon";
            btnUseWeapon.Size = new Size(75, 23);
            btnUseWeapon.TabIndex = 11;
            btnUseWeapon.Text = "Use";
            btnUseWeapon.UseVisualStyleBackColor = true;
            btnUseWeapon.Click += btnUseWeapon_Click;
            // 
            // btnUseItem
            // 
            btnUseItem.Location = new Point(620, 589);
            btnUseItem.Name = "btnUseItem";
            btnUseItem.Size = new Size(75, 23);
            btnUseItem.TabIndex = 12;
            btnUseItem.Text = "Use";
            btnUseItem.UseVisualStyleBackColor = true;
            btnUseItem.Click += btnUseItem_Click;
            // 
            // btnUseSpell
            // 
            btnUseSpell.Location = new Point(620, 623);
            btnUseSpell.Name = "btnUseSpell";
            btnUseSpell.Size = new Size(75, 23);
            btnUseSpell.TabIndex = 11;
            btnUseSpell.Text = "Use";
            btnUseSpell.UseVisualStyleBackColor = true;
            btnUseSpell.Click += btnUseSpell_Click;
            // 
            // btnNorth
            // 
            btnNorth.Location = new Point(492, 433);
            btnNorth.Name = "btnNorth";
            btnNorth.Size = new Size(75, 23);
            btnNorth.TabIndex = 13;
            btnNorth.Text = "North";
            btnNorth.UseVisualStyleBackColor = true;
            btnNorth.Click += btnNorth_Click;
            // 
            // btnEast
            // 
            btnEast.Location = new Point(572, 460);
            btnEast.Name = "btnEast";
            btnEast.Size = new Size(75, 23);
            btnEast.TabIndex = 14;
            btnEast.Text = "East";
            btnEast.UseVisualStyleBackColor = true;
            btnEast.Click += btnEast_Click;
            // 
            // btnSouth
            // 
            btnSouth.Location = new Point(492, 487);
            btnSouth.Name = "btnSouth";
            btnSouth.Size = new Size(75, 23);
            btnSouth.TabIndex = 15;
            btnSouth.Text = "South";
            btnSouth.UseVisualStyleBackColor = true;
            btnSouth.Click += btnSouth_Click;
            // 
            // btnWest
            // 
            btnWest.Location = new Point(412, 460);
            btnWest.Name = "btnWest";
            btnWest.Size = new Size(75, 23);
            btnWest.TabIndex = 16;
            btnWest.Text = "West";
            btnWest.UseVisualStyleBackColor = true;
            btnWest.Click += btnWest_Click;
            // 
            // rtbLocation
            // 
            rtbLocation.Location = new Point(347, 19);
            rtbLocation.Name = "rtbLocation";
            rtbLocation.ReadOnly = true;
            rtbLocation.Size = new Size(360, 105);
            rtbLocation.TabIndex = 17;
            rtbLocation.Text = "";
            // 
            // rtbMessages
            // 
            rtbMessages.HideSelection = false;
            rtbMessages.Location = new Point(347, 130);
            rtbMessages.Name = "rtbMessages";
            rtbMessages.ReadOnly = true;
            rtbMessages.Size = new Size(360, 286);
            rtbMessages.TabIndex = 18;
            rtbMessages.Text = "";
            // 
            // dgvInventory
            // 
            dgvInventory.AllowUserToAddRows = false;
            dgvInventory.AllowUserToDeleteRows = false;
            dgvInventory.AllowUserToResizeRows = false;
            dgvInventory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvInventory.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvInventory.Location = new Point(16, 130);
            dgvInventory.MultiSelect = false;
            dgvInventory.Name = "dgvInventory";
            dgvInventory.ReadOnly = true;
            dgvInventory.RowHeadersVisible = false;
            dgvInventory.Size = new Size(312, 309);
            dgvInventory.TabIndex = 19;
            dgvInventory.CellDoubleClick += dgvInventory_CellDoubleClick;
            dgvInventory.DataBindingComplete += dgvInventory_DataBindingComplete;
            // 
            // dgvQuests
            // 
            dgvQuests.AllowUserToAddRows = false;
            dgvQuests.AllowUserToDeleteRows = false;
            dgvQuests.AllowUserToResizeRows = false;
            dgvQuests.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvQuests.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvQuests.Location = new Point(16, 446);
            dgvQuests.MultiSelect = false;
            dgvQuests.Name = "dgvQuests";
            dgvQuests.ReadOnly = true;
            dgvQuests.RowHeadersVisible = false;
            dgvQuests.Size = new Size(312, 189);
            dgvQuests.TabIndex = 20;
            dgvQuests.CellDoubleClick += dgvQuests_CellDoubleClick;
            dgvQuests.DataBindingComplete += dgvQuests_DataBindingComplete;
            // 
            // btnLoad
            // 
            btnLoad.Location = new Point(181, 101);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(75, 23);
            btnLoad.TabIndex = 21;
            btnLoad.Text = "Load";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(181, 74);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 22;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnTrade
            // 
            btnTrade.Location = new Point(492, 619);
            btnTrade.Name = "btnTrade";
            btnTrade.Size = new Size(75, 23);
            btnTrade.TabIndex = 23;
            btnTrade.Text = "Trade";
            btnTrade.UseVisualStyleBackColor = true;
            btnTrade.Click += btnTrade_Click;
            // 
            // btnMap
            // 
            btnMap.Location = new Point(492, 460);
            btnMap.Name = "btnMap";
            btnMap.Size = new Size(75, 23);
            btnMap.TabIndex = 24;
            btnMap.Text = "Map";
            btnMap.UseVisualStyleBackColor = true;
            btnMap.Click += btnMap_Click;
            // 
            // btnStats
            // 
            btnStats.Location = new Point(262, 101);
            btnStats.Name = "btnStats";
            btnStats.Size = new Size(75, 23);
            btnStats.TabIndex = 25;
            btnStats.Text = "Stats";
            btnStats.UseVisualStyleBackColor = true;
            btnStats.Click += btnStats_Click;
            // 
            // descMana
            // 
            descMana.AutoSize = true;
            descMana.Location = new Point(18, 46);
            descMana.Name = "descMana";
            descMana.Size = new Size(40, 15);
            descMana.TabIndex = 26;
            descMana.Text = "Mana:";
            // 
            // lblMana
            // 
            lblMana.AutoSize = true;
            lblMana.Location = new Point(110, 45);
            lblMana.Name = "lblMana";
            lblMana.Size = new Size(0, 15);
            lblMana.TabIndex = 27;
            // 
            // btnWait
            // 
            btnWait.Location = new Point(620, 521);
            btnWait.Name = "btnWait";
            btnWait.Size = new Size(75, 23);
            btnWait.TabIndex = 28;
            btnWait.Text = "Wait";
            btnWait.UseVisualStyleBackColor = true;
            btnWait.Click += btnWait_Click;
            // 
            // btnSpellbook
            // 
            btnSpellbook.Location = new Point(262, 74);
            btnSpellbook.Name = "btnSpellbook";
            btnSpellbook.Size = new Size(75, 23);
            btnSpellbook.TabIndex = 29;
            btnSpellbook.Text = "Spellbook";
            btnSpellbook.UseVisualStyleBackColor = true;
            btnSpellbook.Click += btnSpellbook_Click;
            // 
            // lblStatus
            // 
            lblStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblStatus.Location = new Point(257, 19);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(80, 15);
            lblStatus.TabIndex = 30;
            lblStatus.TextAlign = ContentAlignment.TopRight;
            // 
            // SuperAdventure
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(719, 651);
            Controls.Add(lblStatus);
            Controls.Add(btnSpellbook);
            Controls.Add(btnWait);
            Controls.Add(lblMana);
            Controls.Add(descMana);
            Controls.Add(btnStats);
            Controls.Add(btnMap);
            Controls.Add(btnTrade);
            Controls.Add(btnSave);
            Controls.Add(btnLoad);
            Controls.Add(dgvQuests);
            Controls.Add(dgvInventory);
            Controls.Add(rtbMessages);
            Controls.Add(rtbLocation);
            Controls.Add(btnWest);
            Controls.Add(btnSouth);
            Controls.Add(btnEast);
            Controls.Add(btnNorth);
            Controls.Add(btnUseSpell);
            Controls.Add(btnUseItem);
            Controls.Add(btnUseWeapon);
            Controls.Add(cboSpells);
            Controls.Add(cboUsableItems);
            Controls.Add(cboWeapons);
            Controls.Add(descSelectAction);
            Controls.Add(lblLevel);
            Controls.Add(lblGold);
            Controls.Add(lblHitPoints);
            Controls.Add(descLevel);
            Controls.Add(descGold);
            Controls.Add(descHitPoints);
            KeyPreview = true;
            Name = "SuperAdventure";
            Text = "My Game";
            KeyDown += SuperAdventure_KeyDown;
            ((System.ComponentModel.ISupportInitialize)dgvInventory).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvQuests).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label descHitPoints;
        private Label descGold;
        private Label descLevel;
        private Label lblHitPoints;
        private Label lblGold;
        private Label lblLevel;
        private Label descSelectAction;
        private ComboBox cboWeapons;
        private ComboBox cboUsableItems;
        private ComboBox cboSpells;
        private Button btnUseWeapon;
        private Button btnUseItem;
        private Button btnUseSpell;
        private Button btnNorth;
        private Button btnEast;
        private Button btnSouth;
        private Button btnWest;
        private RichTextBox rtbLocation;
        private RichTextBox rtbMessages;
        private DataGridView dgvInventory;
        private DataGridView dgvQuests;
        private Button btnLoad;
        private Button btnSave;
        private Button btnTrade;
        private Button btnMap;
        private Button btnStats;
        private Label descMana;
        private Label lblMana;
        private Button btnWait;
        private Button btnSpellbook;
        private Label lblStatus;
    }
}
