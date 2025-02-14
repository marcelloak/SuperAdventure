namespace Engine
{
    public class HealingSpell : Spell
    {
        public int AmountToHeal { get; set; }

        public HealingSpell(int id, string name, string target, int manaCost, int amountToHeal, int minimumLevel = 1, int minimumIntelligence = 1) : base(id, name, target, manaCost, minimumLevel, minimumIntelligence)
        {
            AmountToHeal = amountToHeal;
        }
    }
}
