using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine;
using static System.Windows.Forms.Design.AxImporter;

namespace SuperAdventure
{
    public partial class SuperAdventure : Form
    {
        private Player _player;
        Dictionary<System.Windows.Forms.Keys, Button> _keyBindings;
        private const string PLAYER_DATA_FILE_NAME = "PlayerData.xml";

        public SuperAdventure()
        {
            InitializeComponent();
            _player = Player.CreateDefaultPlayer();
            _keyBindings = new Dictionary<System.Windows.Forms.Keys, Button>();
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
            _player.UseWeapon((Weapon)cboWeapons.SelectedItem);
        }

        private void btnUsePotion_Click(object sender, EventArgs e)
        {
            _player.UsePotion((HealingPotion)cboPotions.SelectedItem);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (File.Exists(PLAYER_DATA_FILE_NAME)) _player = Player.CreatePlayerFromXmlString(File.ReadAllText(PLAYER_DATA_FILE_NAME));
            rtbMessages.AppendText("You have loaded a saved game." + Environment.NewLine);
            bindUI();
            _player.MoveTo(_player.CurrentLocation);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            File.WriteAllText(PLAYER_DATA_FILE_NAME, _player.ToXmlString());
            rtbMessages.AppendText("You have saved the game." + Environment.NewLine);
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
            else if (propertyChangedEventArgs.PropertyName == "Potions")
            {
                cboPotions.DataSource = _player.Potions;
                if (!_player.Potions.Any())
                {
                    cboPotions.Visible = false;
                    btnUsePotion.Visible = false;
                }
            }
            else if (propertyChangedEventArgs.PropertyName == "CurrentLocation")
            {
                btnNorth.Visible = (_player.CurrentLocation.LocationToNorth != null);
                btnEast.Visible = (_player.CurrentLocation.LocationToEast != null);
                btnSouth.Visible = (_player.CurrentLocation.LocationToSouth != null);
                btnWest.Visible = (_player.CurrentLocation.LocationToWest != null);

                rtbLocation.Text = _player.CurrentLocation.Name + Environment.NewLine;
                rtbLocation.Text += _player.CurrentLocation.Description + Environment.NewLine;

                if (_player.CurrentLocation.MonsterLivingHere == null)
                {
                    cboWeapons.Visible = false;
                    cboPotions.Visible = false;
                    btnUseWeapon.Visible = false;
                    btnUsePotion.Visible = false;

                }
                else
                {
                    cboWeapons.Visible = _player.Weapons.Any();
                    cboPotions.Visible = _player.Potions.Any();
                    btnUseWeapon.Visible = _player.Weapons.Any();
                    btnUsePotion.Visible = _player.Potions.Any();
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
            if (e.KeyCode == Keys.Escape) Application.Exit();
            else
            {
                e.Handled = true;
                _keyBindings[e.KeyCode].PerformClick();
            }

        }

        private void bindKeys()
        {
            _keyBindings.Add(Keys.W, btnNorth);
            _keyBindings.Add(Keys.S, btnSouth);
            _keyBindings.Add(Keys.A, btnWest);
            _keyBindings.Add(Keys.D, btnEast);
            _keyBindings.Add(Keys.Z, btnUseWeapon);
            _keyBindings.Add(Keys.X, btnUsePotion);
            _keyBindings.Add(Keys.F4, btnSave);
            _keyBindings.Add(Keys.F5, btnLoad);
        }

        private void bindUI()
        {
            lblHitPoints.DataBindings.Clear();
            lblGold.DataBindings.Clear();
            lblExperience.DataBindings.Clear();
            lblLevel.DataBindings.Clear();

            lblHitPoints.DataBindings.Add("Text", _player, "HitPoints");
            lblGold.DataBindings.Add("Text", _player, "Gold");
            lblExperience.DataBindings.Add("Text", _player, "ExperiencePoints");
            lblLevel.DataBindings.Add("Text", _player, "Level");

            dgvInventory.RowHeadersVisible = false;
            dgvInventory.AutoGenerateColumns = false;
            dgvInventory.Columns.Clear();
            dgvInventory.DataSource = _player.Inventory;
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

            dgvQuests.RowHeadersVisible = false;
            dgvQuests.AutoGenerateColumns = false;
            dgvQuests.Columns.Clear();
            dgvQuests.DataSource = _player.Quests;
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

            _player.PropertyChanged -= PlayerOnPropertyChanged;
            cboWeapons.DataSource = _player.Weapons;
            cboWeapons.DisplayMember = "Name";
            cboWeapons.ValueMember = "ID";
            cboWeapons.SelectedItem = _player.CurrentWeapon;
            cboWeapons.SelectedIndexChanged += cboWeapons_SelectedIndexChanged;

            cboPotions.DataSource = _player.Potions;
            cboPotions.DisplayMember = "Name";
            cboPotions.ValueMember = "ID";
            _player.PropertyChanged += PlayerOnPropertyChanged;

            _player.OnMessage -= DisplayMessage;
            _player.OnMessage += DisplayMessage;
        }
    }
}
