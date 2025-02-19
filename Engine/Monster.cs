using System.ComponentModel;

namespace Engine
{
    public class Monster : LivingCreature
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int MaximumDamage { get; set; }
        public int HitChance { get; set; }
        public int Defence { get; set; }
        public int RewardGold { get; set; }
        public int RewardExperiencePoints { get; set; }
        public List<LootItem> LootTable { get; set; }
        internal List<InventoryItem> LootItems { get; }
        public List<Spell> CastableSpells { get {  return Spellbook.Where(spell => spell.ManaCost <= CurrentMana).ToList(); } }
        public int ChanceToUseItem { get; set; }
        public int ChanceToCastSpell { get; set; }

        public Monster(int id, string name, int maximumDamage, int rewardGold, int rewardExperiencePoints, int currentHitPoints, int maximumHitPoints, int hitChance, int defence, int chanceToUseItem = 0, int chanceToCastSpell = 0, int currentMana = 0, int maximumMana = 0) : base(currentHitPoints, maximumHitPoints, currentMana, maximumMana)
        {
            ID = id;
            Name = name;
            MaximumDamage = maximumDamage;
            RewardGold = rewardGold;
            RewardExperiencePoints = rewardExperiencePoints;
            LootTable = new List<LootItem>();
            LootItems = new List<InventoryItem>();
            HitChance = hitChance;
            Defence = defence;
            ChanceToUseItem = chanceToUseItem;
            ChanceToCastSpell = chanceToCastSpell;
        }

        internal Monster NewInstanceOfMonster()
        {
            Monster newMonster = new Monster(ID, Name, MaximumDamage, RewardGold, RewardExperiencePoints, CurrentHitPoints, MaximumHitPoints, HitChance, Defence, ChanceToUseItem, ChanceToCastSpell, CurrentMana, MaximumMana);
            newMonster.Attributes = Attributes;
            newMonster.Spellbook = Spellbook;

            LootTable.Where(lootItem => RandomNumberGenerator.NumberBetween(1, 100) <= lootItem.DropPercentage).ToList().ForEach(lootItem => newMonster.LootItems.Add(new InventoryItem(lootItem.Details, 1)));
            if (newMonster.LootItems.Count == 0) LootTable.Where(lootItem => lootItem.IsDefaultItem).ToList().ForEach(lootItem => newMonster.LootItems.Add(new InventoryItem(lootItem.Details, 1)));

            Inventory.ToList().ForEach(item => newMonster.Inventory.Add(new InventoryItem(item.Details, item.Quantity)));

            return newMonster;
        }
    }
}
