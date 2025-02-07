namespace Engine
{
    public class Monster : LivingCreature
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int MaximumDamage { get; set; }
        public int RewardGold { get; set; }
        public int RewardExperiencePoints { get; set; }
        public List<LootItem> LootTable { get; set; }
        internal List<InventoryItem> LootItems { get; }

        public Monster(int id, string name, int maximumDamage, int rewardGold, int rewardExperiencePoints, int currentHitPoints, int maximumHitPoints) : base(currentHitPoints, maximumHitPoints)
        {
            ID = id;
            Name = name;
            MaximumDamage = maximumDamage;
            RewardGold = rewardGold;
            RewardExperiencePoints = rewardExperiencePoints;
            LootTable = new List<LootItem>();
            LootItems = new List<InventoryItem>();
        }

        internal Monster NewInstanceOfMonster()
        {
            Monster newMonster = new Monster(ID, Name, MaximumDamage, RewardGold, RewardExperiencePoints, CurrentHitPoints, MaximumHitPoints);

            LootTable.Where(lootItem => RandomNumberGenerator.NumberBetween(1, 100) <= lootItem.DropPercentage).ToList().ForEach(lootItem => newMonster.LootItems.Add(new InventoryItem(lootItem.Details, 1)));
            if (newMonster.LootItems.Count == 0) LootTable.Where(lootItem => lootItem.IsDefaultItem).ToList().ForEach(lootItem => newMonster.LootItems.Add(new InventoryItem(lootItem.Details, 1)));

            return newMonster;
        }
    }
}
