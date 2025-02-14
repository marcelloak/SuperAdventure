using System.ComponentModel;
using System.Xml;

namespace Engine
{
    public class Player : LivingCreature
    {
        private int _currentMana;
        public int CurrentMana
        {
            get { return _currentMana; }
            set
            {
                _currentMana = value;
                OnPropertyChanged("Mana");
            }
        }
        private int _maximumMana;
        public int MaximumMana
        {
            get { return _maximumMana; }
            set
            {
                _maximumMana = value;
                OnPropertyChanged("Mana");
            }
        }
        public string Mana { get { return _currentMana.ToString() + "/" + _maximumMana.ToString(); } }
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
            private set
            {
                _experiencePoints = value;
                OnPropertyChanged("ExperiencePointsDescription");
                OnPropertyChanged("Level");
            }
        }
        public int Level { get { return (ExperiencePoints / ExperiencePointsPerLevel) + 1; } }
        public int ExperiencePointsPerLevel = 100;
        public int ExperiencePointsToNextLevel { get { return ExperiencePointsPerLevel - ExperiencePoints % ExperiencePointsPerLevel; } }
        public string ExperiencePointsDescription { get { return _experiencePoints.ToString() + "/" + (_experiencePoints + ExperiencePointsToNextLevel).ToString(); } }
        public int AttributePointsToSpend { get; set; }
        public BindingList<InventoryItem> Inventory { get; set; }
        public List<InventoryItem> SellableInventory { get { return Inventory.Where(item => item.Details.Price >= 0).ToList(); } }
        public BindingList<Spell> Spellbook { get; set; }
        public List<Spell> UsableSpells { get { return Spellbook.Where(spell => Attributes.Intelligence >= spell.MinimumIntelligence).ToList(); } }
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

        public List<int> LocationsVisited { get; set; }
        public Weapon CurrentWeapon { get; set; }
        public List<Weapon> Weapons { get { return Inventory.Where(item => item.Details is Weapon).Select(item => item.Details as Weapon).ToList().Where(weapon => Level >= weapon.MinimumLevel && Attributes.Strength >= weapon.StrengthRequired && Attributes.Dexterity >= weapon.DexterityRequired).ToList(); } }
        public List<UsableItem> UsableItems { get { return Inventory.Where(item => item.Details is UsableItem && !(item.Details is Weapon)).Select(item => item.Details as UsableItem).ToList().Where(item => Level >= item.MinimumLevel).ToList(); } }
        private Monster CurrentMonster;
        private bool IsFasterThanCurrentMonster { get { return Attributes.Dexterity >= CurrentMonster.Attributes.Dexterity; } }
        public event EventHandler<MessageEventArgs> OnMessage;

        private Player(int currentHitPoints, int maximumHitPoints, int currentMana, int maximumMana, int gold, int experiencePoints) : base(currentHitPoints, maximumHitPoints)
        {
            CurrentMana = currentMana;
            MaximumMana = maximumMana;
            Gold = gold;
            ExperiencePoints = experiencePoints;
            AttributePointsToSpend = 0;
            Inventory = new BindingList<InventoryItem>();
            Spellbook = new BindingList<Spell>();
            Quests = new BindingList<PlayerQuest>();
            LocationsVisited = new List<int>();
        }

        public static Player CreateDefaultPlayer()
        {
            Player player = new Player(10, 10, 5, 5, 20, 0);
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
                int currentMana = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentMana").InnerText);
                int maximumMana = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/MaximumMana").InnerText);
                int gold = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/Gold").InnerText);
                int experiencePoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/ExperiencePoints").InnerText);
                int attributePointsToSpend = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/AttributePointsToSpend").InnerText);
                int currentLocationID = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentLocation").InnerText);
                int currentWeaponID = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentWeapon").InnerText);

                Player player = new Player(currentHitPoints, maximumHitPoints, currentMana, maximumMana, gold, experiencePoints);
                player.AttributePointsToSpend = attributePointsToSpend;
                player.CurrentLocation = World.LocationByID(currentLocationID);
                player.CurrentWeapon = (Weapon)World.ItemByID(currentWeaponID);

                foreach (XmlNode node in playerData.SelectNodes("/Player/LocationsVisited/LocationVisited"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    player.LocationsVisited.Add(id);
                }

                foreach (XmlNode node in playerData.SelectNodes("/Player/InventoryItems/InventoryItem"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    int quantity = Convert.ToInt32(node.Attributes["Quantity"].Value);
                    player.AddItemToInventory(World.ItemByID(id), quantity);
                }

                foreach (XmlNode node in playerData.SelectNodes("/Player/Spellbook/Spells"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    player.Spellbook.Add(World.SpellByID(id));
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
            while (ExperiencePointsToNextLevel <= experiencePointsToAdd)
            {
                int experienceUsed = ExperiencePointsToNextLevel;
                experiencePointsToAdd -= experienceUsed;
                ExperiencePoints += experienceUsed;
                LevelUp();
            }
            ExperiencePoints += experiencePointsToAdd;
        }

        public void LevelUp()
        {
            MaximumHitPoints += Attributes.Vitality;
            MaximumMana += Attributes.Intelligence / 5;
            RaiseMessage("");
            RaiseMessage("You levelled up to level " + Level);
            RaiseMessage("You gained " + Attributes.Vitality + " maximum health.");
            RaiseMessage("You gained " + Attributes.Intelligence / 5 + " maximum mana.");
            RaiseMessage("You gained 1 attribute point to spend.", true);
            AttributePointsToSpend++;
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
            else if (item is UsableItem) OnPropertyChanged("UsableItems");
        }

        private void LearnSpell(Spell spell)
        {
            if (spell.MinimumLevel <= Level)
            {
                Spellbook.Add(spell);
                RaiseMessage("You have learned " + spell.Name);
                OnPropertyChanged("UsableSpells");
            }
            else RaiseMessage("Your level is too low to learn " + spell.Name);
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
            if (!LocationsVisited.Contains(CurrentLocation.ID)) LocationsVisited.Add(CurrentLocation.ID);
            CurrentHitPoints = MaximumHitPoints;

            if (newLocation.HasAQuest)
            {
                if (HasThisQuest(newLocation.QuestAvailableHere))
                {
                    if (CompletedThisQuest(newLocation.QuestAvailableHere))
                    {
                        if (newLocation.QuestAvailableHere.IsRepeatable) ResetQuest(newLocation.QuestAvailableHere);
                    }
                    else
                    {
                        if (HasAllQuestCompletionItems(newLocation.QuestAvailableHere)) CompleteQuest(newLocation.QuestAvailableHere);
                        else PrintQuestDescription(newLocation.QuestAvailableHere, "already have");
                    }
                }
                else if (newLocation.QuestHereHasAPrerequisite)
                {
                    if (CompletedThisQuest(newLocation.QuestAvailableHere.Prerequisite)) AddQuest(newLocation.QuestAvailableHere);
                }
                else AddQuest(newLocation.QuestAvailableHere);
            }

            CurrentMonster = newLocation.NewInstanceOfMonsterLivingHere();
            if (CurrentMonster != null) RaiseMessage("You see a " + CurrentMonster.Name);
        }

        private void AddQuest(Quest quest)
        {
            PrintQuestDescription(quest);
            Quests.Add(new PlayerQuest(quest));
        }

        private void ResetQuest(Quest quest)
        {
            PlayerQuest playerQuest = Quests.SingleOrDefault(pq => pq.Details.ID == quest.ID);
            if (playerQuest == null) AddQuest(quest);
            else
            {
                PrintQuestDescription(quest, "reset");
                playerQuest.IsCompleted = false;
            }
            
        }

        private void PrintQuestDescription(Quest quest, string verb = "receive")
        {
            RaiseMessage("You " + verb  + " the '" + quest.Name + "' quest.");
            RaiseMessage(quest.Description);
            RaiseMessage("To complete it, return with:");
            quest.QuestCompletionItems.ForEach(questItem => RaiseMessage(questItem.Quantity.ToString() + " " + questItem.Description));
            RaiseMessage("");
        }

        private void CompleteQuest(Quest quest)
        {
            RaiseMessage("");
            RaiseMessage("You completed the '" + quest.Name + "' quest.");

            RemoveQuestCompletionItems(quest);

            RaiseMessage("You receive: ");
            RaiseMessage(quest.RewardExperiencePoints.ToString() + " experience points");
            RaiseMessage(quest.RewardGold.ToString() + " gold");
            RaiseMessage(quest.RewardItem.Name, true);

            AddExperiencePoints(quest.RewardExperiencePoints);
            Gold += quest.RewardGold;

            AddItemToInventory(quest.RewardItem);
            MarkQuestCompleted(quest);

            if (quest.IsRepeatable) RaiseMessage("This quest is repeatable. Return here to reset it.");
        }

        public void UseItem(Action<UsableItem> function, UsableItem currentItem)
        {
            bool turnSkipped = CheckForBeforeStatus(this);
            if (turnSkipped) MonsterTakesTurn();
            else
            {
                if (IsFasterThanCurrentMonster)
                {
                    function(currentItem);
                    if (DoesBattleEnd()) return;
                    MonsterTakesTurn();
                }
                else
                {
                    MonsterTakesTurn();
                    if (DoesBattleEnd()) return;
                    function(currentItem);
                }
            }
            CheckForAfterStatus(this);
            ResolveTurn();
        }

        public void Attack(UsableItem currentItem)
        {
            Weapon currentWeapon = currentItem as Weapon;
            bool hit = RandomNumberGenerator.NumberBetween(0, 100) < currentWeapon.HitChance + Level * 5 - CurrentMonster.Defence;
            int damage = 0;

            if (hit) damage = RandomNumberGenerator.NumberBetween(currentWeapon.MinimumDamage, currentWeapon.MaximumDamage);
            if (damage == 0) RaiseMessage("You missed the " + CurrentMonster.Name);
            else
            {
                damage += Attributes.Strength / 5;
                RaiseMessage("You hit the " + CurrentMonster.Name + " for " + damage.ToString() + " points.");
            }
            CurrentMonster.CurrentHitPoints -= damage;
        }

        public void UseHealingItem(UsableItem currentItem)
        {
            HealingItem healingItem = currentItem as HealingItem;
            CurrentHitPoints += healingItem.AmountToHeal;
            if (CurrentHitPoints > MaximumHitPoints) CurrentHitPoints = MaximumHitPoints;
            RemoveItemFromInventory(currentItem);
            RaiseMessage("You drink a " + healingItem.Name + " and heal for " + healingItem.AmountToHeal + " points.");
        }

        public void UseStatusItem(UsableItem currentItem)
        {
            Status status = (currentItem as StatusItem).StatusApplied;
            if (status.Turns == 1)
            {
                CurrentMonster.CurrentHitPoints -= status.Value;
                RaiseMessage("You throw a " + currentItem.Name + " and deal " + status.Value + " damage to the " + CurrentMonster.Name);
            }
            else
            {
                CurrentMonster.CurrentStatus = status.NewInstanceOfStatus(status.Value, status.Turns);
                RaiseMessage("You throw a " + currentItem.Name + " and " + status.Name + " the " + CurrentMonster.Name + " for " + status.Turns + " turns.");
            }
            RemoveItemFromInventory(currentItem);
        }

        public void UseScroll(UsableItem currentItem)
        {
            Scroll scroll = currentItem as Scroll;
            RaiseMessage("You use a " + currentItem.Name);
            CastSpell(scroll.SpellContained, this);
            RemoveItemFromInventory(currentItem);
        }

        public bool AttemptToCastSpell(Spell spell, LivingCreature user)
        {
            if (spell.ManaCost > CurrentMana)
            {
                RaiseMessage("You don't have the mana to cast " + spell.Name);
                return false;
            }
            CurrentMana -= spell.ManaCost;
            CastSpell(spell, user);
            return true;
        }

        public void CastSpell(Spell spell, LivingCreature user)
        {
            RaiseMessage("You cast " + spell.Name);
            String identifier = "You";
            LivingCreature target = this;

            if ((user != this && spell.Target == "Self") || (user == this && spell.Target == "Enemy"))
            {
                identifier = "The " + CurrentMonster.Name;
                target = CurrentMonster;
            }

            if (spell is HealingSpell)
            {
                HealingSpell heal = spell as HealingSpell;
                target.CurrentHitPoints += heal.AmountToHeal;
                if (target.CurrentHitPoints > target.MaximumHitPoints) target.CurrentHitPoints = target.MaximumHitPoints;
                RaiseMessage(identifier + " heal" + (target == this ? "" : "s") + " for " + heal.AmountToHeal + " points.");
            }
            else if (spell is StatusSpell)
            {
                Status status = (spell as StatusSpell).StatusApplied;
                if (status.Turns == 1)
                {
                    CurrentMonster.CurrentHitPoints -= status.Value;
                    RaiseMessage(identifier + " take" + (target == this ? " " : "s ") + status.Value + " damage.");
                }
                else
                {
                    CurrentMonster.CurrentStatus = status.NewInstanceOfStatus(status.Value, status.Turns);
                    RaiseMessage(identifier + (target == this ? " are " : " is ") + status.Description + " for " + status.Turns + " turns.");
                }
            }
        }

        private bool DoesBattleEnd()
        {
            if (IsDead) return PlayerDies();
            else if (CurrentMonster.IsDead) return MonsterDies();
            else return false;
        }

        private void ResolveTurn()
        {
            if (IsDead) PlayerDies();
            else if (CurrentMonster.IsDead) MonsterDies();
        }

        private void MonsterTakesTurn()
        {
            bool turnSkipped = CheckForBeforeStatus(CurrentMonster);
            if (!turnSkipped)
            {
                bool hit = RandomNumberGenerator.NumberBetween(0, 100) < CurrentMonster.HitChance - Level * 5;
                int damage = 0;

                if (hit) damage = RandomNumberGenerator.NumberBetween(0, CurrentMonster.MaximumDamage);
                if (damage == 0) RaiseMessage("The " + CurrentMonster.Name + " missed.");
                else
                {
                    damage += CurrentMonster.Attributes.Strength / 5;
                    RaiseMessage("The " + CurrentMonster.Name + " did " + damage.ToString() + " points of damage.");
                }
                CurrentHitPoints -= damage;
            }
            CheckForAfterStatus(CurrentMonster);
        }

        private bool CheckForBeforeStatus(LivingCreature livingCreature)
        {
            if (livingCreature.HasAStatus)
            {
                if (livingCreature.CurrentStatus.ID == World.STATUS_ID_POISON) return false;
            }
            return false;
        }

        private void CheckForAfterStatus(LivingCreature livingCreature)
        {
            if (livingCreature.HasAStatus)
            {
                String identifier = "You";
                if (livingCreature is Monster)
                {
                    Monster currentMonster = livingCreature as Monster;
                    identifier = "The " + currentMonster.Name;
                }
                RaiseMessage(identifier + (livingCreature == this ? " are " : " is ") + livingCreature.CurrentStatus.Description);

                if (livingCreature.CurrentStatus.ID == World.STATUS_ID_POISON)
                {
                    livingCreature.CurrentHitPoints -= livingCreature.CurrentStatus.Value;
                    RaiseMessage(identifier + (livingCreature == this ? " lose " : " lost ") + livingCreature.CurrentStatus.Value + " health to poison.");
                }

                if (livingCreature.CurrentStatus.Turns == 1)
                {
                    RaiseMessage(identifier + (livingCreature == this ? " are" : " is") + " no longer " + livingCreature.CurrentStatus.Description);
                    livingCreature.CurrentStatus = null;
                }
                else livingCreature.CurrentStatus.Turns--;
            }
        }

        private bool PlayerDies()
        {
            RaiseMessage("The " + CurrentMonster.Name + " killed you.");
            MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
            return true;
        }

        private bool MonsterDies()
        {
            RaiseMessage("");
            RaiseMessage("You defeated the " + CurrentMonster.Name);
            RaiseMessage("You receive " + CurrentMonster.RewardExperiencePoints.ToString() + " experience points");
            RaiseMessage("You receive " + CurrentMonster.RewardGold.ToString() + " gold");

            foreach (InventoryItem inventoryItem in CurrentMonster.LootItems)
            {
                AddItemToInventory(inventoryItem.Details);
                RaiseMessage("You loot " + inventoryItem.Quantity.ToString() + " " + inventoryItem.Description);
            }

            RaiseMessage("");

            AddExperiencePoints(CurrentMonster.RewardExperiencePoints);
            Gold += CurrentMonster.RewardGold;
            MoveTo(CurrentLocation);
            return true;
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
            currentHitPoints.AppendChild(playerData.CreateTextNode(CurrentHitPoints.ToString()));
            stats.AppendChild(currentHitPoints);

            XmlNode maximumHitPoints = playerData.CreateElement("MaximumHitPoints");
            maximumHitPoints.AppendChild(playerData.CreateTextNode(MaximumHitPoints.ToString()));
            stats.AppendChild(maximumHitPoints);

            XmlNode currentMana = playerData.CreateElement("CurrentMana");
            currentMana.AppendChild(playerData.CreateTextNode(CurrentMana.ToString()));
            stats.AppendChild(currentMana);

            XmlNode maximumMana = playerData.CreateElement("MaximumMana");
            maximumMana.AppendChild(playerData.CreateTextNode(MaximumMana.ToString()));
            stats.AppendChild(maximumMana);

            XmlNode gold = playerData.CreateElement("Gold");
            gold.AppendChild(playerData.CreateTextNode(Gold.ToString()));
            stats.AppendChild(gold);

            XmlNode experiencePoints = playerData.CreateElement("ExperiencePoints");
            experiencePoints.AppendChild(playerData.CreateTextNode(ExperiencePoints.ToString()));
            stats.AppendChild(experiencePoints);

            XmlNode attributePointsToSpend = playerData.CreateElement("AttributePointsToSpend");
            attributePointsToSpend.AppendChild(playerData.CreateTextNode(AttributePointsToSpend.ToString()));
            stats.AppendChild(attributePointsToSpend);

            XmlNode currentLocation = playerData.CreateElement("CurrentLocation");
            currentLocation.AppendChild(playerData.CreateTextNode(CurrentLocation.ID.ToString()));
            stats.AppendChild(currentLocation);

            XmlNode currentWeapon = playerData.CreateElement("CurrentWeapon");
            currentWeapon.AppendChild(playerData.CreateTextNode(CurrentWeapon.ID.ToString()));
            stats.AppendChild(currentWeapon);

            XmlNode locationsVisited = playerData.CreateElement("LocationsVisited");
            player.AppendChild(locationsVisited);

            foreach (int locationID in LocationsVisited)
            {
                XmlNode locationVisited = playerData.CreateElement("LocationVisited");
                XmlAttribute idAttribute = playerData.CreateAttribute("ID");
                idAttribute.Value = locationID.ToString();
                locationVisited.Attributes.Append(idAttribute);
                locationsVisited.AppendChild(locationVisited);
            }

            XmlNode inventoryItems = playerData.CreateElement("InventoryItems");
            player.AppendChild(inventoryItems);

            foreach (InventoryItem item in Inventory)
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

            XmlNode spellbook = playerData.CreateElement("Spellbook");
            player.AppendChild(spellbook);

            foreach (Spell spell in Spellbook)
            {
                XmlNode spellNode = playerData.CreateElement("Spell");
                XmlAttribute idAttribute = playerData.CreateAttribute("ID");
                idAttribute.Value = spell.ID.ToString();
                spellNode.Attributes.Append(idAttribute);
                spellbook.AppendChild(spellNode);
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

            foreach (PlayerQuest quest in Quests)
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
