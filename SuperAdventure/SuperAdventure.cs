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
            RefreshUI();
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
                _player.ExperiencePoints += _currentMonster.RewardExperiencePoints;
                rtbMessages.AppendText("You receive " + _currentMonster.RewardExperiencePoints.ToString() + " experience points" + Environment.NewLine);
                _player.Gold += _currentMonster.RewardGold;
                rtbMessages.AppendText("You receive " + _currentMonster.RewardGold.ToString() + " gold" + Environment.NewLine);

                List<InventoryItem> lootedItems = new List<InventoryItem>();
                _currentMonster.LootTable.Where(lootItem => RandomNumberGenerator.NumberBetween(1, 100) <= lootItem.DropPercentage).ToList().ForEach(lootItem => lootedItems.Add(new InventoryItem(lootItem.Details, 1)));
                if (lootedItems.Count == 0) _currentMonster.LootTable.Where(lootItem => lootItem.IsDefaultItem).ToList().ForEach(lootItem => lootedItems.Add(new InventoryItem(lootItem.Details, 1)));

                foreach (InventoryItem inventoryItem in lootedItems)
                {
                    _player.AddItemToInventory(inventoryItem.Details);
                    if (inventoryItem.Quantity == 1) rtbMessages.AppendText("You loot " + inventoryItem.Quantity.ToString() + " " + inventoryItem.Details.Name + Environment.NewLine);
                    else rtbMessages.AppendText("You loot " + inventoryItem.Quantity.ToString() + " " + inventoryItem.Details.NamePlural + Environment.NewLine);
                }

                RefreshUI();
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
            InventoryItem item = _player.Inventory.SingleOrDefault(ii => ii.Details.ID == potion.ID);
            if (item != null) item.Quantity--;
            rtbMessages.AppendText("You drink a " + potion.Name + Environment.NewLine);
            MonsterTakesTurn();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (File.Exists(PLAYER_DATA_FILE_NAME)) _player = Player.CreatePlayerFromXmlString(File.ReadAllText(PLAYER_DATA_FILE_NAME));
            MoveTo(_player.CurrentLocation);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            File.WriteAllText(PLAYER_DATA_FILE_NAME, _player.ToXmlString());
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
            lblHitPoints.Text = _player.CurrentHitPoints.ToString();

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

                            _player.ExperiencePoints += newLocation.QuestAvailableHere.RewardExperiencePoints;
                            _player.Gold += newLocation.QuestAvailableHere.RewardGold;

                            _player.AddItemToInventory(newLocation.QuestAvailableHere.RewardItem);

                            _player.MarkQuestCompleted(newLocation.QuestAvailableHere);
                        }
                    }
                }
                else
                {
                    rtbMessages.AppendText("You receive the " + newLocation.QuestAvailableHere.Name + " quest." + Environment.NewLine);
                    rtbMessages.AppendText(newLocation.QuestAvailableHere.Description + Environment.NewLine);
                    rtbMessages.AppendText("To complete it, return with:" + Environment.NewLine);
                    foreach (QuestCompletionItem questItem in newLocation.QuestAvailableHere.QuestCompletionItems)
                    {
                        if (questItem.Quantity == 1) rtbMessages.AppendText(questItem.Quantity.ToString() + " " + questItem.Details.Name + Environment.NewLine);
                        else rtbMessages.AppendText(questItem.Quantity.ToString() + " " + questItem.Details.NamePlural + Environment.NewLine);
                    }
                    rtbMessages.AppendText(Environment.NewLine);

                    _player.Quests.Add(new PlayerQuest(newLocation.QuestAvailableHere));
                }
            }

            bool monsterLivingHere = (newLocation.MonsterLivingHere != null);

            cboWeapons.Visible = monsterLivingHere;
            cboPotions.Visible = monsterLivingHere;
            btnUseWeapon.Visible = monsterLivingHere;
            btnUsePotion.Visible = monsterLivingHere;

            if (monsterLivingHere)
            {
                rtbMessages.AppendText("You see a " + newLocation.MonsterLivingHere.Name + Environment.NewLine);
                _currentMonster = CreateMonsterInstance(newLocation.MonsterLivingHere);

            }
            else _currentMonster = null;
            RefreshUI();
        }

        private Monster CreateMonsterInstance(Monster monster)
        {
            Monster standardMonster = World.MonsterByID(monster.ID);

            Monster currentMonster = new Monster(standardMonster.ID, standardMonster.Name, standardMonster.MaximumDamage, standardMonster.RewardGold, standardMonster.RewardExperiencePoints, standardMonster.CurrentHitPoints, standardMonster.MaximumHitPoints);

            standardMonster.LootTable.ForEach(lootItem => currentMonster.LootTable.Add(lootItem));

            return currentMonster;
        }

        private void UpdateInventoryListInUI()
        {
            dgvInventory.RowHeadersVisible = false;

            dgvInventory.ColumnCount = 2;
            dgvInventory.Columns[0].Name = "Name";
            dgvInventory.Columns[0].Width = 197;
            dgvInventory.Columns[1].Name = "Quantity";

            dgvInventory.Rows.Clear();

            _player.Inventory.Where(item => item.Quantity > 0).ToList().ForEach(item => dgvInventory.Rows.Add([item.Details.Name, item.Quantity.ToString()]));
        }

        private void UpdateQuestListInUI()
        {
            dgvQuests.RowHeadersVisible = false;

            dgvQuests.ColumnCount = 2;
            dgvQuests.Columns[0].Name = "Name";
            dgvQuests.Columns[0].Width = 197;
            dgvQuests.Columns[1].Name = "Done?";

            dgvQuests.Rows.Clear();

            _player.Quests.ForEach(quest => dgvQuests.Rows.Add([quest.Details.Name, quest.IsCompleted.ToString()]));
        }

        private void UpdateWeaponListInUI()
        {
            List<Weapon> weapons = new List<Weapon>();

            _player.Inventory.Where(item => item.Details is Weapon && item.Quantity > 0).ToList().ForEach(item => weapons.Add((Weapon)item.Details));

            if (weapons.Count == 0)
            {
                cboWeapons.Visible = false;
                btnUseWeapon.Visible = false;
            }
            else
            {
                cboWeapons.DataSource = weapons;
                cboWeapons.DisplayMember = "Name";
                cboWeapons.ValueMember = "ID";
                cboWeapons.SelectedIndex = 0;
            }
        }

        private void UpdatePotionListInUI()
        {
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
            }
        }

        private void RefreshUI()
        {
            lblHitPoints.Text = _player.CurrentHitPoints.ToString();
            lblGold.Text = _player.Gold.ToString();
            lblExperience.Text = _player.ExperiencePoints.ToString();
            lblLevel.Text = _player.Level.ToString();
            UpdateInventoryListInUI();
            UpdateQuestListInUI();
            UpdateWeaponListInUI();
            UpdatePotionListInUI();
        }

        private void MonsterTakesTurn()
        {
            int damageToPlayer = RandomNumberGenerator.NumberBetween(0, _currentMonster.MaximumDamage);
            rtbMessages.AppendText("The " + _currentMonster.Name + " did " + damageToPlayer.ToString() + " points of damage." + Environment.NewLine);
            _player.CurrentHitPoints -= damageToPlayer;
            RefreshUI();
            if (_player.CurrentHitPoints <= 0)
            {
                rtbMessages.AppendText("The " + _currentMonster.Name + " killed you." + Environment.NewLine);
                MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
            }
        }
    }
}
