using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Engine
{
    public class Player : LivingCreature
    {
        private int _gold;
        public int Gold
        {
            get { return _gold; }
            set
            {
                _gold = value;
                OnPropertyChanged("Gold");
            }
        }
        private int _experiencePoints;
        public int ExperiencePoints
        {
            get { return _experiencePoints; }
            set
            {
                _experiencePoints = value;
                OnPropertyChanged("ExperiencePoints");
                OnPropertyChanged("Level");
            }
        }
        public int Level { get { return (ExperiencePoints / 100) + 1; } }
        public BindingList<InventoryItem> Inventory { get; set; } // TODO: See if possible to filter out unsellable items for trade screen
        public List<InventoryItem> SellableInventory { get { return Inventory.Where(item => item.Details.Price >= 0).ToList(); } }
        public BindingList<PlayerQuest> Quests { get; set; }
        private Location _currentLocation;
        public Location CurrentLocation
        {
            get { return _currentLocation; }
            set
            {
                _currentLocation = value;
                OnPropertyChanged("CurrentLocation");
            }
        }
        public Weapon CurrentWeapon { get; set; }
        public List<Weapon> Weapons { get { return Inventory.Where(item => item.Details is Weapon).Select(item => item.Details as Weapon).ToList().Where(weapon => Level >= weapon.MinimumLevel).ToList(); } }
        public List<HealingPotion> Potions { get { return Inventory.Where(item => item.Details is HealingPotion).Select(item => item.Details as HealingPotion).ToList().Where(potion => Level >= potion.MinimumLevel).ToList(); } }
        private Monster CurrentMonster;
        public event EventHandler<MessageEventArgs> OnMessage;

        private Player(int currentHitPoints, int maximumHitPoints, int gold, int experiencePoints) : base(currentHitPoints, maximumHitPoints)
        {
            Gold = gold;
            ExperiencePoints = experiencePoints;
            Inventory = new BindingList<InventoryItem>();
            Quests = new BindingList<PlayerQuest>();
        }

        public static Player CreateDefaultPlayer()
        {
            Player player = new Player(10, 10, 20, 0);
            player.Inventory.Add(new InventoryItem(World.ItemByID(World.ITEM_ID_RUSTY_SWORD), 1));
            player.CurrentWeapon = (Weapon)World.ItemByID(World.ITEM_ID_RUSTY_SWORD);
            player.CurrentLocation = World.LocationByID(World.LOCATION_ID_HOME);
            return player;
        }

        public static Player CreatePlayerFromXmlString(string xmlPlayerData)
        {
            try
            {
                XmlDocument playerData = new XmlDocument();
                playerData.LoadXml(xmlPlayerData);
                int currentHitPoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentHitPoints").InnerText);
                int maximumHitPoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/MaximumHitPoints").InnerText);
                int gold = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/Gold").InnerText);
                int experiencePoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/ExperiencePoints").InnerText);
                int currentLocationID = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentLocation").InnerText);
                int currentWeaponID = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentWeapon").InnerText);

                Player player = new Player(currentHitPoints, maximumHitPoints, gold, experiencePoints);
                player.CurrentLocation = World.LocationByID(currentLocationID);
                player.CurrentWeapon = (Weapon)World.ItemByID(currentWeaponID);

                foreach (XmlNode node in playerData.SelectNodes("/Player/InventoryItems/InventoryItem"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    int quantity = Convert.ToInt32(node.Attributes["Quantity"].Value);
                    player.AddItemToInventory(World.ItemByID(id), quantity);
                }

                foreach (XmlNode node in playerData.SelectNodes("/Player/VendorInventories"))
                {
                    int vendorId = Convert.ToInt32(node.SelectSingleNode(".//VendorID").InnerText);
                    Vendor vendor = World.VendorByID(vendorId);
                    vendor.ClearInventory();
                    foreach (XmlNode item in node.SelectNodes(".//InventoryItem"))
                    {
                        int id = Convert.ToInt32(item.Attributes["ID"].Value);
                        int quantity = Convert.ToInt32(item.Attributes["Quantity"].Value);
                        vendor.AddItemToInventory(World.ItemByID(id), quantity);
                    }
                }

                foreach (XmlNode node in playerData.SelectNodes("/Player/PlayerQuests/PlayerQuest"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    bool isCompleted = Convert.ToBoolean(node.Attributes["IsCompleted"].Value);
                    PlayerQuest playerQuest = new PlayerQuest(World.QuestByID(id));
                    playerQuest.IsCompleted = isCompleted;
                    player.Quests.Add(playerQuest);
                }
                return player;
            }
            catch
            {
                return Player.CreateDefaultPlayer();
            }
        }

        public void AddExperiencePoints(int experiencePointsToAdd)
        {
            ExperiencePoints += experiencePointsToAdd;
            MaximumHitPoints = (Level * 10);
        }

        public bool HasRequiredItemToEnterThisLocation(Location location)
        {
            if (location.ItemRequiredToEnter == null) return true;
            return Inventory.Any(item => item.Details.ID == location.ItemRequiredToEnter.ID);
        }

        public bool HasRequiredLevelToEnterThisLocation(Location location)
        {
            return Level >= location.LevelRequiredToEnter;
        }

        public bool HasThisQuest(Quest quest)
        {
            return Quests.Any(playerQuest => playerQuest.Details.ID == quest.ID);
        }

        public bool CompletedThisQuest(Quest quest)
        {
            PlayerQuest playerQuest = Quests.SingleOrDefault(pq => pq.Details.ID == quest.ID);
            if (playerQuest == null) return false;
            return playerQuest.IsCompleted;
        }

        public bool HasAllQuestCompletionItems(Quest quest)
        {
            foreach (QuestCompletionItem questItem in quest.QuestCompletionItems)
            {
                if (!Inventory.Any(item => item.Details.ID == questItem.Details.ID && item.Quantity >= questItem.Quantity)) return false;
            }
            return true;
        }

        public void RemoveQuestCompletionItems(Quest quest)
        {
            quest.QuestCompletionItems.ForEach(questItem => RemoveItemFromInventory(questItem.Details, questItem.Quantity));
        }

        public void AddItemToInventory(Item itemToAdd, int quantity = 1)
        {
            InventoryItem item = Inventory.SingleOrDefault(ii => ii.Details.ID == itemToAdd.ID);
            if (item == null) Inventory.Add(new InventoryItem(itemToAdd, quantity));
            else item.Quantity += quantity;
            RaiseInventoryChangedEvent(itemToAdd);
        }

        public void RemoveItemFromInventory(Item itemToRemove, int quantity = 1)
        {
            InventoryItem item = Inventory.SingleOrDefault(ii => ii.Details.ID == itemToRemove.ID);
            if (item != null)
            {
                item.Quantity -= quantity;
                if (item.Quantity < 0) item.Quantity = 0; // TODO: Might want to raise an error instead
                if (item.Quantity == 0) Inventory.Remove(item);
                RaiseInventoryChangedEvent(itemToRemove);
            }
            // TODO: Might want to raise an error for if item is null
        }

        private void RaiseInventoryChangedEvent(Item item)
        {
            if (item is Weapon) OnPropertyChanged("Weapons");
            else if (item is HealingPotion) OnPropertyChanged("Potions");
        }

        public void MarkQuestCompleted(Quest quest)
        {
            PlayerQuest playerQuest = Quests.SingleOrDefault(pq => pq.Details.ID == quest.ID);
            if (playerQuest != null) playerQuest.IsCompleted = true;
        }

        public void MoveTo(Location newLocation)
        {
            if (!HasRequiredItemToEnterThisLocation(newLocation))
            {
                RaiseMessage("You must have a " + newLocation.ItemRequiredToEnter.Name + " to enter this location.");
                return;
            }

            if (!HasRequiredLevelToEnterThisLocation(newLocation))
            {
                RaiseMessage("You must be level " + newLocation.LevelRequiredToEnter + " or higher to enter this location.");
                return;
            }

            CurrentLocation = newLocation;
            CurrentHitPoints = MaximumHitPoints;

            if (newLocation.QuestAvailableHere != null)
            {
                if (HasThisQuest(newLocation.QuestAvailableHere))
                {
                    if (!CompletedThisQuest(newLocation.QuestAvailableHere) && HasAllQuestCompletionItems(newLocation.QuestAvailableHere))
                    {
                        RaiseMessage("");
                        RaiseMessage("You completed the '" + newLocation.QuestAvailableHere.Name + "' quest.");

                        RemoveQuestCompletionItems(newLocation.QuestAvailableHere);

                        RaiseMessage("You receive: ");
                        RaiseMessage(newLocation.QuestAvailableHere.RewardExperiencePoints.ToString() + " experience points");
                        RaiseMessage(newLocation.QuestAvailableHere.RewardGold.ToString() + " gold");
                        RaiseMessage(newLocation.QuestAvailableHere.RewardItem.Name, true);

                        AddExperiencePoints(newLocation.QuestAvailableHere.RewardExperiencePoints);
                        Gold += newLocation.QuestAvailableHere.RewardGold;

                        AddItemToInventory(newLocation.QuestAvailableHere.RewardItem);
                        MarkQuestCompleted(newLocation.QuestAvailableHere);
                    }
                }
                else
                {
                    RaiseMessage("You receive the " + newLocation.QuestAvailableHere.Name + " quest.");
                    RaiseMessage(newLocation.QuestAvailableHere.Description);
                    RaiseMessage("To complete it, return with:");
                    newLocation.QuestAvailableHere.QuestCompletionItems.ForEach(questItem => RaiseMessage(questItem.Quantity.ToString() + " " + questItem.Description));
                    RaiseMessage("");

                    Quests.Add(new PlayerQuest(newLocation.QuestAvailableHere));
                }
            }

            if (newLocation.MonsterLivingHere != null)
            {
                RaiseMessage("You see a " + newLocation.MonsterLivingHere.Name);
                CurrentMonster = CreateMonsterInstance(newLocation.MonsterLivingHere);

            }
            else CurrentMonster = null;
        }

        private Monster CreateMonsterInstance(Monster monster)
        {
            Monster standardMonster = World.MonsterByID(monster.ID);
            Monster currentMonster = new Monster(standardMonster.ID, standardMonster.Name, standardMonster.MaximumDamage, standardMonster.RewardGold, standardMonster.RewardExperiencePoints, standardMonster.CurrentHitPoints, standardMonster.MaximumHitPoints);
            standardMonster.LootTable.ForEach(lootItem => currentMonster.LootTable.Add(lootItem));
            return currentMonster;
        }

        public void UseWeapon(Weapon currentWeapon)
        {
            int damageToMonster = RandomNumberGenerator.NumberBetween(currentWeapon.MinimumDamage, currentWeapon.MaximumDamage);
            CurrentMonster.CurrentHitPoints -= damageToMonster;
            RaiseMessage("You hit the " + CurrentMonster.Name + " for " + damageToMonster.ToString() + " points.");

            if (CurrentHitPoints <= 0) PlayerDies();
            else if (CurrentMonster.CurrentHitPoints <= 0) MonsterDies();
            else MonsterTakesTurn();
        }

        public void UsePotion(HealingPotion potion)
        {
            CurrentHitPoints += potion.AmountToHeal;
            if (CurrentHitPoints > MaximumHitPoints) CurrentHitPoints = MaximumHitPoints;
            RemoveItemFromInventory(potion);
            RaiseMessage("You drink a " + potion.Name + " and heal for " + potion.AmountToHeal + " points.");

            if (CurrentHitPoints <= 0) PlayerDies();
            else if (CurrentMonster.CurrentHitPoints <= 0) MonsterDies();
            else MonsterTakesTurn();
        }

        private void MonsterTakesTurn()
        {
            int damageToPlayer = RandomNumberGenerator.NumberBetween(0, CurrentMonster.MaximumDamage);
            RaiseMessage("The " + CurrentMonster.Name + " did " + damageToPlayer.ToString() + " points of damage.");
            CurrentHitPoints -= damageToPlayer;

            if (CurrentHitPoints <= 0) PlayerDies();
            else if (CurrentMonster.CurrentHitPoints <= 0) MonsterDies();
        }

        private void PlayerDies()
        {
            RaiseMessage("The " + CurrentMonster.Name + " killed you.");
            MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
        }

        private void MonsterDies()
        {
            RaiseMessage("");
            RaiseMessage("You defeated the " + CurrentMonster.Name);
            AddExperiencePoints(CurrentMonster.RewardExperiencePoints);
            RaiseMessage("You receive " + CurrentMonster.RewardExperiencePoints.ToString() + " experience points");
            Gold += CurrentMonster.RewardGold;
            RaiseMessage("You receive " + CurrentMonster.RewardGold.ToString() + " gold");

            List<InventoryItem> lootedItems = new List<InventoryItem>();
            CurrentMonster.LootTable.Where(lootItem => RandomNumberGenerator.NumberBetween(1, 100) <= lootItem.DropPercentage).ToList().ForEach(lootItem => lootedItems.Add(new InventoryItem(lootItem.Details, 1)));
            if (lootedItems.Count == 0) CurrentMonster.LootTable.Where(lootItem => lootItem.IsDefaultItem).ToList().ForEach(lootItem => lootedItems.Add(new InventoryItem(lootItem.Details, 1)));

            foreach (InventoryItem inventoryItem in lootedItems)
            {
                AddItemToInventory(inventoryItem.Details);
                RaiseMessage("You loot " + inventoryItem.Quantity.ToString() + " " + inventoryItem.Description);
            }

            RaiseMessage("");
            MoveTo(CurrentLocation);
        }

        private void RaiseMessage(string message, bool addExtraNewLine = false)
        {
            if (OnMessage != null) OnMessage(this, new MessageEventArgs(message, addExtraNewLine));
        }

        public string ToXmlString()
        {
            XmlDocument playerData = new XmlDocument();
            XmlNode player = playerData.CreateElement("Player");
            playerData.AppendChild(player);

            XmlNode stats = playerData.CreateElement("Stats");
            player.AppendChild(stats);

            XmlNode currentHitPoints = playerData.CreateElement("CurrentHitPoints");
            currentHitPoints.AppendChild(playerData.CreateTextNode(this.CurrentHitPoints.ToString()));
            stats.AppendChild(currentHitPoints);

            XmlNode maximumHitPoints = playerData.CreateElement("MaximumHitPoints");
            maximumHitPoints.AppendChild(playerData.CreateTextNode(this.MaximumHitPoints.ToString()));
            stats.AppendChild(maximumHitPoints);

            XmlNode gold = playerData.CreateElement("Gold");
            gold.AppendChild(playerData.CreateTextNode(this.Gold.ToString()));
            stats.AppendChild(gold);

            XmlNode experiencePoints = playerData.CreateElement("ExperiencePoints");
            experiencePoints.AppendChild(playerData.CreateTextNode(this.ExperiencePoints.ToString()));
            stats.AppendChild(experiencePoints);

            XmlNode currentLocation = playerData.CreateElement("CurrentLocation");
            currentLocation.AppendChild(playerData.CreateTextNode(this.CurrentLocation.ID.ToString()));
            stats.AppendChild(currentLocation);

            XmlNode currentWeapon = playerData.CreateElement("CurrentWeapon");
            currentWeapon.AppendChild(playerData.CreateTextNode(this.CurrentWeapon.ID.ToString()));
            stats.AppendChild(currentWeapon);

            XmlNode inventoryItems = playerData.CreateElement("InventoryItems");
            player.AppendChild(inventoryItems);

            foreach (InventoryItem item in this.Inventory)
            {
                XmlNode inventoryItem = playerData.CreateElement("InventoryItem");
                XmlAttribute idAttribute = playerData.CreateAttribute("ID");
                idAttribute.Value = item.Details.ID.ToString();
                inventoryItem.Attributes.Append(idAttribute);
                XmlAttribute quantityAttribute = playerData.CreateAttribute("Quantity");
                quantityAttribute.Value = item.Quantity.ToString();
                inventoryItem.Attributes.Append(quantityAttribute);
                inventoryItems.AppendChild(inventoryItem);
            }

            XmlNode vendorInventories = playerData.CreateElement("VendorInventories");
            player.AppendChild(vendorInventories);

            foreach (Vendor v in World.Vendors)
            {
                XmlNode vendor = playerData.CreateElement("Vendor");
                vendorInventories.AppendChild(vendor);
                XmlNode vendorID = playerData.CreateElement("VendorID");
                vendorID.AppendChild(playerData.CreateTextNode(v.ID.ToString()));
                vendor.AppendChild(vendorID);
                foreach (InventoryItem item in v.Inventory)
                {
                    XmlNode inventoryItem = playerData.CreateElement("InventoryItem");
                    XmlAttribute idAttribute = playerData.CreateAttribute("ID");
                    idAttribute.Value = item.Details.ID.ToString();
                    inventoryItem.Attributes.Append(idAttribute);
                    XmlAttribute quantityAttribute = playerData.CreateAttribute("Quantity");
                    quantityAttribute.Value = item.Quantity.ToString();
                    inventoryItem.Attributes.Append(quantityAttribute);
                    vendor.AppendChild(inventoryItem);
                }
            }

            XmlNode playerQuests = playerData.CreateElement("PlayerQuests");
            player.AppendChild(playerQuests);

            foreach (PlayerQuest quest in this.Quests)
            {
                XmlNode playerQuest = playerData.CreateElement("PlayerQuest");
                XmlAttribute idAttribute = playerData.CreateAttribute("ID");
                idAttribute.Value = quest.Details.ID.ToString();
                playerQuest.Attributes.Append(idAttribute);
                XmlAttribute isCompletedAttribute = playerData.CreateAttribute("IsCompleted");
                isCompletedAttribute.Value = quest.IsCompleted.ToString();
                playerQuest.Attributes.Append(isCompletedAttribute);
                playerQuests.AppendChild(playerQuest);
            }

            return playerData.InnerXml;
        }
    }
}
