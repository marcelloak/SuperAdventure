namespace Engine
{
    public class Spell
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Target { get; set; }
        public int ManaCost { get; set; }
        public int MinimumLevel { get; set; }
        public int MinimumIntelligence { get; set; }

        public Spell(int id, string name, string target, int manaCost, int minimumLevel = 1, int minimumIntelligence = 1)
        {
            ID = id;
            Name = name;
            Target = target;
            ManaCost = manaCost;
            MinimumLevel = minimumLevel;
            MinimumIntelligence = minimumIntelligence;
        }
    }
}
