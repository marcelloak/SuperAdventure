using System.ComponentModel;
using Engine;

namespace SuperAdventure
{
    public partial class SuperAdventure : Form
    {
        private Player _player;
        Dictionary<Keys, Button> _keyBindings;
        private const string PLAYER_DATA_FILE_NAME = "PlayerData.xml";

        public SuperAdventure()
        {
            InitializeComponent();
            _player = Player.CreateDefaultPlayer();
            _keyBindings = new Dictionary<Keys, Button>();
            bindKeys();
            bindUI();
            _player.MoveTo(_player.CurrentLocation);
        }

        private void btnNorth_Click(object sender, EventArgs e)
        {
            _player.MoveTo(_player.CurrentLocation.LocationToNorth);
        }

        private void btnEast_Click(object sender, EventArgs e)
        {
            _player.MoveTo(_player.CurrentLocation.LocationToEast);
        }

        private void btnSouth_Click(object sender, EventArgs e)
        {
            _player.MoveTo(_player.CurrentLocation.LocationToSouth);
        }

        private void btnWest_Click(object sender, EventArgs e)
        {
            _player.MoveTo(_player.CurrentLocation.LocationToWest);
        }

        private void btnUseWeapon_Click(object sender, EventArgs e)
        {
            _player.TakeTurn(_player.Attack, _player, cboWeapons.SelectedItem);
        }

        private void btnUseItem_Click(object sender, EventArgs e)
        {
            if (cboUsableItems.SelectedItem is HealingItem) _player.TakeTurn(_player.UseHealingItem, _player, cboUsableItems.SelectedItem);
            else if (cboUsableItems.SelectedItem is StatusItem) _player.TakeTurn(_player.UseStatusItem, _player, cboUsableItems.SelectedItem);
            else if (cboUsableItems.SelectedItem is Scroll) _player.TakeTurn(_player.UseScroll, _player, cboUsableItems.SelectedItem);
        }

        private void btnUseSpell_Click(object sender, EventArgs e)
        {
            _player.TakeTurn(_player.AttemptToCastSpell, _player, cboSpells.SelectedItem);
        }

        private void btnWait_Click(object sender, EventArgs e)
        {
            _player.TakeTurn(_player.WaitATurn, _player);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (File.Exists(PLAYER_DATA_FILE_NAME))
            {
                _player = Player.CreatePlayerFromXmlString(File.ReadAllText(PLAYER_DATA_FILE_NAME));
                rtbMessages.AppendText("You have loaded a saved game." + Environment.NewLine);
                clearUI();
                bindUI();
                _player.MoveTo(_player.CurrentLocation);
            }
            else rtbMessages.AppendText("Failed to load game." + Environment.NewLine);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            File.WriteAllText(PLAYER_DATA_FILE_NAME, _player.ToXmlString());
            rtbMessages.AppendText("You have saved the game." + Environment.NewLine);
        }

        private void btnTrade_Click(object sender, EventArgs e)
        {
            TradingScreen tradingScreen = new TradingScreen(_player);
            tradingScreen.StartPosition = FormStartPosition.CenterParent;
            tradingScreen.ShowDialog(this);
        }

        private void btnMap_Click(object sender, EventArgs e)
        {
            WorldMap mapScreen = new WorldMap(_player);
            mapScreen.StartPosition = FormStartPosition.CenterParent;
            mapScreen.ShowDialog(this);
        }

        private void btnStats_Click(object sender, EventArgs e)
        {
            PlayerStats statScreen = new PlayerStats(_player);
            statScreen.StartPosition = FormStartPosition.CenterParent;
            statScreen.ShowDialog(this);
        }

        private void btnSpellbook_Click(object sender, EventArgs e)
        {
            Spellbook spellbookScreen = new Spellbook(_player);
            spellbookScreen.StartPosition = FormStartPosition.CenterParent;
            spellbookScreen.ShowDialog(this);
        }

        private void dgvInventory_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = Int32.Parse(dgvInventory.Rows[e.RowIndex].Cells[0].Value.ToString());
            ItemWindow itemWindow = new ItemWindow(World.ItemByID(id));
            itemWindow.StartPosition = FormStartPosition.CenterParent;
            itemWindow.ShowDialog(this);
        }

        private void dgvQuests_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = Int32.Parse(dgvQuests.Rows[e.RowIndex].Cells[0].Value.ToString());
            ItemWindow itemWindow = new ItemWindow(World.QuestByID(id));
            itemWindow.StartPosition = FormStartPosition.CenterParent;
            itemWindow.ShowDialog(this);
        }

        private void cboWeapons_SelectedIndexChanged(object sender, EventArgs e)
        {
            _player.CurrentWeapon = (Weapon)cboWeapons.SelectedItem;
        }

        private void PlayerOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "Weapons")
            {
                cboWeapons.DataSource = _player.Weapons;
                if (!_player.Weapons.Any())
                {
                    cboWeapons.Visible = false;
                    btnUseWeapon.Visible = false;
                }
            }
            else if (propertyChangedEventArgs.PropertyName == "UsableItems")
            {
                cboUsableItems.DataSource = _player.UsableItems;
                if (!_player.UsableItems.Any())
                {
                    cboUsableItems.Visible = false;
                    btnUseItem.Visible = false;
                }
            }
            else if (propertyChangedEventArgs.PropertyName == "UsableSpells")
            {
                cboSpells.DataSource = _player.UsableSpells;
                if (!_player.UsableSpells.Any())
                {
                    cboSpells.Visible = false;
                    btnUseSpell.Visible = false;
                }
            }
            else if (propertyChangedEventArgs.PropertyName == "CurrentLocation")
            {
                btnNorth.Visible = (_player.CurrentLocation.LocationToNorth != null);
                btnEast.Visible = (_player.CurrentLocation.LocationToEast != null);
                btnSouth.Visible = (_player.CurrentLocation.LocationToSouth != null);
                btnWest.Visible = (_player.CurrentLocation.LocationToWest != null);
                btnTrade.Visible = (_player.CurrentLocation.VendorWorkingHere != null);

                rtbLocation.Text = _player.CurrentLocation.Name + Environment.NewLine;
                rtbLocation.Text += _player.CurrentLocation.Description + Environment.NewLine;

                if (_player.CurrentLocation.HasAMonster)
                {
                    cboWeapons.Visible = _player.Weapons.Any();
                    cboUsableItems.Visible = _player.UsableItems.Any();
                    cboSpells.Visible = _player.UsableSpells.Any();
                    btnUseWeapon.Visible = _player.Weapons.Any();
                    btnUseItem.Visible = _player.UsableItems.Any();
                    btnUseSpell.Visible = _player.UsableSpells.Any();
                    btnWait.Visible = true;
                }
                else
                {
                    cboWeapons.Visible = false;
                    cboUsableItems.Visible = false;
                    cboSpells.Visible = false;
                    btnUseWeapon.Visible = false;
                    btnUseItem.Visible = false;
                    btnUseSpell.Visible = false;
                    btnWait.Visible = false;
                }
            }
        }
        private void DisplayMessage(object sender, MessageEventArgs messageEventArgs)
        {
            rtbMessages.AppendText(messageEventArgs.Message + Environment.NewLine);
            if (messageEventArgs.AddExtraNewLine) rtbMessages.AppendText(Environment.NewLine);
        }

        private void SuperAdventure_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            if (e.KeyCode == Keys.Escape) Application.Exit();
            if (_keyBindings.ContainsKey(e.KeyCode)) _keyBindings[e.KeyCode].PerformClick();
        }

        private void bindKeys()
        {
            _keyBindings.Add(Keys.W, btnNorth);
            _keyBindings.Add(Keys.S, btnSouth);
            _keyBindings.Add(Keys.A, btnWest);
            _keyBindings.Add(Keys.D, btnEast);
            _keyBindings.Add(Keys.Z, btnUseWeapon);
            _keyBindings.Add(Keys.X, btnUseItem);
            _keyBindings.Add(Keys.T, btnTrade);
            _keyBindings.Add(Keys.C, btnStats);
            _keyBindings.Add(Keys.F4, btnSave);
            _keyBindings.Add(Keys.F5, btnLoad);
        }

        private void clearUI()
        {
            lblHitPoints.DataBindings.Clear();
            lblMana.DataBindings.Clear();
            lblGold.DataBindings.Clear();
            lblLevel.DataBindings.Clear();

            dgvInventory.DataSource = null;
            dgvQuests.DataSource = null;
            dgvInventory.Columns.Clear();
            dgvQuests.Columns.Clear();

            _player.PropertyChanged -= PlayerOnPropertyChanged;
            _player.OnMessage -= DisplayMessage;
        }

        private void bindUI()
        {
            lblHitPoints.DataBindings.Add("Text", _player, "HitPoints");
            lblMana.DataBindings.Add("Text", _player, "Mana");
            lblGold.DataBindings.Add("Text", _player, "Gold");
            lblLevel.DataBindings.Add("Text", _player, "Level");

            dgvInventory.RowHeadersVisible = false;
            dgvInventory.AutoGenerateColumns = false;
            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ItemID",
                Visible = false
            });
            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                Width = 197,
                DataPropertyName = "Description"
            });
            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Quantity",
                DataPropertyName = "Quantity"
            });
            dgvInventory.DataSource = _player.Inventory;

            dgvQuests.RowHeadersVisible = false;
            dgvQuests.AutoGenerateColumns = false;
            dgvQuests.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "QuestID",
                Visible = false
            });
            dgvQuests.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                Width = 197,
                DataPropertyName = "Name"
            });
            dgvQuests.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Done?",
                DataPropertyName = "IsCompleted"
            });
            dgvQuests.DataSource = _player.Quests;

            cboWeapons.DataSource = _player.Weapons;
            cboWeapons.DisplayMember = "Name";
            cboWeapons.ValueMember = "ID";
            cboWeapons.SelectedItem = _player.CurrentWeapon;
            cboWeapons.SelectedIndexChanged += cboWeapons_SelectedIndexChanged;

            cboUsableItems.DataSource = _player.UsableItems;
            cboUsableItems.DisplayMember = "Name";
            cboUsableItems.ValueMember = "ID";

            cboSpells.DataSource = _player.UsableSpells;
            cboSpells.DisplayMember = "Name";
            cboSpells.ValueMember = "ID";

            _player.PropertyChanged += PlayerOnPropertyChanged;
            _player.OnMessage += DisplayMessage;
        }
    }
}
