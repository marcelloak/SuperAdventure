﻿namespace Engine
{
    public static class World
    {
        public static readonly List<Status> Statuses = new List<Status>();
        public static readonly List<Spell> Spells = new List<Spell>();
        public static readonly List<Item> Items = new List<Item>();
        public static readonly List<Monster> Monsters = new List<Monster>();
        public static readonly List<Quest> Quests = new List<Quest>();
        public static readonly List<Location> Locations = new List<Location>();
        public static readonly List<Vendor> Vendors = new List<Vendor>();

        public const int STATUS_ID_POISON = 1;
        public const int STATUS_ID_SLEEP = 2;
        public const int STATUS_ID_HASTE = 3;
        public const int STATUS_ID_PARALYZE = 4;
        public const int STATUS_ID_FROZEN = 5;
        public const int STATUS_ID_BURN = 6;

        public const int SPELL_ID_HEAL = 1;
        public const int SPELL_ID_FIREBALL = 2;
        public const int SPELL_ID_POISON = 3;
        public const int SPELL_ID_SLEEP = 4;
        public const int SPELL_ID_HASTE = 5;
        public const int SPELL_ID_PARALYZE = 6;
        public const int SPELL_ID_FROZEN = 7;
        public const int SPELL_ID_BURN = 8;

        public const int ITEM_ID_RUSTY_SWORD = 1;
        public const int ITEM_ID_CLUB = 2;

        public const int ITEM_ID_HEALING_POTION = 3;
        public const int ITEM_ID_POISON = 4;
        public const int ITEM_ID_HEAL_SCROLL = 5;
        public const int ITEM_ID_FIREBALL_SCROLL = 6;
        public const int ITEM_ID_POISON_SCROLL = 7;
        public const int ITEM_ID_SLEEP_SCROLL = 8;
        public const int ITEM_ID_HASTE_SCROLL = 9;
        public const int ITEM_ID_PARALYZE_SCROLL = 10;
        public const int ITEM_ID_FROZEN_SCROLL = 11;
        public const int ITEM_ID_BURN_SCROLL = 12;

        public const int ITEM_ID_RAT_TAIL = 13;
        public const int ITEM_ID_PIECE_OF_FUR = 14;
        public const int ITEM_ID_SNAKE_FANG = 15;
        public const int ITEM_ID_SNAKESKIN = 16;
        public const int ITEM_ID_SPIDER_FANG = 17;
        public const int ITEM_ID_SPIDER_SILK = 18;

        public const int ITEM_ID_ADVENTURER_PASS = 19;

        public const int WORTHLESS_ITEM_PRICE = 0;
        public const int UNSELLABLE_ITEM_PRICE = -1;

        public const int MONSTER_ID_RAT = 1;
        public const int MONSTER_ID_SNAKE = 2;
        public const int MONSTER_ID_GIANT_SPIDER = 3;

        public const int QUEST_ID_CLEAR_ALCHEMIST_GARDEN = 1;
        public const int QUEST_ID_CLEAR_FARMERS_FIELD = 2;

        public const int VENDOR_ID_BOB_THE_RAT_CATCHER = 1;

        public const int LOCATION_ID_HOME = 1;
        public const int LOCATION_ID_TOWN_SQUARE = 2;
        public const int LOCATION_ID_GUARD_POST = 3;
        public const int LOCATION_ID_ALCHEMIST_HUT = 4;
        public const int LOCATION_ID_ALCHEMISTS_GARDEN = 5;
        public const int LOCATION_ID_FARMHOUSE = 6;
        public const int LOCATION_ID_FARM_FIELD = 7;
        public const int LOCATION_ID_BRIDGE = 8;
        public const int LOCATION_ID_SPIDER_FIELD = 9;

        static World()
        {
            PopulateStatuses();
            PopulateSpells();
            PopulateItems();
            PopulateMonsters();
            PopulateQuests();
            PopulateVendors();
            PopulateLocations();
        }

        private static void PopulateStatuses()
        {
            Statuses.Add(new Status(STATUS_ID_POISON, "Poison", "Poisoned"));
            Statuses.Add(new Status(STATUS_ID_SLEEP, "Sleep", "Asleep"));
            Statuses.Add(new Status(STATUS_ID_HASTE, "Haste", "Hasted"));
            Statuses.Add(new Status(STATUS_ID_PARALYZE, "Paralyze", "Paralyzed"));
            Statuses.Add(new Status(STATUS_ID_FROZEN, "Frozen", "Frozen"));
            Statuses.Add(new Status(STATUS_ID_BURN, "Burn", "Burned"));
        }

        private static void PopulateSpells()
        {
            Spells.Add(new HealingSpell(SPELL_ID_HEAL, "Heal", "Self", 3, 3));

            StatusSpell fireball = new StatusSpell(SPELL_ID_FIREBALL, "Fireball", "Enemy", 3);
            Status poisonStatus = StatusByID(STATUS_ID_POISON).NewInstanceOfStatus(3);
            fireball.StatusApplied = poisonStatus;
            Spells.Add(fireball);

            StatusSpell poison = new StatusSpell(SPELL_ID_POISON, "Poison", "Enemy", 3);
            poisonStatus = StatusByID(STATUS_ID_POISON).NewInstanceOfStatus(1, 3);
            poison.StatusApplied = poisonStatus;
            Spells.Add(poison);

            StatusSpell sleep = new StatusSpell(SPELL_ID_SLEEP, "Sleep", "Enemy", 3);
            Status sleepStatus = StatusByID(STATUS_ID_SLEEP).NewInstanceOfStatus(0, Int32.MaxValue, 100, 25);
            sleep.StatusApplied = sleepStatus;
            Spells.Add(sleep);

            StatusSpell haste = new StatusSpell(SPELL_ID_HASTE, "Haste", "Self", 3);
            Status hasteStatus = StatusByID(STATUS_ID_HASTE).NewInstanceOfStatus(0, 4);
            haste.StatusApplied = hasteStatus;
            Spells.Add(haste);

            StatusSpell paralyze = new StatusSpell(SPELL_ID_PARALYZE, "Paralyze", "Enemy", 3);
            Status paralyzeStatus = StatusByID(STATUS_ID_PARALYZE).NewInstanceOfStatus(0, 10, 25);
            paralyze.StatusApplied = paralyzeStatus;
            Spells.Add(paralyze);

            StatusSpell frozen = new StatusSpell(SPELL_ID_FROZEN, "Frozen", "Enemy", 3);
            Status frozenStatus = StatusByID(STATUS_ID_FROZEN).NewInstanceOfStatus(0, Int32.MaxValue, 100, 25);
            frozen.StatusApplied = frozenStatus;
            Spells.Add(frozen);

            StatusSpell burn = new StatusSpell(SPELL_ID_BURN, "Burn", "Enemy", 3);
            Status burnStatus = StatusByID(STATUS_ID_BURN).NewInstanceOfStatus(1, Int32.MaxValue, 100, 25);
            burn.StatusApplied = burnStatus;
            Spells.Add(burn);
        }

        private static void PopulateItems()
        {
            Items.Add(new Weapon(ITEM_ID_RUSTY_SWORD, "Rusty sword", "Rusty swords", 0, 5, 5, 60));
            Items.Add(new Weapon(ITEM_ID_CLUB, "Club", "Clubs", 3, 10, 8, 80, 2));

            Items.Add(new HealingItem(ITEM_ID_HEALING_POTION, "Healing potion", "Healing potions", 5, 3));

            Items.Add(new Item(ITEM_ID_RAT_TAIL, "Rat tail", "Rat tails", 1));
            Items.Add(new Item(ITEM_ID_PIECE_OF_FUR, "Piece of fur", "Pieces of fur", 1));
            Items.Add(new Item(ITEM_ID_SNAKE_FANG, "Snake fang", "Snake fangs", 1));
            Items.Add(new Item(ITEM_ID_SNAKESKIN, "Snakeskin", "Snakeskins", 2));
            Items.Add(new Item(ITEM_ID_SPIDER_FANG, "Spider fang", "Spider fangs", 1));
            Items.Add(new Item(ITEM_ID_SPIDER_SILK, "Spider silk", "Spider silks", 1));

            Items.Add(new Item(ITEM_ID_ADVENTURER_PASS, "Adventurer pass", "Adventurer passes", UNSELLABLE_ITEM_PRICE));

            StatusItem poison = new StatusItem(ITEM_ID_POISON, "Poison", "Poisons", 3);
            Status poisonStatus = StatusByID(STATUS_ID_POISON).NewInstanceOfStatus(1, 3);
            poison.StatusApplied = poisonStatus;
            Items.Add(poison);

            Items.Add(new Scroll(ITEM_ID_HEAL_SCROLL, SpellByID(SPELL_ID_HEAL), 5));
            Items.Add(new Scroll(ITEM_ID_FIREBALL_SCROLL, SpellByID(SPELL_ID_FIREBALL), 5));
            Items.Add(new Scroll(ITEM_ID_POISON_SCROLL, SpellByID(SPELL_ID_POISON), 5));
            Items.Add(new Scroll(ITEM_ID_SLEEP_SCROLL, SpellByID(SPELL_ID_SLEEP), 5));
            Items.Add(new Scroll(ITEM_ID_HASTE_SCROLL, SpellByID(SPELL_ID_HASTE), 5));
            Items.Add(new Scroll(ITEM_ID_PARALYZE_SCROLL, SpellByID(SPELL_ID_PARALYZE), 5));
            Items.Add(new Scroll(ITEM_ID_FROZEN_SCROLL, SpellByID(SPELL_ID_FROZEN), 5));
            Items.Add(new Scroll(ITEM_ID_BURN_SCROLL, SpellByID(SPELL_ID_BURN), 5));
        }

        private static void PopulateMonsters()
        {
            Monster rat = new Monster(MONSTER_ID_RAT, "Rat", 5, 3, 10, 3, 3, 60, 0);
            Attributes ratAttributes = new Attributes(1, 1, 1, 1);
            rat.Attributes = ratAttributes;
            rat.LootTable.Add(new LootItem(ItemByID(ITEM_ID_PIECE_OF_FUR), 75, true));
            rat.LootTable.Add(new LootItem(ItemByID(ITEM_ID_RAT_TAIL), 75, false));

            Monster snake = new Monster(MONSTER_ID_SNAKE, "Snake", 5, 5, 20, 4, 4, 80, 20);
            Attributes snakeAttributes = new Attributes(3, 3, 3, 3);
            snake.Attributes = snakeAttributes;
            snake.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SNAKESKIN), 75, true));
            snake.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SNAKE_FANG), 75, false));

            Monster giantSpider = new Monster(MONSTER_ID_GIANT_SPIDER, "Giant Spider", 20, 10, 40, 10, 10, 100, 50);
            giantSpider.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SPIDER_FANG), 75, true));
            giantSpider.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SPIDER_SILK), 25, false));
            giantSpider.LootTable.Add(new LootItem(ItemByID(ITEM_ID_CLUB), 25, false));

            Monsters.Add(rat);
            Monsters.Add(snake);
            Monsters.Add(giantSpider);
        }

        private static void PopulateQuests()
        {
            Quest clearAlchemistGarden =
                new Quest(
                    QUEST_ID_CLEAR_ALCHEMIST_GARDEN,
                    "Clear the alchemist's garden",
                    "Kill rats in the alchemist's garden and bring back 3 rat tails. You will receive a healing potion and 10 gold pieces.", 20, 10, true);

            clearAlchemistGarden.QuestCompletionItems.Add(new QuestCompletionItem(ItemByID(ITEM_ID_RAT_TAIL), 3));
            clearAlchemistGarden.RewardItem = ItemByID(ITEM_ID_HEALING_POTION);

            Quest clearFarmersField =
                new Quest(
                    QUEST_ID_CLEAR_FARMERS_FIELD,
                    "Clear the farmer's field",
                    "Kill snakes in the farmer's field and bring back 3 snake fangs. You will receive an adventurer's pass and 20 gold pieces.", 20, 20);

            clearFarmersField.QuestCompletionItems.Add(new QuestCompletionItem(ItemByID(ITEM_ID_SNAKE_FANG), 3));
            clearFarmersField.RewardItem = ItemByID(ITEM_ID_ADVENTURER_PASS);
            clearFarmersField.Prerequisite = clearAlchemistGarden;

            Quests.Add(clearAlchemistGarden);
            Quests.Add(clearFarmersField);
        }

        private static void PopulateVendors()
        {
            Vendor bobTheRatCatcher = new Vendor(VENDOR_ID_BOB_THE_RAT_CATCHER, "Bob the Rat-Catcher");
            bobTheRatCatcher.AddItemToInventory(ItemByID(ITEM_ID_PIECE_OF_FUR), 5);
            bobTheRatCatcher.AddItemToInventory(ItemByID(ITEM_ID_RAT_TAIL), 3);

            Vendors.Add(bobTheRatCatcher);
        }

        private static void PopulateLocations()
        {
            Location home = new Location(LOCATION_ID_HOME, "Home", "Your house. You really need to clean up the place.", "Home");

            Location townSquare = new Location(LOCATION_ID_TOWN_SQUARE, "Town square", "You see a fountain.", "TownSquare");
            townSquare.VendorWorkingHere = VendorByID(VENDOR_ID_BOB_THE_RAT_CATCHER);

            Location alchemistHut = new Location(LOCATION_ID_ALCHEMIST_HUT, "Alchemist's hut", "There are many strange plants on the shelves.", "AlchemistHut");
            alchemistHut.QuestAvailableHere = QuestByID(QUEST_ID_CLEAR_ALCHEMIST_GARDEN);

            Location alchemistsGarden = new Location(LOCATION_ID_ALCHEMISTS_GARDEN, "Alchemist's garden", "Many plants are growing here.", "AlchemistsGarden");
            alchemistsGarden.AddMonster(MONSTER_ID_RAT, 100);

            Location farmhouse = new Location(LOCATION_ID_FARMHOUSE, "Farmhouse", "There is a small farmhouse, with a farmer in front.", "Farmhouse");
            farmhouse.LevelRequiredToEnter = 2;
            farmhouse.QuestAvailableHere = QuestByID(QUEST_ID_CLEAR_FARMERS_FIELD);

            Location farmersField = new Location(LOCATION_ID_FARM_FIELD, "Farmer's field", "You see rows of vegetables growing here.", "FarmersFields");
            farmersField.AddMonster(MONSTER_ID_RAT, 20);
            farmersField.AddMonster(MONSTER_ID_SNAKE, 80);

            Location guardPost = new Location(LOCATION_ID_GUARD_POST, "Guard post", "There is a large, tough-looking guard here.", "GuardPost");
            guardPost.ItemRequiredToEnter = ItemByID(ITEM_ID_ADVENTURER_PASS);

            Location bridge = new Location(LOCATION_ID_BRIDGE, "Bridge", "A stone bridge crosses a wide river.", "Bridge");

            Location spiderField = new Location(LOCATION_ID_SPIDER_FIELD, "Forest", "You see spider webs covering covering the trees in this forest.", "SpiderField");
            spiderField.AddMonster(MONSTER_ID_GIANT_SPIDER, 100);

            home.LocationToNorth = townSquare;

            townSquare.LocationToNorth = alchemistHut;
            townSquare.LocationToSouth = home;
            townSquare.LocationToEast = guardPost;
            townSquare.LocationToWest = farmhouse;

            alchemistHut.LocationToSouth = townSquare;
            alchemistHut.LocationToNorth = alchemistsGarden;

            alchemistsGarden.LocationToSouth = alchemistHut;

            farmhouse.LocationToEast = townSquare;
            farmhouse.LocationToWest = farmersField;

            farmersField.LocationToEast = farmhouse;

            guardPost.LocationToEast = bridge;
            guardPost.LocationToWest = townSquare;

            bridge.LocationToWest = guardPost;
            bridge.LocationToEast = spiderField;

            spiderField.LocationToWest = bridge;

            Locations.Add(home);
            Locations.Add(townSquare);
            Locations.Add(alchemistHut);
            Locations.Add(alchemistsGarden);
            Locations.Add(farmhouse);
            Locations.Add(farmersField);
            Locations.Add(guardPost);
            Locations.Add(bridge);
            Locations.Add(spiderField);
        }

        public static Status StatusByID(int id)
        {
            return Statuses.SingleOrDefault(status => status.ID == id);
        }

        public static Spell SpellByID(int id)
        {
            return Spells.SingleOrDefault(spell => spell.ID == id);
        }

        public static Item ItemByID(int id)
        {
            return Items.SingleOrDefault(item => item.ID == id);
        }

        public static Monster MonsterByID(int id)
        {
            return Monsters.SingleOrDefault(monster => monster.ID == id);
        }

        public static Quest QuestByID(int id)
        {
            return Quests.SingleOrDefault(quest => quest.ID == id);
        }

        public static Location LocationByID(int id)
        {
            return Locations.SingleOrDefault(location => location.ID == id);
        }

        public static Vendor VendorByID(int id)
        {
            return Vendors.SingleOrDefault(vendor => vendor.ID == id);
        }
    }
}
