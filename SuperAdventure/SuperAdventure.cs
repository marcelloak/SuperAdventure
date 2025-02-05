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

namespace SuperAdventure
{
    public partial class SuperAdventure : Form
    {
        private Player _player;
        private Monster _currentMonster;
        private const string PLAYER_DATA_FILE_NAME = "PlayerData.xml";

        public SuperAdventure()
        {
            InitializeComponent();
            _player = Player.CreateDefaultPlayer();
            bindUI();
            MoveTo(_player.CurrentLocation);
        }

        private void btnNorth_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToNorth);
        }

        private void btnEast_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToEast);
        }

        private void btnSouth_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToSouth);
        }

        private void btnWest_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToWest);
        }

        private void btnUseWeapon_Click(object sender, EventArgs e)
        {
            Weapon currentWeapon = (Weapon)cboWeapons.SelectedItem;
            int damageToMonster = RandomNumberGenerator.NumberBetween(currentWeapon.MinimumDamage, currentWeapon.MaximumDamage);
            _currentMonster.CurrentHitPoints -= damageToMonster;
            rtbMessages.AppendText("You hit the " + _currentMonster.Name + " for " + damageToMonster.ToString() + " points." + Environment.NewLine);

            if (_currentMonster.CurrentHitPoints <= 0)
            {
                rtbMessages.AppendText(Environment.NewLine);
                rtbMessages.AppendText("You defeated the " + _currentMonster.Name + Environment.NewLine);
                _player.AddExperiencePoints(_currentMonster.RewardExperiencePoints);
                rtbMessages.AppendText("You receive " + _currentMonster.RewardExperiencePoints.ToString() + " experience points" + Environment.NewLine);
                _player.Gold += _currentMonster.RewardGold;
                rtbMessages.AppendText("You receive " + _currentMonster.RewardGold.ToString() + " gold" + Environment.NewLine);

                List<InventoryItem> lootedItems = new List<InventoryItem>();
                _currentMonster.LootTable.Where(lootItem => RandomNumberGenerator.NumberBetween(1, 100) <= lootItem.DropPercentage).ToList().ForEach(lootItem => lootedItems.Add(new InventoryItem(lootItem.Details, 1)));
                if (lootedItems.Count == 0) _currentMonster.LootTable.Where(lootItem => lootItem.IsDefaultItem).ToList().ForEach(lootItem => lootedItems.Add(new InventoryItem(lootItem.Details, 1)));

                foreach (InventoryItem inventoryItem in lootedItems)
                {
                    _player.AddItemToInventory(inventoryItem.Details, 1);
                    rtbMessages.AppendText("You loot " + inventoryItem.Quantity.ToString() + " " + inventoryItem.Description + Environment.NewLine);
                }

                RefreshUI(true);
                rtbMessages.AppendText(Environment.NewLine);
                MoveTo(_player.CurrentLocation);
            }
            else MonsterTakesTurn();
        }

        private void btnUsePotion_Click(object sender, EventArgs e)
        {
            HealingPotion potion = (HealingPotion)cboPotions.SelectedItem;
            _player.CurrentHitPoints += potion.AmountToHeal;
            if (_player.CurrentHitPoints > _player.MaximumHitPoints) _player.CurrentHitPoints = _player.MaximumHitPoints;
            _player.RemoveItemFromInventory(potion, 1);
            rtbMessages.AppendText("You drink a " + potion.Name + " and heal for " + potion.AmountToHeal + " points." + Environment.NewLine);
            MonsterTakesTurn();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (File.Exists(PLAYER_DATA_FILE_NAME)) _player = Player.CreatePlayerFromXmlString(File.ReadAllText(PLAYER_DATA_FILE_NAME));
            rtbMessages.AppendText("You have loaded a saved game." + Environment.NewLine);
            bindUI();
            MoveTo(_player.CurrentLocation);
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

        private void MoveTo(Location newLocation)
        {
            if (!_player.HasRequiredItemToEnterThisLocation(newLocation))
            {
                rtbMessages.AppendText("You must have a " + newLocation.ItemRequiredToEnter.Name + " to enter this location." + Environment.NewLine);
                return;
            }

            _player.CurrentLocation = newLocation;

            btnNorth.Visible = (newLocation.LocationToNorth != null);
            btnEast.Visible = (newLocation.LocationToEast != null);
            btnSouth.Visible = (newLocation.LocationToSouth != null);
            btnWest.Visible = (newLocation.LocationToWest != null);

            rtbLocation.Text = newLocation.Name + Environment.NewLine;
            rtbLocation.Text += newLocation.Description + Environment.NewLine;

            _player.CurrentHitPoints = _player.MaximumHitPoints;

            if (newLocation.QuestAvailableHere != null)
            {
                if (_player.HasThisQuest(newLocation.QuestAvailableHere))
                {
                    if (!_player.CompletedThisQuest(newLocation.QuestAvailableHere))
                    {
                        if (_player.HasAllQuestCompletionItems(newLocation.QuestAvailableHere))
                        {
                            rtbMessages.AppendText(Environment.NewLine);
                            rtbMessages.AppendText("You complete the '" + newLocation.QuestAvailableHere.Name + "' quest." + Environment.NewLine);

                            _player.RemoveQuestCompletionItems(newLocation.QuestAvailableHere);

                            rtbMessages.AppendText("You receive: " + Environment.NewLine);
                            rtbMessages.AppendText(newLocation.QuestAvailableHere.RewardExperiencePoints.ToString() + " experience points" + Environment.NewLine);
                            rtbMessages.AppendText(newLocation.QuestAvailableHere.RewardGold.ToString() + " gold" + Environment.NewLine);
                            rtbMessages.AppendText(newLocation.QuestAvailableHere.RewardItem.Name + Environment.NewLine);
                            rtbMessages.AppendText(Environment.NewLine);

                            _player.AddExperiencePoints(newLocation.QuestAvailableHere.RewardExperiencePoints);
                            _player.Gold += newLocation.QuestAvailableHere.RewardGold;

                            _player.AddItemToInventory(newLocation.QuestAvailableHere.RewardItem, 1);

                            _player.MarkQuestCompleted(newLocation.QuestAvailableHere);
                            bindUI(); // TODO: Fix so this is not necessary (without it quest list only updates when new quest is added, not when quest is completed)
                        }
                    }
                }
                else
                {
                    rtbMessages.AppendText("You receive the " + newLocation.QuestAvailableHere.Name + " quest." + Environment.NewLine);
                    rtbMessages.AppendText(newLocation.QuestAvailableHere.Description + Environment.NewLine);
                    rtbMessages.AppendText("To complete it, return with:" + Environment.NewLine);
                    newLocation.QuestAvailableHere.QuestCompletionItems.ForEach(questItem => rtbMessages.AppendText(questItem.Quantity.ToString() + " " + questItem.Description + Environment.NewLine));
                    rtbMessages.AppendText(Environment.NewLine);

                    _player.Quests.Add(new PlayerQuest(newLocation.QuestAvailableHere));
                }
            }

            bool monsterLivingHere = (newLocation.MonsterLivingHere != null);

            if (monsterLivingHere)
            {
                rtbMessages.AppendText("You see a " + newLocation.MonsterLivingHere.Name + Environment.NewLine);
                _currentMonster = CreateMonsterInstance(newLocation.MonsterLivingHere);

            }
            else _currentMonster = null;

            RefreshUI(monsterLivingHere);
        }

        private Monster CreateMonsterInstance(Monster monster)
        {
            Monster standardMonster = World.MonsterByID(monster.ID);

            Monster currentMonster = new Monster(standardMonster.ID, standardMonster.Name, standardMonster.MaximumDamage, standardMonster.RewardGold, standardMonster.RewardExperiencePoints, standardMonster.CurrentHitPoints, standardMonster.MaximumHitPoints);

            standardMonster.LootTable.ForEach(lootItem => currentMonster.LootTable.Add(lootItem));

            return currentMonster;
        }

        private void UpdateWeaponListInUI(bool monsterLivingHere)
        {
            if (!monsterLivingHere)
            {
                cboWeapons.Visible = false;
                btnUseWeapon.Visible = false;
                return;
            }

            List<Weapon> weapons = new List<Weapon>();
            _player.Inventory.Where(item => item.Details is Weapon && item.Quantity > 0).ToList().ForEach(item => weapons.Add((Weapon)item.Details));

            if (weapons.Count == 0)
            {
                cboWeapons.Visible = false;
                btnUseWeapon.Visible = false;
            }
            else
            {
                cboWeapons.SelectedIndexChanged -= cboWeapons_SelectedIndexChanged;
                cboWeapons.DataSource = weapons;
                cboWeapons.SelectedIndexChanged += cboWeapons_SelectedIndexChanged;
                cboWeapons.DisplayMember = "Name";
                cboWeapons.ValueMember = "ID";
                cboWeapons.SelectedItem = _player.CurrentWeapon;
                cboWeapons.Visible = true;
                btnUseWeapon.Visible = true;
            }
        }

        private void UpdatePotionListInUI(bool monsterLivingHere)
        {
            if (!monsterLivingHere)
            {
                cboPotions.Visible = false;
                btnUsePotion.Visible = false;
                return;
            }

            List<HealingPotion> healingPotions = new List<HealingPotion>();
            _player.Inventory.Where(item => item.Details is HealingPotion && item.Quantity > 0).ToList().ForEach(item => healingPotions.Add((HealingPotion)item.Details));

            if (healingPotions.Count == 0)
            {
                cboPotions.Visible = false;
                btnUsePotion.Visible = false;
            }
            else
            {
                cboPotions.DataSource = healingPotions;
                cboPotions.DisplayMember = "Name";
                cboPotions.ValueMember = "ID";
                cboPotions.SelectedIndex = 0;
                cboPotions.Visible = true;
                btnUsePotion.Visible = true;
            }
        }

        private void RefreshUI(bool monsterLivingHere)
        {
            UpdateWeaponListInUI(monsterLivingHere);
            UpdatePotionListInUI(monsterLivingHere);
        }

        private void MonsterTakesTurn()
        {
            int damageToPlayer = RandomNumberGenerator.NumberBetween(0, _currentMonster.MaximumDamage);
            rtbMessages.AppendText("The " + _currentMonster.Name + " did " + damageToPlayer.ToString() + " points of damage." + Environment.NewLine);
            _player.CurrentHitPoints -= damageToPlayer;
            RefreshUI(true);
            if (_player.CurrentHitPoints <= 0)
            {
                rtbMessages.AppendText("The " + _currentMonster.Name + " killed you." + Environment.NewLine);
                MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
            }
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
        }
    }
}
