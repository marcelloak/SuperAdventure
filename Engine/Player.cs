using System.ComponentModel;
using System.Xml;

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
            private set
            {
                _experiencePoints = value;
                OnPropertyChanged("ExperiencePointsDescription");
                OnPropertyChanged("Level");
            }
        }
        public int Level { get; set; }
        public int ExperiencePointsToNextLevel { get { return (Level * 100) + ((Level - 1) * (Level) / 2) * 5; } }
        public string ExperiencePointsDescription { get { return _experiencePoints.ToString() + "/" + ExperiencePointsToNextLevel.ToString(); } }
        public int AttributePointsToSpend { get; set; }
        public List<InventoryItem> SellableInventory { get { return Inventory.Where(item => item.Details.Price >= 0).ToList(); } }
        public List<Spell> UsableSpells { get { return Spellbook.Where(spell => TotalIntelligence >= spell.MinimumIntelligence).ToList(); } }
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
        public List<Weapon> Weapons { get { return Inventory.Where(item => item.Details is Weapon).Select(item => item.Details as Weapon).ToList().Where(weapon => Level >= weapon.MinimumLevel && TotalStrength >= weapon.StrengthRequired && TotalDexterity >= weapon.DexterityRequired).ToList(); } }
        public List<UsableItem> UsableItems { get { return Inventory.Where(item => item.Details is UsableItem && !(item.Details is Weapon)).Select(item => item.Details as UsableItem).ToList().Where(item => Level >= item.MinimumLevel).ToList(); } }
        public EquipmentSet Equipment {  get; set; }
        public int TotalStrength { get { return BaseAttributes.Strength + Equipment.GetStrengthIncreased(); } }
        public int TotalDexterity { get { return BaseAttributes.Dexterity + Equipment.GetDexterityIncreased(); } }
        public int TotalIntelligence{ get { return BaseAttributes.Intelligence + Equipment.GetIntelligenceIncreased(); } }
        public int TotalVitality { get { return BaseAttributes.Vitality + Equipment.GetVitalityIncreased(); } }
        private Monster CurrentMonster;
        public event EventHandler<MessageEventArgs> OnMessage;

        private Player(int currentHitPoints, int maximumHitPoints, int currentMana, int maximumMana, int gold, int experiencePoints) : base(currentHitPoints, maximumHitPoints, currentMana, maximumMana)
        {
            Level = 1;
            Gold = gold;
            ExperiencePoints = experiencePoints;
            AttributePointsToSpend = 0;
            Quests = new BindingList<PlayerQuest>();
            LocationsVisited = new List<int>();
            Equipment = new EquipmentSet();
        }

        public static Player CreateDefaultPlayer()
        {
            Player player = new Player(10, 10, 5, 5, 20, 0);
            player.Inventory.Add(new InventoryItem(World.ItemByID(World.ITEM_ID_RUSTY_SWORD), 1));
            player.Equipment.Head = World.ItemByID(World.ITEM_ID_HELM) as Equipment;
            player.Spellbook.Add(World.SpellByID(World.SPELL_ID_HEAL));
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
                int level = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/Level").InnerText);
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
                player.Level = level;
                player.AttributePointsToSpend = attributePointsToSpend;
                player.CurrentLocation = World.LocationByID(currentLocationID);
                player.CurrentWeapon = (Weapon)World.ItemByID(currentWeaponID);

                XmlNode baseAttributes = playerData.SelectSingleNode("/Player/Stats/BaseAttributes");
                int strength = Convert.ToInt32(baseAttributes.Attributes["Strength"].Value);
                int dexterity = Convert.ToInt32(baseAttributes.Attributes["Dexterity"].Value);
                int intelligence = Convert.ToInt32(baseAttributes.Attributes["Intelligence"].Value);
                int vitality = Convert.ToInt32(baseAttributes.Attributes["Vitality"].Value);
                player.BaseAttributes = new Attributes(strength, intelligence, dexterity, vitality);

                XmlNode currentStatus = playerData.SelectSingleNode("/Player/Stats/CurrentStatus");
                int currentStatusID = Convert.ToInt32(currentStatus.Attributes["ID"].Value);
                if (currentStatusID != 0)
                {
                    int currentStatusValue = Convert.ToInt32(currentStatus.Attributes["Value"].Value);
                    int currentStatusTurns = Convert.ToInt32(currentStatus.Attributes["Turns"].Value);
                    int currentStatusChanceToActivate = Convert.ToInt32(currentStatus.Attributes["ChanceToActivate"].Value);
                    int currentStatusChanceToCure = Convert.ToInt32(currentStatus.Attributes["ChanceToCure"].Value);
                    player.SetStatus(player, World.StatusByID(currentStatusID).NewInstanceOfStatus(currentStatusValue, currentStatusTurns, currentStatusChanceToActivate, currentStatusChanceToCure));
                }

                int headID = Convert.ToInt32(playerData.SelectSingleNode("/Player/Equipment/HeadID").InnerText);
                int armsID = Convert.ToInt32(playerData.SelectSingleNode("/Player/Equipment/ArmsID").InnerText);
                int handsID = Convert.ToInt32(playerData.SelectSingleNode("/Player/Equipment/HandsID").InnerText);
                int legsID = Convert.ToInt32(playerData.SelectSingleNode("/Player/Equipment/LegsID").InnerText);
                int feetID = Convert.ToInt32(playerData.SelectSingleNode("/Player/Equipment/FeetID").InnerText);

                if (headID != 0) player.Equipment.Head = World.ItemByID(headID) as Equipment;
                if (armsID != 0) player.Equipment.Arms = World.ItemByID(armsID) as Equipment;
                if (handsID != 0) player.Equipment.Hands = World.ItemByID(handsID) as Equipment;
                if (legsID != 0) player.Equipment.Legs = World.ItemByID(legsID) as Equipment;
                if (feetID != 0) player.Equipment.Feet = World.ItemByID(feetID) as Equipment;

                foreach (XmlNode node in playerData.SelectNodes("/Player/LocationsVisited/LocationVisited"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    player.LocationsVisited.Add(id);
                }

                foreach (XmlNode node in playerData.SelectNodes("/Player/InventoryItems/InventoryItem"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    int quantity = Convert.ToInt32(node.Attributes["Quantity"].Value);
                    player.AddItemToInventory(player, World.ItemByID(id), quantity);
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
            while (ExperiencePointsToNextLevel <= ExperiencePoints + experiencePointsToAdd)
            {
                int experienceUsed = ExperiencePointsToNextLevel - ExperiencePoints;
                experiencePointsToAdd -= experienceUsed;
                ExperiencePoints += experienceUsed;
                LevelUp();
            }
            ExperiencePoints += experiencePointsToAdd;
        }

        public void LevelUp()
        {
            Level++;
            MaximumHitPoints += TotalVitality;
            MaximumMana += TotalIntelligence / 5;
            RaiseMessage("");
            RaiseMessage("You levelled up to level " + Level);
            RaiseMessage("You gained " + TotalVitality + " maximum health.");
            RaiseMessage("You gained " + TotalIntelligence / 5 + " maximum mana.");
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
            quest.QuestCompletionItems.ForEach(questItem => RemoveItemFromInventory(this, questItem.Details, questItem.Quantity));
        }

        public void AddItemToInventory(LivingCreature user, Item itemToAdd, int quantity = 1)
        {
            InventoryItem item = user.Inventory.SingleOrDefault(ii => ii.Details.ID == itemToAdd.ID);
            if (item == null) user.Inventory.Add(new InventoryItem(itemToAdd, quantity));
            else item.Quantity += quantity;
            if (user == this) RaiseInventoryChangedEvent(itemToAdd);
        }

        public void RemoveItemFromInventory(LivingCreature user, Item itemToRemove, int quantity = 1)
        {
            InventoryItem item = user.Inventory.SingleOrDefault(ii => ii.Details.ID == itemToRemove.ID);
            if (item != null)
            {
                item.Quantity -= quantity;
                if (item.Quantity < 0) item.Quantity = 0; // TODO: Might want to raise an error instead
                if (item.Quantity == 0) user.Inventory.Remove(item);
                if (user == this) RaiseInventoryChangedEvent(itemToRemove);
            }
            // TODO: Might want to raise an error for if item is null
        }

        private void RaiseInventoryChangedEvent(Item item)
        {
            if (item is Weapon) OnPropertyChanged("Weapons");
            else if (item is UsableItem) OnPropertyChanged("UsableItems");
        }

        public void EquipItem(Equipment item)
        {
            RemoveItemFromInventory(this, item);
            Equipment.EquipItem(item);
        }

        public void UnequipSlot(string slot, bool keepItem = true)
        {
            if (slot == "Head")
            {
                if (Equipment.Head != null)
                {
                    if (keepItem) AddItemToInventory(this, Equipment.Head);
                    Equipment.Head = null;
                }
            }
            else if (slot == "Arms")
            {
                if (Equipment.Arms != null)
                {
                    if (keepItem) AddItemToInventory(this, Equipment.Arms);
                    Equipment.Arms = null;
                }
            }
            else if (slot == "Hands")
            {
                if (Equipment.Hands != null)
                {
                    if (keepItem) AddItemToInventory(this, Equipment.Hands);
                    Equipment.Hands = null;
                }
            }
            else if (slot == "Legs")
            {
                if (Equipment.Legs != null)
                {
                    if (keepItem) AddItemToInventory(this, Equipment.Legs);
                    Equipment.Legs = null;
                }
            }
            else if (slot == "Feet")
            {
                if (Equipment.Feet != null)
                {
                    if (keepItem) AddItemToInventory(this, Equipment.Feet);
                    Equipment.Feet = null;
                }
            }
        }

        public List<Equipment> GetEquipmentForSlot(string slot)
        {
            List<Equipment> equipment = Inventory.Where(item => item.Details is Equipment).Select(item => item.Details as Equipment).Where(equipment => equipment.Slot == slot).ToList();
            if (slot == "Head")
            {
                if (Equipment.Head != null) equipment.Add(Equipment.Head);
            }
            else if (slot == "Arms")
            {
                if (Equipment.Arms != null) equipment.Add(Equipment.Arms);
            }
            else if (slot == "Hands")
            {
                if (Equipment.Hands != null) equipment.Add(Equipment.Hands);
            }
            else if (slot == "Legs")
            {
                if (Equipment.Legs != null) equipment.Add(Equipment.Legs);
            }
            else if (slot == "Feet")
            {
                if (Equipment.Feet != null) equipment.Add(Equipment.Feet);
            }
            return equipment;
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
            CurrentMana = MaximumMana;
            ClearNegativeStatus(this);

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
            if (CurrentStatus != null) RaiseMessage("You are " + CurrentStatus.Description);
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

            AddItemToInventory(this, quest.RewardItem);
            MarkQuestCompleted(quest);

            if (quest.IsRepeatable) RaiseMessage("This quest is repeatable. Return here to reset it.");
        }

        public void TakeTurn(Func<LivingCreature, Object, bool> function, LivingCreature user, Object obj = null)
        {
            bool turnSkipped = CheckForSkippedTurn(this);
            if (turnSkipped) MonsterTakesTurn();
            else
            {
                if (IsFasterThanCurrentMonster())
                {
                    if (!function(user, obj)) return;
                    if (DoesBattleEnd())
                    {
                        CheckForAfterStatus(this);
                        return;
                    }
                    MonsterTakesTurn();
                }
                else
                {
                    MonsterTakesTurn();
                    if (DoesBattleEnd())
                    {
                        CheckForAfterStatus(this);
                        return;
                    }
                    if (!function(user, obj)) return;
                }
            }
            CheckForAfterStatus(this);
            ResolveTurn();
        }

        public bool WaitATurn(LivingCreature user, Object obj = null)
        {
            RaiseMessage("You waited.");
            return true;
        }

        public bool Attack(LivingCreature user, Object currentItem)
        {
            Weapon currentWeapon = currentItem as Weapon;
            bool hit = RandomNumberGenerator.NumberBetween(1, 100) <= currentWeapon.HitChance + Level * 5 - CurrentMonster.Defence;
            int damage = 0;

            if (hit) damage = RandomNumberGenerator.NumberBetween(currentWeapon.MinimumDamage, currentWeapon.MaximumDamage);
            if (damage == 0) RaiseMessage("You missed the " + CurrentMonster.Name);
            else
            {
                damage += TotalStrength / 5;
                RaiseMessage("You hit the " + CurrentMonster.Name + " for " + damage.ToString() + " points.");
            }
            CurrentMonster.CurrentHitPoints -= damage;
            return true;
        }

        public bool UseHealingItem(LivingCreature user, Object currentItem)
        {
            HealingItem healingItem = currentItem as HealingItem;
            user.CurrentHitPoints += healingItem.AmountToHeal;
            if (user.CurrentHitPoints > user.MaximumHitPoints) user.CurrentHitPoints = user.MaximumHitPoints;
            RemoveItemFromInventory(user, healingItem);
            string identifier = "You";
            if (user != this) identifier = "The " + (user as Monster).Name;
            RaiseMessage(identifier + " drink" + (user == this ? "" : "s") + " a " + healingItem.Name + " and heal" + (user == this ? "" : "s") + " for " + healingItem.AmountToHeal + " point" + (healingItem.AmountToHeal == 1 ? "" : "s"));
            return true;
        }

        public bool UseStatusItem(LivingCreature user, Object currentItem)
        {
            StatusItem statusItem = currentItem as StatusItem;
            Status status = (currentItem as StatusItem).StatusApplied;
            LivingCreature target = this;
            string identifier = "You";
            if (user == this)
            {
                target = CurrentMonster;
                identifier = "The " + CurrentMonster.Name;
            }
            if (status.Turns == 1)
            {
                target.CurrentHitPoints -= status.Value;
                RaiseMessage(identifier + " throw" + (user == this ? "" : "s") + " a " + statusItem.Name + " and deal" + (user == this ? "" : "s") + " " + status.Value + " damage to " + identifier);
            }
            else
            {
                SetStatus(target, status.NewInstanceOfStatus(status.Value, status.Turns, status.ChanceToActivate, status.ChanceToCure));
                RaiseMessage(identifier + " throw" + (user == this ? "" : "s") + " a " + statusItem.Name + " and " + status.Name + " " + identifier + (status.Turns == Int32.MaxValue ? "" : " for " + status.Turns + " turn" + (status.Turns == 1 ? "" : "s")));
            }
            RemoveItemFromInventory(user, statusItem);
            return true;
        }

        public bool UseScroll(LivingCreature user, Object currentItem)
        {
            Scroll scroll = currentItem as Scroll;
            string identifier = "You";
            if (user != this) identifier = "The " + (user as Monster).Name;
            RaiseMessage(identifier + " use" + (user == this ? "" : "s") + " a " + scroll.Name);
            CastSpell(user, scroll.SpellContained);
            RemoveItemFromInventory(user, scroll);
            return true;
        }

        public bool AttemptToCastSpell(LivingCreature user, Object spellObject)
        {
            Spell spell = spellObject as Spell;
            if (spell.ManaCost > CurrentMana)
            {
                RaiseMessage("You don't have the mana to cast " + spell.Name);
                return false;
            }
            CurrentMana -= spell.ManaCost;
            CastSpell(user, spell);
            return true;
        }

        public void CastSpell(LivingCreature user, Spell spell)
        {
            string userIdentifier = user == this ? "You" : "The " + CurrentMonster.Name;
            LivingCreature target = this;
            if ((user == this && spell.Target == "Enemy") || (user != this && spell.Target == "Self")) target = CurrentMonster;
            string targetIdentifier = target == this ? "You" : "The " + CurrentMonster.Name;

            RaiseMessage(userIdentifier + " cast" + (user == this ? "" : "s") + " " + spell.Name);

            if (spell is HealingSpell)
            {
                HealingSpell heal = spell as HealingSpell;
                target.CurrentHitPoints += heal.AmountToHeal;
                if (target.CurrentHitPoints > target.MaximumHitPoints) target.CurrentHitPoints = target.MaximumHitPoints;
                RaiseMessage(targetIdentifier + " heal" + (target == this ? "" : "s") + " for " + heal.AmountToHeal + " point" + (heal.AmountToHeal == 1 ? "" : "s"));
            }
            else if (spell is StatusSpell)
            {
                Status status = (spell as StatusSpell).StatusApplied;
                if (status.Turns == 1)
                {
                    target.CurrentHitPoints -= status.Value;
                    RaiseMessage(targetIdentifier + " take" + (target == this ? " " : "s ") + status.Value + " damage.");
                }
                else
                {
                    SetStatus(target, status.NewInstanceOfStatus(status.Value, status.Turns, status.ChanceToActivate, status.ChanceToCure));
                    RaiseMessage(targetIdentifier + (target == this ? " are " : " is ") + status.Description + (status.Turns == Int32.MaxValue ? "" : " for " + status.Turns + " turn" + (status.Turns == 1 ? "" : "s")));
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
            bool turnSkipped = CheckForSkippedTurn(CurrentMonster);
            if (!turnSkipped)
            {
                bool turnTaken = false;
                if (CurrentMonster.Inventory.Count > 0)
                {
                    bool useItem = RandomNumberGenerator.NumberBetween(1, 100) <= CurrentMonster.ChanceToUseItem;
                    if (useItem)
                    {
                        Item item = CurrentMonster.Inventory[RandomNumberGenerator.NumberBetween(0, CurrentMonster.Inventory.Count - 1)].Details;
                        if (item is HealingItem) UseHealingItem(CurrentMonster, item);
                        else if (item is StatusItem) UseStatusItem(CurrentMonster, item);
                        else if (item is Scroll) UseScroll(CurrentMonster, item);
                        RemoveItemFromInventory(CurrentMonster, item);
                        turnTaken = true;
                    }
                }
                if (!turnTaken && CurrentMonster.CastableSpells.Count > 0)
                {
                    bool castSpell = RandomNumberGenerator.NumberBetween(1, 100) <= CurrentMonster.ChanceToCastSpell;
                    if (castSpell)
                    {
                        Spell spell = CurrentMonster.CastableSpells[RandomNumberGenerator.NumberBetween(0, CurrentMonster.CastableSpells.Count - 1)];
                        CastSpell(CurrentMonster, spell);
                        turnTaken = true;
                    }
                }
                if (!turnTaken)
                {
                    bool hit = RandomNumberGenerator.NumberBetween(1, 100) <= CurrentMonster.HitChance - Level * 5 - Equipment.GetDefence();
                    int damage = 0;

                    if (hit) damage = RandomNumberGenerator.NumberBetween(0, CurrentMonster.MaximumDamage);
                    if (damage == 0) RaiseMessage("The " + CurrentMonster.Name + " missed.");
                    else
                    {
                        damage += CurrentMonster.BaseAttributes.Strength / 5;
                        RaiseMessage("The " + CurrentMonster.Name + " did " + damage.ToString() + " points of damage.");
                    }
                    CurrentHitPoints -= damage;
                }
            }
            CheckForAfterStatus(CurrentMonster);
        }

        private bool CheckForSkippedTurn(LivingCreature livingCreature)
        {
            if (livingCreature.HasAStatus)
            {
                string identifier = "You";
                if (livingCreature is Monster)
                {
                    Monster currentMonster = livingCreature as Monster;
                    identifier = "The " + currentMonster.Name;
                }

                bool activated = true;
                if (livingCreature.CurrentStatus.ChanceToActivate < 100) activated = RandomNumberGenerator.NumberBetween(1, 100) <= livingCreature.CurrentStatus.ChanceToActivate;

                if (activated)
                {
                    if (livingCreature.CurrentStatus.ID == World.STATUS_ID_SLEEP || livingCreature.CurrentStatus.ID == World.STATUS_ID_PARALYZE || livingCreature.CurrentStatus.ID == World.STATUS_ID_FROZEN || livingCreature.CurrentStatus.ID == World.STATUS_ID_STOP)
                    {
                        RaiseMessage(identifier + " missed " + (livingCreature == this ? "your" : "their") + " turn because " + (livingCreature == this ? "you" : "they") + " were " + livingCreature.CurrentStatus.Description);
                        return true;
                    }
                }  
            }
            return false;
        }

        private void CheckForAfterStatus(LivingCreature livingCreature)
        {
            if (livingCreature.HasAStatus)
            {
                string identifier = "You";
                if (livingCreature is Monster)
                {
                    Monster currentMonster = livingCreature as Monster;
                    identifier = "The " + currentMonster.Name;
                }
                RaiseMessage(identifier + (livingCreature == this ? " are " : " is ") + livingCreature.CurrentStatus.Description);

                bool activated = true;
                if (livingCreature.CurrentStatus.ChanceToActivate < 100) activated = RandomNumberGenerator.NumberBetween(1, 100) <= livingCreature.CurrentStatus.ChanceToActivate;

                if (activated)
                {
                    if (livingCreature.CurrentStatus.ID == World.STATUS_ID_POISON || livingCreature.CurrentStatus.ID == World.STATUS_ID_BURN)
                    {
                        livingCreature.CurrentHitPoints -= livingCreature.CurrentStatus.Value;
                        RaiseMessage(identifier + (livingCreature == this ? " lose " : " lost ") + livingCreature.CurrentStatus.Value + " health to " + livingCreature.CurrentStatus.Name);
                    }
                }

                if (livingCreature.CurrentStatus.Turns == 1)
                {
                    if (livingCreature.CurrentStatus.ID == World.STATUS_ID_PETRIFY || livingCreature.CurrentStatus.ID == World.STATUS_ID_DEATH)
                    {
                        RaiseMessage(identifier + " died because " + (livingCreature == this ? "you were " : "it was ") + livingCreature.CurrentStatus.Description);
                        livingCreature.CurrentHitPoints = 0;
                    }
                    else ClearStatus(livingCreature, identifier);
                }
                else if (livingCreature.CurrentStatus.ChanceToCure > 0)
                {
                    bool cured = RandomNumberGenerator.NumberBetween(1, 100) <= livingCreature.CurrentStatus.ChanceToCure;
                    if (cured) ClearStatus(livingCreature, identifier);
                }
                else if (livingCreature.CurrentStatus.Turns != Int32.MaxValue) livingCreature.CurrentStatus.Turns--;
            }
        }

        private int CheckSpeed(LivingCreature livingCreature)
        {
            if (livingCreature.HasAStatus)
            {
                if (livingCreature.CurrentStatus.ID == World.STATUS_ID_HASTE) return Int32.MaxValue;
                if (livingCreature.CurrentStatus.ID == World.STATUS_ID_SLOW) return Int32.MinValue;
            }
            if (livingCreature == this) return TotalDexterity;
            return livingCreature.BaseAttributes.Dexterity;
        }

        private bool IsFasterThanCurrentMonster()
        {
            return CheckSpeed(this) >= CheckSpeed(CurrentMonster);
        }

        private void SetStatus(LivingCreature livingCreature, Status status)
        {
            livingCreature.CurrentStatus = status;
            if (livingCreature == this) OnPropertyChanged("Status");
        }

        private void ClearNegativeStatus(LivingCreature livingCreature)
        {
            if (livingCreature.HasAStatus && livingCreature.HasANegativeStatus) ClearStatus(livingCreature, "You");
        }

        private void ClearStatus(LivingCreature livingCreature, string identifier = "")
        {
            if (identifier != "") RaiseMessage(identifier + (livingCreature == this ? " are" : " is") + " no longer " + livingCreature.CurrentStatus.Description);
            livingCreature.CurrentStatus = null;
            if (livingCreature == this) OnPropertyChanged("Status");
        }

        private bool PlayerDies()
        {
            if (CurrentMonster != null) RaiseMessage("The " + CurrentMonster.Name + " killed you.");
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
                AddItemToInventory(this, inventoryItem.Details);
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

            XmlNode level = playerData.CreateElement("Level");
            level.AppendChild(playerData.CreateTextNode(Level.ToString()));
            stats.AppendChild(level);

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

            XmlNode baseAttributes = playerData.CreateElement("BaseAttributes");
            XmlAttribute strengthAttribute = playerData.CreateAttribute("Strength");
            strengthAttribute.Value = BaseAttributes.Strength.ToString();
            baseAttributes.Attributes.Append(strengthAttribute);
            XmlAttribute dexterityAttribute = playerData.CreateAttribute("Dexterity");
            dexterityAttribute.Value = BaseAttributes.Dexterity.ToString();
            baseAttributes.Attributes.Append(dexterityAttribute);
            XmlAttribute intelligenceAttribute = playerData.CreateAttribute("Intelligence");
            intelligenceAttribute.Value = BaseAttributes.Intelligence.ToString();
            baseAttributes.Attributes.Append(intelligenceAttribute);
            XmlAttribute vitalityAttribute = playerData.CreateAttribute("Vitality");
            vitalityAttribute.Value = BaseAttributes.Vitality.ToString();
            baseAttributes.Attributes.Append(vitalityAttribute);
            stats.AppendChild(baseAttributes);

            XmlNode currentStatus = playerData.CreateElement("CurrentStatus");
            if (HasAStatus)
            {
                XmlAttribute statusIdAttribute = playerData.CreateAttribute("ID");
                statusIdAttribute.Value = CurrentStatus.ID.ToString();
                currentStatus.Attributes.Append(statusIdAttribute);
                XmlAttribute valueAttribute = playerData.CreateAttribute("Value");
                valueAttribute.Value = CurrentStatus.Value.ToString();
                currentStatus.Attributes.Append(valueAttribute);
                XmlAttribute turnsAttribute = playerData.CreateAttribute("Turns");
                turnsAttribute.Value = CurrentStatus.Turns.ToString();
                currentStatus.Attributes.Append(turnsAttribute);
                XmlAttribute chanceToActivateAttribute = playerData.CreateAttribute("ChanceToActivate");
                chanceToActivateAttribute.Value = CurrentStatus.ChanceToActivate.ToString();
                currentStatus.Attributes.Append(chanceToActivateAttribute);
                XmlAttribute chanceToCureAttribute = playerData.CreateAttribute("ChanceToCure");
                chanceToCureAttribute.Value = CurrentStatus.ChanceToCure.ToString();
                currentStatus.Attributes.Append(chanceToCureAttribute);
            }
            else
            {
                XmlAttribute statusIdAttribute = playerData.CreateAttribute("ID");
                statusIdAttribute.Value = "0";
                currentStatus.Attributes.Append(statusIdAttribute);
            }
            stats.AppendChild(currentStatus);

            XmlNode equipment = playerData.CreateElement("Equipment");
            player.AppendChild(equipment);
            XmlNode head = playerData.CreateElement("HeadID");
            if (Equipment.Head != null) head.AppendChild(playerData.CreateTextNode(Equipment.Head.ID.ToString()));
            else head.AppendChild(playerData.CreateTextNode("0"));
            equipment.AppendChild(head);
            XmlNode arms = playerData.CreateElement("ArmsID");
            if (Equipment.Arms != null) arms.AppendChild(playerData.CreateTextNode(Equipment.Arms.ID.ToString()));
            else arms.AppendChild(playerData.CreateTextNode("0"));
            equipment.AppendChild(arms);
            XmlNode hands = playerData.CreateElement("HandsID");
            if (Equipment.Hands != null) hands.AppendChild(playerData.CreateTextNode(Equipment.Hands.ID.ToString()));
            else hands.AppendChild(playerData.CreateTextNode("0"));
            equipment.AppendChild(hands);
            XmlNode legs = playerData.CreateElement("LegsID");
            if (Equipment.Legs != null) legs.AppendChild(playerData.CreateTextNode(Equipment.Legs.ID.ToString()));
            else legs.AppendChild(playerData.CreateTextNode("0"));
            equipment.AppendChild(legs);
            XmlNode feet = playerData.CreateElement("FeetID");
            if (Equipment.Feet != null) feet.AppendChild(playerData.CreateTextNode(Equipment.Feet.ID.ToString()));
            else feet.AppendChild(playerData.CreateTextNode("0"));
            equipment.AppendChild(feet);

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
