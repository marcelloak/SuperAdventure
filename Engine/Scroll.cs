namespace Engine
{
    public class Scroll : UsableItem
    {
        public Spell SpellContained { get; set; }

        public Scroll(int id, Spell spellContained, int price, int minimumLevel = 1) : base(id, spellContained.Name + " scroll", spellContained.Name + " scrolls", price, minimumLevel)
        {
            SpellContained = spellContained;
        }
    }
}
