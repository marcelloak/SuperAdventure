namespace Engine
{
    public class StatusSpell : Spell
    {
        public Status StatusApplied { get; set; }

        public StatusSpell(int id, string name, string target, int manaCost, int minimumLevel = 1, int minimumIntelligence = 1, Status statusApplied = null) : base(id, name, target, manaCost, minimumLevel, minimumIntelligence)
        {
            StatusApplied = statusApplied;
        }
    }
}
