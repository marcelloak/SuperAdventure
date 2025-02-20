namespace Engine
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

        public static int currentID = 1;

        public static int STATUS_ID_POISON;
        public static int STATUS_ID_SLEEP;
        public static int STATUS_ID_HASTE;
        public static int STATUS_ID_PARALYZE;
        public static int STATUS_ID_FROZEN;
        public static int STATUS_ID_BURN;
        public static int STATUS_ID_SLOW;
        public static int STATUS_ID_STOP;
        public static int STATUS_ID_PETRIFY;
        public static int STATUS_ID_DEATH;

        public static int SPELL_ID_HEAL;
        public static int SPELL_ID_FIREBALL;
        public static int SPELL_ID_POISON;
        public static int SPELL_ID_SLEEP;
        public static int SPELL_ID_HASTE;
        public static int SPELL_ID_PARALYZE;
        public static int SPELL_ID_FROZEN;
        public static int SPELL_ID_BURN;
        public static int SPELL_ID_SLOW;
        public static int SPELL_ID_STOP;
        public static int SPELL_ID_PETRIFY;
        public static int SPELL_ID_DEATH;

        public static int ITEM_ID_RUSTY_SWORD;
        public static int ITEM_ID_CLUB;

        public static int ITEM_ID_HEALING_POTION;
        public static int ITEM_ID_POISON;

        public static int ITEM_ID_HEAL_SCROLL;
        public static int ITEM_ID_FIREBALL_SCROLL;
        public static int ITEM_ID_POISON_SCROLL;
        public static int ITEM_ID_SLEEP_SCROLL;
        public static int ITEM_ID_HASTE_SCROLL;
        public static int ITEM_ID_PARALYZE_SCROLL;
        public static int ITEM_ID_FROZEN_SCROLL;
        public static int ITEM_ID_BURN_SCROLL;
        public static int ITEM_ID_SLOW_SCROLL;
        public static int ITEM_ID_STOP_SCROLL;
        public static int ITEM_ID_PETRIFY_SCROLL;
        public static int ITEM_ID_DEATH_SCROLL;

        public static int ITEM_ID_HELM;

        public static int ITEM_ID_RAT_TAIL;
        public static int ITEM_ID_PIECE_OF_FUR;
        public static int ITEM_ID_SNAKE_FANG;
        public static int ITEM_ID_SNAKESKIN;
        public static int ITEM_ID_SPIDER_FANG;
        public static int ITEM_ID_SPIDER_SILK;

        public static int ITEM_ID_ADVENTURER_PASS;

        public const int WORTHLESS_ITEM_PRICE = 0;
        public const int UNSELLABLE_ITEM_PRICE = -1;

        public static int MONSTER_ID_RAT;
        public static int MONSTER_ID_SNAKE;
        public static int MONSTER_ID_GIANT_SPIDER;

        public static int QUEST_ID_CLEAR_ALCHEMIST_GARDEN;
        public static int QUEST_ID_CLEAR_FARMERS_FIELD;

        public static int VENDOR_ID_BOB_THE_RAT_CATCHER;

        public static int LOCATION_ID_HOME;
        public static int LOCATION_ID_TOWN_SQUARE;
        public static int LOCATION_ID_GUARD_POST;
        public static int LOCATION_ID_ALCHEMIST_HUT;
        public static int LOCATION_ID_ALCHEMISTS_GARDEN;
        public static int LOCATION_ID_FARMHOUSE;
        public static int LOCATION_ID_FARM_FIELD;
        public static int LOCATION_ID_BRIDGE;
        public static int LOCATION_ID_SPIDER_FIELD;

        static World()
        {
            PopulateIDs();
            PopulateStatuses();
            PopulateSpells();
            PopulateItems();
            PopulateMonsters();
            PopulateQuests();
            PopulateVendors();
            PopulateLocations();
        }

        private static int NextID()
        {
            return currentID++;
        }

        private static void PopulateIDs()
        {
            STATUS_ID_POISON = NextID();
            STATUS_ID_SLEEP = NextID();
            STATUS_ID_HASTE = NextID();
            STATUS_ID_PARALYZE = NextID();
            STATUS_ID_FROZEN = NextID();
            STATUS_ID_BURN = NextID();
            STATUS_ID_SLOW = NextID();
            STATUS_ID_STOP = NextID();
            STATUS_ID_PETRIFY = NextID();
            STATUS_ID_DEATH = NextID();

            SPELL_ID_HEAL = NextID();
            SPELL_ID_FIREBALL = NextID();
            SPELL_ID_POISON = NextID();
            SPELL_ID_SLEEP = NextID();
            SPELL_ID_HASTE = NextID();
            SPELL_ID_PARALYZE = NextID();
            SPELL_ID_FROZEN = NextID();
            SPELL_ID_BURN = NextID();
            SPELL_ID_SLOW = NextID();
            SPELL_ID_STOP = NextID();
            SPELL_ID_PETRIFY = NextID();
            SPELL_ID_DEATH = NextID();

            ITEM_ID_RUSTY_SWORD = NextID();
            ITEM_ID_CLUB = NextID();

            ITEM_ID_HEALING_POTION = NextID();
            ITEM_ID_POISON = NextID();

            ITEM_ID_HEAL_SCROLL = NextID();
            ITEM_ID_FIREBALL_SCROLL = NextID();
            ITEM_ID_POISON_SCROLL = NextID();
            ITEM_ID_SLEEP_SCROLL = NextID();
            ITEM_ID_HASTE_SCROLL = NextID();
            ITEM_ID_PARALYZE_SCROLL = NextID();
            ITEM_ID_FROZEN_SCROLL = NextID();
            ITEM_ID_BURN_SCROLL = NextID();
            ITEM_ID_SLOW_SCROLL = NextID();
            ITEM_ID_STOP_SCROLL = NextID();
            ITEM_ID_PETRIFY_SCROLL = NextID();
            ITEM_ID_DEATH_SCROLL = NextID();

            ITEM_ID_HELM = NextID();

            ITEM_ID_RAT_TAIL = NextID();
            ITEM_ID_PIECE_OF_FUR = NextID();
            ITEM_ID_SNAKE_FANG = NextID();
            ITEM_ID_SNAKESKIN = NextID();
            ITEM_ID_SPIDER_FANG = NextID();
            ITEM_ID_SPIDER_SILK = NextID();

            ITEM_ID_ADVENTURER_PASS = NextID();

            MONSTER_ID_RAT = NextID();
            MONSTER_ID_SNAKE = NextID();
            MONSTER_ID_GIANT_SPIDER = NextID();

            QUEST_ID_CLEAR_ALCHEMIST_GARDEN = NextID();
            QUEST_ID_CLEAR_FARMERS_FIELD = NextID();

            VENDOR_ID_BOB_THE_RAT_CATCHER = NextID();

            LOCATION_ID_HOME = NextID();
            LOCATION_ID_TOWN_SQUARE = NextID();
            LOCATION_ID_GUARD_POST = NextID();
            LOCATION_ID_ALCHEMIST_HUT = NextID();
            LOCATION_ID_ALCHEMISTS_GARDEN = NextID();
            LOCATION_ID_FARMHOUSE = NextID();
            LOCATION_ID_FARM_FIELD = NextID();
            LOCATION_ID_BRIDGE = NextID();
            LOCATION_ID_SPIDER_FIELD = NextID();
        }

        private static void PopulateStatuses()
        {
            Statuses.Add(new Status(STATUS_ID_POISON, "Poison", "Poisoned", "Deals damage each turn for a set amount of turns."));
            Statuses.Add(new Status(STATUS_ID_SLEEP, "Sleep", "Asleep", "Skips turns and has a chance to wear off each turn."));
            Statuses.Add(new Status(STATUS_ID_HASTE, "Haste", "Hasted", "Increases speed for a set amount of turns."));
            Statuses.Add(new Status(STATUS_ID_PARALYZE, "Paralyze", "Paralyzed", "Chance to skip turn each turn for a set amount of turns."));
            Statuses.Add(new Status(STATUS_ID_FROZEN, "Frozen", "Frozen", "Skips turns and has a chance to wear off each turn."));
            Statuses.Add(new Status(STATUS_ID_BURN, "Burn", "Burned", "Deals damage and has a chance to wear off each turn."));
            Statuses.Add(new Status(STATUS_ID_SLOW, "Slow", "Slowed", "Decreases speed for a set amount of turns."));
            Statuses.Add(new Status(STATUS_ID_STOP, "Stop", "Stopped", "Skips turns for a set amount of turns."));
            Statuses.Add(new Status(STATUS_ID_PETRIFY, "Petrify", "Petrified", "Kills you after a set amount of turns."));
            Statuses.Add(new Status(STATUS_ID_DEATH, "Death", "Cursed", "Kills you after a set amount of turns."));
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

            StatusSpell slow = new StatusSpell(SPELL_ID_SLOW, "Slow", "Enemy", 3);
            Status slowStatus = StatusByID(STATUS_ID_SLOW).NewInstanceOfStatus(0, 4);
            slow.StatusApplied = slowStatus;
            Spells.Add(slow);

            StatusSpell stop = new StatusSpell(SPELL_ID_STOP, "Stop", "Enemy", 3);
            Status stopStatus = StatusByID(STATUS_ID_STOP).NewInstanceOfStatus(0, 4);
            stop.StatusApplied = stopStatus;
            Spells.Add(stop);

            StatusSpell petrify = new StatusSpell(SPELL_ID_PETRIFY, "Petrify", "Enemy", 3);
            Status petrifyStatus = StatusByID(STATUS_ID_PETRIFY).NewInstanceOfStatus(0, 4);
            petrify.StatusApplied = petrifyStatus;
            Spells.Add(petrify);

            StatusSpell death = new StatusSpell(SPELL_ID_DEATH, "Death", "Enemy", 3);
            Status deathStatus = StatusByID(STATUS_ID_DEATH).NewInstanceOfStatus(0, 4);
            death.StatusApplied = deathStatus;
            Spells.Add(death);
        }

        private static void PopulateItems()
        {
            Items.Add(new Weapon(ITEM_ID_RUSTY_SWORD, "Rusty sword", "Rusty swords", 0, 5, 5, 60));
            Items.Add(new Weapon(ITEM_ID_CLUB, "Club", "Clubs", 3, 10, 8, 80, 2));

            Items.Add(new HealingItem(ITEM_ID_HEALING_POTION, "Healing potion", "Healing potions", 5, 3));
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
            Items.Add(new Scroll(ITEM_ID_SLOW_SCROLL, SpellByID(SPELL_ID_SLOW), 5));
            Items.Add(new Scroll(ITEM_ID_STOP_SCROLL, SpellByID(SPELL_ID_STOP), 5));
            Items.Add(new Scroll(ITEM_ID_PETRIFY_SCROLL, SpellByID(SPELL_ID_PETRIFY), 5));
            Items.Add(new Scroll(ITEM_ID_DEATH_SCROLL, SpellByID(SPELL_ID_DEATH), 5));

            Equipment helm = new Equipment(ITEM_ID_HELM, "Helm", "Helms", 5, 2);
            Attributes helmAttributes = new Attributes(2, 2, 0, 0);
            helm.AttributesIncreased = helmAttributes;
            Items.Add(helm);

            Items.Add(new Item(ITEM_ID_RAT_TAIL, "Rat tail", "Rat tails", 1));
            Items.Add(new Item(ITEM_ID_PIECE_OF_FUR, "Piece of fur", "Pieces of fur", 1));
            Items.Add(new Item(ITEM_ID_SNAKE_FANG, "Snake fang", "Snake fangs", 1));
            Items.Add(new Item(ITEM_ID_SNAKESKIN, "Snakeskin", "Snakeskins", 2));
            Items.Add(new Item(ITEM_ID_SPIDER_FANG, "Spider fang", "Spider fangs", 1));
            Items.Add(new Item(ITEM_ID_SPIDER_SILK, "Spider silk", "Spider silks", 1));

            Items.Add(new Item(ITEM_ID_ADVENTURER_PASS, "Adventurer pass", "Adventurer passes", UNSELLABLE_ITEM_PRICE));
        }

        private static void PopulateMonsters()
        {
            Monster rat = new Monster(MONSTER_ID_RAT, "Rat", 3, 3, 10, 3, 3, 60, 0);
            Attributes ratAttributes = new Attributes(1, 1, 1, 1);
            rat.BaseAttributes = ratAttributes;
            rat.LootTable.Add(new LootItem(ItemByID(ITEM_ID_PIECE_OF_FUR), 75, true));
            rat.LootTable.Add(new LootItem(ItemByID(ITEM_ID_RAT_TAIL), 75, false));

            Monster snake = new Monster(MONSTER_ID_SNAKE, "Snake", 5, 5, 20, 4, 4, 80, 20);
            Attributes snakeAttributes = new Attributes(3, 3, 3, 3);
            snake.BaseAttributes = snakeAttributes;
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
