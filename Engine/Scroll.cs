namespace Engine
{
    public class Scroll : UsableItem
    {
        public Spell SpellContained { get; set; }

        public Scroll(int id, string name, string namePlural, int price, int minimumLevel = 1, Spell spellContained = null) : base(id, name, namePlural, price, minimumLevel)
        {
            SpellContained = spellContained;
        }
    }
}
